using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.Credentials.DTOs;
using Fgs.Setup.Application.Features.Credentials.Services;
using Fgs.Setup.Domain.Enums;
using MediatR;

namespace Fgs.Setup.Application.Features.Credentials.Commands.UpdateCredential;

public sealed class UpdateCredentialCommandHandler
    : IRequestHandler<UpdateCredentialCommand, ApiResponse<CredentialMutationResultDto>>
{
    private readonly CredentialMutationService _mutationService;

    public UpdateCredentialCommandHandler(CredentialMutationService mutationService) =>
        _mutationService = mutationService;

    public async Task<ApiResponse<CredentialMutationResultDto>> Handle(
        UpdateCredentialCommand request,
        CancellationToken cancellationToken)
    {
        byte[]? payload = request.Payload is null ? null : CredentialRequestHelpers.ParsePayload(request.Payload);

        return request.Scope switch
        {
            CredentialScope.Global when CredentialRequestHelpers.TryParseGlobalId(request.Id, out var globalId) =>
                await UpdateGlobalAsync(globalId, request, payload, cancellationToken),
            CredentialScope.Tenant when CredentialRequestHelpers.TryParseTenantId(request.Id, out var tenantCredentialId) =>
                await UpdateTenantAsync(tenantCredentialId, request, payload, cancellationToken),
            _ => ApiResponse<CredentialMutationResultDto>.Fail(
                [CredentialErrorMessages.InvalidScope],
                ApiStatusCodes.BadRequest)
        };
    }

    private async Task<ApiResponse<CredentialMutationResultDto>> UpdateGlobalAsync(
        int id,
        UpdateCredentialCommand request,
        byte[]? payload,
        CancellationToken cancellationToken)
    {
        var credential = await _mutationService.UpdateGlobalAsync(
            id,
            request.CredentialName,
            request.Description,
            payload,
            request.IsActive,
            cancellationToken);

        return ApiResponse<CredentialMutationResultDto>.Ok(
            CredentialRequestHelpers.ToMutationResult(
                CredentialScope.Global,
                credential.Id.ToString(),
                credential.ProviderType.ProviderCode,
                credential.CredentialName));
    }

    private async Task<ApiResponse<CredentialMutationResultDto>> UpdateTenantAsync(
        Guid id,
        UpdateCredentialCommand request,
        byte[]? payload,
        CancellationToken cancellationToken)
    {
        var credential = await _mutationService.UpdateTenantAsync(
            id,
            request.CredentialName,
            request.Description,
            payload,
            request.IsActive,
            cancellationToken);

        return ApiResponse<CredentialMutationResultDto>.Ok(
            CredentialRequestHelpers.ToMutationResult(
                CredentialScope.Tenant,
                credential.Id.ToString("D"),
                credential.ProviderType.ProviderCode,
                credential.CredentialName));
    }
}
