using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Credentials;
using Fgs.Credentials.Options;
using Fgs.File.Application.Features.TenantStorage.Commands.ProvisionTenantBucket;
using Fgs.Foundation.Api;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Fgs.File.API.Controllers;

[AllowAnonymous]
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("tenants")]
public sealed class TenantStorageController(
    IMediator mediator,
    IOptions<CredentialDistributionOptions> distributionOptions) : FgsApiControllerBase(mediator)
{
    [HttpPost("{tenantId:long}/bucket")]
    [ProducesResponseType(typeof(ApiResponse<ProvisionTenantBucketResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ProvisionBucket(
        long tenantId,
        [FromBody] ProvisionTenantBucketRequest request,
        [FromHeader(Name = InternalServiceHeaders.ServiceKey)] string? serviceKey,
        CancellationToken cancellationToken)
    {
        if (!InternalServiceAuthorization.IsAuthorized(serviceKey, distributionOptions.Value))
        {
            return StatusCode(
                StatusCodes.Status401Unauthorized,
                ApiResponse<object>.Fail(["Unauthorized."], ApiStatusCodes.Unauthorized));
        }

        return FromApiResponse(await Mediator.Send(
            new ProvisionTenantBucketCommand(tenantId, request),
            cancellationToken));
    }
}
