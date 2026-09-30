using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.ServiceSetups.Commands.PatchFgsTenantServiceSetup;
using Fgs.User.Application.Features.ServiceSetups.Commands.UpdateFgsTenantServiceSetup;
using Fgs.User.Application.Features.ServiceSetups.Dtos;
using Fgs.User.Application.Features.ServiceSetups.Queries.GetFgsTenantServiceSetupById;
using Fgs.User.Application.Features.ServiceSetups.Queries.ListFgsTenantServiceSetups;
using Fgs.User.Application.Features.ServiceSetups.Queries.LookupFgsTenantServiceSetups;
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
    [HttpGet("{companyId:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsTenantServiceSetupDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long companyId, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new GetFgsTenantServiceSetupByIdQuery(companyId), cancellationToken));

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsTenantServiceSetupSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = null,
        CancellationToken cancellationToken = default) =>
        FromApiResponse(await Mediator.Send(
            new ListFgsTenantServiceSetupsQuery(
                new IdentityListQuery(page, pageSize, sortBy, sortDirection, search, isActive)),
            cancellationToken));

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsTenantServiceSetupLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default) =>
        FromApiResponse(await Mediator.Send(new LookupFgsTenantServiceSetupsQuery(activeOnly), cancellationToken));

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
