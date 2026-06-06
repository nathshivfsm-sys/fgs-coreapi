using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Contracts.Requests;
using Fgs.Foundation.Api;
using Fgs.Setup.Application.Features.TenantProvisioning.Commands.ProvisionTenant;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Setup.API.Controllers;

[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("tenant-provisioning")]
[Authorize]
public sealed class TenantProvisioningController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ProvisionTenant(
        [FromBody] ProvisionTenantRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new ProvisionTenantCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
