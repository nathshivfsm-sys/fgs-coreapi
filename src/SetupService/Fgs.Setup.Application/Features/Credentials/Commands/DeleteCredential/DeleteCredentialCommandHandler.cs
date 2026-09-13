using Fgs.Contracts.Api;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Credentials;
using Fgs.Setup.Application.Features.Credentials.Services;
using Fgs.Setup.Domain.Enums;
using MediatR;

namespace Fgs.Setup.Application.Features.Credentials.Commands.DeleteCredential;

public sealed class DeleteCredentialCommandHandler(
    CredentialMutationService mutationService,
    ICredentialRepository repository,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<DeleteCredentialCommand, ApiResponse<object>>
{
    public async Task<ApiResponse<object>> Handle(
        DeleteCredentialCommand request,
        CancellationToken cancellationToken)
    {
        switch (request.Scope)
        {
            case CredentialScope.Global when CredentialRequestHelpers.TryParseGlobalId(request.Id, out var globalId):
                await mutationService.DeleteGlobalAsync(globalId, cancellationToken);
                break;
            case CredentialScope.Tenant when CredentialRequestHelpers.TryParseTenantId(request.Id, out var tenantId):
                var existing = await repository.GetTenantByIdAsync(tenantId, cancellationToken);
                if (existing is null)
                {
                    return ApiResponse<object>.Fail(
                        [CredentialErrorMessages.TenantCredentialNotFound],
                        ApiStatusCodes.NotFound);
                }

                var scopeDenied = CredentialRequestHelpers.EnsureTenantScope<object>(
                    tenantContextAccessor,
                    existing);
                if (scopeDenied is not null)
                {
                    return scopeDenied;
                }

                await mutationService.DeleteTenantAsync(tenantId, cancellationToken);
                break;
            default:
                return ApiResponse<object>.Fail([CredentialErrorMessages.InvalidScope], ApiStatusCodes.BadRequest);
        }

        return ApiResponse<object>.Ok(new { deleted = true });
    }
}
