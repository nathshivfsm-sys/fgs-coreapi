using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.User.Application.Features.ServiceAccountsSetups.Commands.PatchFgsTenantServiceAccountsSetup;
using Fgs.User.Application.Features.ServiceAccountsSetups.Commands.UpdateFgsTenantServiceAccountsSetup;
using Fgs.User.Application.Features.ServiceAccountsSetups.Dtos;
using Fgs.User.Application.Features.ServiceAccountsSetups.Queries.GetFgsTenantServiceAccountsSetup;
using MediatR;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.User.API.Controllers;

/// <summary>
/// Per-company default general ledger account mappings.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("serviceaccountssetup")]
[Produces("application/json")]
public sealed class ServiceAccountsSetupController(IMediator mediator) : FgsApiControllerBase(mediator)
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<FgsTenantServiceAccountsSetupDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new GetFgsTenantServiceAccountsSetupQuery(), cancellationToken));

    [RequirePermission(FgsPermissionCodes.UserEdit)]
    [HttpPut]
    [ProducesResponseType(typeof(ApiResponse<FgsTenantServiceAccountsSetupDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromBody] FgsTenantServiceAccountsSetupUpdateDto request,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new UpdateFgsTenantServiceAccountsSetupCommand(request), cancellationToken));

    [RequirePermission(FgsPermissionCodes.UserEdit)]
    [HttpPatch]
    [ProducesResponseType(typeof(ApiResponse<FgsTenantServiceAccountsSetupDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Patch(
        [FromBody] FgsTenantServiceAccountsSetupPatchDto request,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new PatchFgsTenantServiceAccountsSetupCommand(request), cancellationToken));
}
