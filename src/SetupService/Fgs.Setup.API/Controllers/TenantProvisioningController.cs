using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Contracts.Requests;
using Fgs.Foundation.Api;
using Fgs.Setup.Application.Common;
using Fgs.Setup.Application.Features.TenantProvisioning.Commands.ProvisionTenant;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Setup.API.Controllers;

/// <summary>
/// Internal tenant provisioning orchestration invoked by the consumer service.
/// </summary>
[AllowAnonymous]
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("tenantprovisioning")]
public sealed class TenantProvisioningController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ProvisionTenant(
        [FromBody] ProvisionTenantRequest request,
        [FromHeader(Name = CredentialDistributionHeaders.InternalServiceKey)] string? serviceKey,
        [FromHeader(Name = CredentialDistributionHeaders.ServiceName)] string? serviceName,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(
            new ProvisionTenantCommand(request, serviceKey, serviceName),
            cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
