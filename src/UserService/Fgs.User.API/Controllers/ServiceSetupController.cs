using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.User.Application.Features.ServiceSetups.Commands.PatchFgsTenantServiceSetup;
using Fgs.User.Application.Features.ServiceSetups.Commands.UpdateFgsTenantServiceSetup;
using Fgs.User.Application.Features.ServiceSetups.Dtos;
using Fgs.User.Application.Features.ServiceSetups.Queries.GetFgsTenantServiceSetup;
using MediatR;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.User.API.Controllers;

/// <summary>
/// Per-company service / operations configuration.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("servicesetup")]
[Produces("application/json")]
public sealed class ServiceSetupController(IMediator mediator) : FgsApiControllerBase(mediator)
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<FgsTenantServiceSetupDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new GetFgsTenantServiceSetupQuery(), cancellationToken));

    [RequirePermission(FgsPermissionCodes.UserEdit)]
    [HttpPut]
    [ProducesResponseType(typeof(ApiResponse<FgsTenantServiceSetupDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromBody] FgsTenantServiceSetupUpdateDto request,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new UpdateFgsTenantServiceSetupCommand(request), cancellationToken));

    [RequirePermission(FgsPermissionCodes.UserEdit)]
    [HttpPatch]
    [ProducesResponseType(typeof(ApiResponse<FgsTenantServiceSetupDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Patch(
        [FromBody] FgsTenantServiceSetupPatchDto request,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new PatchFgsTenantServiceSetupCommand(request), cancellationToken));
}
