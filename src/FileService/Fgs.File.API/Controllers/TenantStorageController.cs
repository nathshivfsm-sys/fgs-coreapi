using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.File.Application.Features.TenantStorage.Commands.ProvisionTenantBucket;
using Fgs.Foundation.Api;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.File.API.Controllers;

[AllowAnonymous]
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("tenants")]
public sealed class TenantStorageController(IMediator mediator) : FgsApiControllerBase(mediator)
{
    [HttpPost("{tenantId:long}/bucket")]
    [ProducesResponseType(typeof(ApiResponse<ProvisionTenantBucketResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ProvisionBucket(
        long tenantId,
        [FromBody] ProvisionTenantBucketRequest request,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(
            new ProvisionTenantBucketCommand(tenantId, request),
            cancellationToken));
}
