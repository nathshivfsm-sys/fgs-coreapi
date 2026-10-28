using System.Text;
using System.Text.Json;
using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Contracts.CredentialAudit;
using Fgs.MultiTenancy;
using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.Credentials;
using Fgs.Setup.Application.Features.Credentials.Services;
using MediatR;

namespace Fgs.Setup.Application.Features.Credentials.Queries.GetCompanyProviderCredential;

public sealed class GetCompanyProviderCredentialQueryHandler(
    ICredentialRepository repository,
    CredentialMutationService mutationService,
    ICredentialAuditRecorder auditRecorder,
    IUnitOfWork unitOfWork,
    ITenantContextAccessor tenantContextAccessor,
    ICredentialActorResolver actorResolver)
    : IRequestHandler<GetCompanyProviderCredentialQuery, ApiResponse<CompanyProviderCredentialDto>>
{
    public async Task<ApiResponse<CompanyProviderCredentialDto>> Handle(
        GetCompanyProviderCredentialQuery request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.ProviderCode))
        {
            return ApiResponse<CompanyProviderCredentialDto>.Fail(
                ["providerCode is required."],
                ApiStatusCodes.BadRequest);
        }

        if (tenantContextAccessor.Current is not { } scope)
        {
            return ApiResponse<CompanyProviderCredentialDto>.Fail(
                ["Tenant context is required."],
                ApiStatusCodes.BadRequest);
        }

        var providerType = await repository.GetProviderTypeByCodeAsync(request.ProviderCode.Trim(), cancellationToken);
        if (providerType is null || !providerType.IsActive)
        {
            return NotFound();
        }

        var credential = await repository.GetTenantByProviderTypeAsync(
            scope.TenantId,
            scope.CompanyId,
            providerType.Id,
            cancellationToken);
        if (credential is null || !credential.IsActive)
        {
            return NotFound();
        }

        var plaintext = await mutationService.DecryptTenantAsync(credential, cancellationToken);
        if (!TryRead(Encoding.UTF8.GetString(plaintext), out var settings))
        {
            return NotFound();
        }

        await auditRecorder.RecordAsync(
            new RecordCredentialAuditRequest(
                credential.TenantId,
                credential.CompanyId,
                credential.Id,
                CredentialAuditActions.SecretAccessed,
                $"Company provider credential accessed; ProviderCode={providerType.ProviderCode}",
                CreatedBy: actorResolver.ResolveActorId()),
            cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<CompanyProviderCredentialDto>.Ok(settings!);
    }

    private static ApiResponse<CompanyProviderCredentialDto> NotFound() =>
        ApiResponse<CompanyProviderCredentialDto>.Fail(
            ["Company provider credential was not found."],
            ApiStatusCodes.NotFound);

    private static bool TryRead(string json, out CompanyProviderCredentialDto? settings)
    {
        settings = null;
        try
        {
            using var document = JsonDocument.Parse(json);
            if (document.RootElement.ValueKind != JsonValueKind.Object)
            {
                return false;
            }

            var root = document.RootElement;
            if (!TryString(root, "ApiLoginId", out var apiLoginId)
                || !TryString(root, "PublicClientKey", out var publicClientKey)
                || !TryString(root, "TransactionKey", out var transactionKey)
                || !root.TryGetProperty("UseSandbox", out var useSandbox)
                || useSandbox.ValueKind is not (JsonValueKind.True or JsonValueKind.False))
            {
                return false;
            }

            settings = new CompanyProviderCredentialDto(
                apiLoginId,
                publicClientKey,
                transactionKey,
                useSandbox.GetBoolean());
            return true;
        }
        catch (JsonException)
        {
            return false;
        }
    }

    private static bool TryString(JsonElement root, string name, out string value)
    {
        value = string.Empty;
        if (!root.TryGetProperty(name, out var property) || property.ValueKind != JsonValueKind.String)
        {
            return false;
        }

        var text = property.GetString();
        if (string.IsNullOrWhiteSpace(text))
        {
            return false;
        }

        value = text.Trim();
        return true;
    }
}
