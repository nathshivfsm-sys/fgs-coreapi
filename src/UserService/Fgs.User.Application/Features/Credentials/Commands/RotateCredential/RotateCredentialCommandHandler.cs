using Fgs.Foundation.Result;
using Fgs.User.Application.Features.Credentials.DTOs;
using Fgs.User.Application.Features.Credentials.Services;
using Fgs.User.Domain.Enums;
using MediatR;

namespace Fgs.User.Application.Features.Credentials.Commands.RotateCredential;

public sealed class RotateCredentialCommandHandler
    : IRequestHandler<RotateCredentialCommand, ApiResponse<CredentialMutationResultDto>>
{
    private readonly CredentialMutationService _mutationService;

    public RotateCredentialCommandHandler(CredentialMutationService mutationService) =>
        _mutationService = mutationService;

    public async Task<ApiResponse<CredentialMutationResultDto>> Handle(
        RotateCredentialCommand request,
        CancellationToken cancellationToken)
    {
        try
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
        catch (Exception ex)
        {
            return CredentialRequestHelpers.MapException<CredentialMutationResultDto>(ex);
        }
    }

    private async Task<ApiResponse<CredentialMutationResultDto>> RotateGlobalAsync(
        int id,
        RotateCredentialCommand request,
        CancellationToken cancellationToken)
    {
        var credential = await _mutationService.RotateGlobalAsync(id, request.RotationMode, cancellationToken);
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
        var credential = await _mutationService.RotateTenantAsync(id, request.RotationMode, cancellationToken);
        return ApiResponse<CredentialMutationResultDto>.Ok(
            CredentialRequestHelpers.ToMutationResult(
                CredentialScope.Tenant,
                credential.Id.ToString("D"),
                credential.ProviderType.ProviderCode,
                credential.CredentialName));
    }
}
