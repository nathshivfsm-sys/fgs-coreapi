using Fgs.Contracts.Api;
using Fgs.File.Application.Abstractions.Provisioning;
using MediatR;

namespace Fgs.File.Application.Features.TenantStorage.Commands.InitializeTenantFolders;

public sealed class InitializeTenantFoldersCommandHandler(ITenantS3BucketProvisioner provisioner)
    : IRequestHandler<InitializeTenantFoldersCommand, ApiResponse<object>>
{
    public async Task<ApiResponse<object>> Handle(
        InitializeTenantFoldersCommand request,
        CancellationToken cancellationToken)
    {
        await provisioner.InitializeFolderStructureAsync(
            request.Request.BucketName,
            request.Request.TenantId,
            request.Request.CompanyNumbers,
            cancellationToken);

        return ApiResponse<object>.Ok(new object(), ApiStatusCodes.NoContent);
    }
}
