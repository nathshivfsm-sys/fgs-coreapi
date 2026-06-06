using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.File.Application.Abstractions.Provisioning;
using MediatR;

namespace Fgs.File.Application.Features.TenantStorage.Commands.ProvisionTenantBucket;

public sealed class ProvisionTenantBucketCommandHandler(ITenantS3BucketProvisioner provisioner)
    : IRequestHandler<ProvisionTenantBucketCommand, ApiResponse<ProvisionTenantBucketResponse>>
{
    public async Task<ApiResponse<ProvisionTenantBucketResponse>> Handle(
        ProvisionTenantBucketCommand request,
        CancellationToken cancellationToken)
    {
        var bucket = await provisioner.EnsureTenantBucketAsync(
            request.TenantId,
            request.Request.ExistingBucketName,
            cancellationToken);

        await provisioner.InitializeFolderStructureAsync(
            bucket,
            request.TenantId,
            request.Request.CompanyNumbers,
            cancellationToken);

        return ApiResponse<ProvisionTenantBucketResponse>.Ok(new ProvisionTenantBucketResponse(bucket));
    }
}
