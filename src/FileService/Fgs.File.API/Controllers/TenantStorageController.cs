using Asp.Versioning;
using Fgs.Contracts.Clients;
using Fgs.File.Application.Abstractions.Provisioning;
using Fgs.Foundation.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.File.API.Controllers;

[AllowAnonymous]
[ApiController]
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("tenants")]
public sealed class TenantStorageController(ITenantS3BucketProvisioner bucketProvisioner) : ControllerBase
{
    [HttpPost("{tenantId:long}/bucket")]
    public async Task<ActionResult<ProvisionTenantBucketResponse>> ProvisionBucket(
        long tenantId,
        [FromBody] ProvisionTenantBucketRequest request,
        CancellationToken cancellationToken)
    {
        var bucket = await bucketProvisioner.EnsureTenantBucketAsync(
            tenantId,
            request.ExistingBucketName,
            cancellationToken);

        return new ProvisionTenantBucketResponse(bucket);
    }

    [HttpPost("{tenantId:long}/folders")]
    public async Task<IActionResult> InitializeFolders(
        long tenantId,
        [FromBody] InitializeTenantFoldersRequest request,
        CancellationToken cancellationToken)
    {
        await bucketProvisioner.InitializeFolderStructureAsync(
            request.BucketName,
            request.TenantId,
            request.CompanyNumbers,
            cancellationToken);

        return NoContent();
    }
}
