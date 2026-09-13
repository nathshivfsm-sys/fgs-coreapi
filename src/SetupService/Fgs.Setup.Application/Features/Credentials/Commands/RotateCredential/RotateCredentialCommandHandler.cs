using Fgs.Contracts.Api;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Credentials;
using Fgs.Setup.Application.Features.Credentials.DTOs;
using Fgs.Setup.Application.Features.Credentials.Services;
using Fgs.Setup.Domain.Enums;
using MediatR;

namespace Fgs.Setup.Application.Features.Credentials.Commands.RotateCredential;

public sealed class RotateCredentialCommandHandler(
    CredentialMutationService mutationService,
    ICredentialRepository repository,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<RotateCredentialCommand, ApiResponse<CredentialMutationResultDto>>
{
    public async Task<ApiResponse<CredentialMutationResultDto>> Handle(
        RotateCredentialCommand request,
        CancellationToken cancellationToken)
    {
        return request.Scope switch
        {
            CredentialScope.Global when CredentialRequestHelpers.TryParseGlobalId(request.Id, out var globalId) =>
                await RotateGlobalAsync(globalId, request, cancellationToken),
            CredentialScope.Tenant when CredentialRequestHelpers.TryParseTenantId(request.Id, out var tenantId) =>
                await RotateTenantAsync(tenantId, request, cancellationToken),
            _ => ApiResponse<CredentialMutationResultDto>.Fail(
                [CredentialErrorMessages.InvalidScope],
                ApiStatusCodes.BadRequest)
        };
    }

    private async Task<ApiResponse<CredentialMutationResultDto>> RotateGlobalAsync(
        int id,
        RotateCredentialCommand request,
        CancellationToken cancellationToken)
    {
        var credential = await mutationService.RotateGlobalAsync(id, request.RotationMode, cancellationToken);
        return ApiResponse<CredentialMutationResultDto>.Ok(
            CredentialRequestHelpers.ToMutationResult(
                CredentialScope.Global,
                credential.Id.ToString(),
                credential.ProviderType.ProviderCode,
                credential.CredentialName));
    }

    private async Task<ApiResponse<CredentialMutationResultDto>> RotateTenantAsync(
        Guid id,
        RotateCredentialCommand request,
        CancellationToken cancellationToken)
    {
        var existing = await repository.GetTenantByIdAsync(id, cancellationToken);
        if (existing is null)
        {
            return ApiResponse<CredentialMutationResultDto>.Fail(
                [CredentialErrorMessages.TenantCredentialNotFound],
                ApiStatusCodes.NotFound);
        }

        var scopeDenied = CredentialRequestHelpers.EnsureTenantScope<CredentialMutationResultDto>(
            tenantContextAccessor,
            existing);
        if (scopeDenied is not null)
        {
            return scopeDenied;
        }

        var credential = await mutationService.RotateTenantAsync(id, request.RotationMode, cancellationToken);
        return ApiResponse<CredentialMutationResultDto>.Ok(
            CredentialRequestHelpers.ToMutationResult(
                CredentialScope.Tenant,
                credential.Id.ToString("D"),
                credential.ProviderType.ProviderCode,
                credential.CredentialName));
    }
}
