using Fgs.Foundation.Result;
using Fgs.Setup.Application.Features.Credentials.DTOs;
using Fgs.Setup.Application.Features.Credentials.Services;
using Fgs.Setup.Domain.Enums;
using MediatR;

namespace Fgs.Setup.Application.Features.Credentials.Commands.CreateCredential;

public sealed class CreateCredentialCommandHandler
    : IRequestHandler<CreateCredentialCommand, ApiResponse<CredentialMutationResultDto>>
{
    private readonly CredentialMutationService _mutationService;

    public CreateCredentialCommandHandler(CredentialMutationService mutationService) =>
        _mutationService = mutationService;

    public async Task<ApiResponse<CredentialMutationResultDto>> Handle(
        CreateCredentialCommand request,
        CancellationToken cancellationToken)
    {
        try
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
        catch (Exception ex)
        {
            return CredentialRequestHelpers.MapException<CredentialMutationResultDto>(ex);
        }
    }

    private async Task<ApiResponse<CredentialMutationResultDto>> CreateGlobalAsync(
        CreateCredentialCommand request,
        byte[] payload,
        CancellationToken cancellationToken)
    {
        var (credential, providerCode) = await _mutationService.CreateGlobalAsync(
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
        if (!request.TenantId.HasValue || !request.CompanyId.HasValue)
        {
            return ApiResponse<CredentialMutationResultDto>.Fail(
                [CredentialErrorMessages.TenantContextRequired],
                ApiStatusCodes.BadRequest);
        }

        var (credential, providerCode) = await _mutationService.CreateTenantAsync(
            request.TenantId.Value,
            request.CompanyId.Value,
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
