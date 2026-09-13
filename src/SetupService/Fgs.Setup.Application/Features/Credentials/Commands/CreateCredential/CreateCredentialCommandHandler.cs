using Fgs.Contracts.Api;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Features.Credentials.DTOs;
using Fgs.Setup.Application.Features.Credentials.Services;
using Fgs.Setup.Domain.Enums;
using MediatR;

namespace Fgs.Setup.Application.Features.Credentials.Commands.CreateCredential;

public sealed class CreateCredentialCommandHandler(
    CredentialMutationService mutationService,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<CreateCredentialCommand, ApiResponse<CredentialMutationResultDto>>
{
    public async Task<ApiResponse<CredentialMutationResultDto>> Handle(
        CreateCredentialCommand request,
        CancellationToken cancellationToken)
    {
        var payload = CredentialRequestHelpers.ParsePayload(request.Payload);

        return request.Scope switch
        {
            CredentialScope.Global => await CreateGlobalAsync(request, payload, cancellationToken),
            CredentialScope.Tenant => await CreateTenantAsync(request, payload, cancellationToken),
            _ => ApiResponse<CredentialMutationResultDto>.Fail(
                [CredentialErrorMessages.InvalidScope],
                ApiStatusCodes.BadRequest)
        };
    }

    private async Task<ApiResponse<CredentialMutationResultDto>> CreateGlobalAsync(
        CreateCredentialCommand request,
        byte[] payload,
        CancellationToken cancellationToken)
    {
        var (credential, providerCode) = await mutationService.CreateGlobalAsync(
            request.ProviderCode,
            request.CredentialName,
            request.Description,
            payload,
            cancellationToken);

        return ApiResponse<CredentialMutationResultDto>.Ok(
            CredentialRequestHelpers.ToMutationResult(
                CredentialScope.Global,
                credential.Id.ToString(),
                providerCode,
                credential.CredentialName),
            ApiStatusCodes.Created);
    }

    private async Task<ApiResponse<CredentialMutationResultDto>> CreateTenantAsync(
        CreateCredentialCommand request,
        byte[] payload,
        CancellationToken cancellationToken)
    {
        if (tenantContextAccessor.Current is not { } tenantScope)
        {
            return ApiResponse<CredentialMutationResultDto>.Fail(
                [CredentialErrorMessages.TenantContextRequired],
                ApiStatusCodes.BadRequest);
        }

        if (request.TenantId is { } bodyTenant && bodyTenant != tenantScope.TenantId)
        {
            return ApiResponse<CredentialMutationResultDto>.Fail(
                [CredentialErrorMessages.TenantScopeMismatch],
                ApiStatusCodes.Forbidden);
        }

        if (request.CompanyId is { } bodyCompany && bodyCompany != tenantScope.CompanyId)
        {
            return ApiResponse<CredentialMutationResultDto>.Fail(
                [CredentialErrorMessages.TenantScopeMismatch],
                ApiStatusCodes.Forbidden);
        }

        var (credential, providerCode) = await mutationService.CreateTenantAsync(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            request.ProviderCode,
            request.CredentialName,
            request.Description,
            payload,
            cancellationToken);

        return ApiResponse<CredentialMutationResultDto>.Ok(
            CredentialRequestHelpers.ToMutationResult(
                CredentialScope.Tenant,
                credential.Id.ToString("D"),
                providerCode,
                credential.CredentialName),
            ApiStatusCodes.Created);
    }
}
