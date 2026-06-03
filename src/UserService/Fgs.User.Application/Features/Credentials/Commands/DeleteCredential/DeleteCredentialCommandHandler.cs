using Fgs.Foundation.Result;
using Fgs.User.Application.Features.Credentials.Services;
using Fgs.User.Domain.Enums;
using MediatR;

namespace Fgs.User.Application.Features.Credentials.Commands.DeleteCredential;

public sealed class DeleteCredentialCommandHandler : IRequestHandler<DeleteCredentialCommand, ApiResponse<object>>
{
    private readonly CredentialMutationService _mutationService;

    public DeleteCredentialCommandHandler(CredentialMutationService mutationService) =>
        _mutationService = mutationService;

    public async Task<ApiResponse<object>> Handle(
        DeleteCredentialCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            switch (request.Scope)
            {
                case CredentialScope.Global when CredentialRequestHelpers.TryParseGlobalId(request.Id, out var globalId):
                    await _mutationService.DeleteGlobalAsync(globalId, cancellationToken);
                    break;
                case CredentialScope.Tenant when CredentialRequestHelpers.TryParseTenantId(request.Id, out var tenantId):
                    await _mutationService.DeleteTenantAsync(tenantId, cancellationToken);
                    break;
                default:
                    return ApiResponse<object>.Fail([CredentialErrorMessages.InvalidScope], ApiStatusCodes.BadRequest);
            }

            return ApiResponse<object>.Ok(new { deleted = true });
        }
        catch (Exception ex)
        {
            return CredentialRequestHelpers.MapException<object>(ex);
        }
    }
}
