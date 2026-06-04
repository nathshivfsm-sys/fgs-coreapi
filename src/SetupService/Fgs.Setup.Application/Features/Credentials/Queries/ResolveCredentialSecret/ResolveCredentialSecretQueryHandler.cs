using System.Text;
using Fgs.Contracts.Api;
using Fgs.Setup.Application.Abstractions.Credentials;
using Fgs.Setup.Application.Features.Credentials.DTOs;
using Fgs.Setup.Application.Features.Credentials.Services;
using Fgs.Setup.Domain.Enums;
using MediatR;

namespace Fgs.Setup.Application.Features.Credentials.Queries.ResolveCredentialSecret;

public sealed class ResolveCredentialSecretQueryHandler
    : IRequestHandler<ResolveCredentialSecretQuery, ApiResponse<CredentialSecretDto>>
{
    private readonly ICredentialRepository _repository;
    private readonly CredentialMutationService _mutationService;
    private readonly ICredentialSecretAccessPolicy _secretAccessPolicy;

    public ResolveCredentialSecretQueryHandler(
        ICredentialRepository repository,
        CredentialMutationService mutationService,
        ICredentialSecretAccessPolicy secretAccessPolicy)
    {
        _repository = repository;
        _mutationService = mutationService;
        _secretAccessPolicy = secretAccessPolicy;
    }

    public async Task<ApiResponse<CredentialSecretDto>> Handle(
        ResolveCredentialSecretQuery request,
        CancellationToken cancellationToken)
    {
        if (!_secretAccessPolicy.IsSecretResolutionAllowed())
        {
            return ApiResponse<CredentialSecretDto>.Fail(
                [CredentialErrorMessages.SecretResolveDisabled],
                ApiStatusCodes.Forbidden);
        }

        try
        {
            return request.Scope switch
            {
                CredentialScope.Global when CredentialRequestHelpers.TryParseGlobalId(request.Id, out var globalId) =>
                    await ResolveGlobalAsync(globalId, cancellationToken),
                CredentialScope.Tenant when CredentialRequestHelpers.TryParseTenantId(request.Id, out var tenantId) =>
                    await ResolveTenantAsync(tenantId, cancellationToken),
                _ => ApiResponse<CredentialSecretDto>.Fail([CredentialErrorMessages.InvalidScope], ApiStatusCodes.BadRequest)
            };
        }
        catch (Exception ex)
        {
            return CredentialRequestHelpers.MapException<CredentialSecretDto>(ex);
        }
    }

    private async Task<ApiResponse<CredentialSecretDto>> ResolveGlobalAsync(int id, CancellationToken cancellationToken)
    {
        var credential = await _repository.GetGlobalByIdAsync(id, cancellationToken);
        if (credential is null)
        {
            return ApiResponse<CredentialSecretDto>.Fail([CredentialErrorMessages.GlobalCredentialNotFound], ApiStatusCodes.NotFound);
        }

        var plaintext = await _mutationService.DecryptGlobalAsync(credential, cancellationToken);
        return ApiResponse<CredentialSecretDto>.Ok(new CredentialSecretDto(
            CredentialScope.Global,
            credential.Id.ToString(),
            credential.ProviderType.ProviderCode,
            Encoding.UTF8.GetString(plaintext)));
    }

    private async Task<ApiResponse<CredentialSecretDto>> ResolveTenantAsync(Guid id, CancellationToken cancellationToken)
    {
        var credential = await _repository.GetTenantByIdAsync(id, cancellationToken);
        if (credential is null)
        {
            return ApiResponse<CredentialSecretDto>.Fail([CredentialErrorMessages.TenantCredentialNotFound], ApiStatusCodes.NotFound);
        }

        var plaintext = await _mutationService.DecryptTenantAsync(credential, cancellationToken);
        return ApiResponse<CredentialSecretDto>.Ok(new CredentialSecretDto(
            CredentialScope.Tenant,
            credential.Id.ToString("D"),
            credential.ProviderType.ProviderCode,
            Encoding.UTF8.GetString(plaintext)));
    }
}

