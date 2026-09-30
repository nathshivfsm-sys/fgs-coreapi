using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.ServiceAccountsSetups.Commands.PatchFgsTenantServiceAccountsSetup;
using Fgs.User.Application.Features.ServiceAccountsSetups.Commands.UpdateFgsTenantServiceAccountsSetup;
using Fgs.User.Application.Features.ServiceAccountsSetups.Dtos;
using Fgs.User.Application.Features.ServiceAccountsSetups.Queries.GetFgsTenantServiceAccountsSetupById;
using Fgs.User.Application.Features.ServiceAccountsSetups.Queries.ListFgsTenantServiceAccountsSetups;
using Fgs.User.Application.Features.ServiceAccountsSetups.Queries.LookupFgsTenantServiceAccountsSetups;
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
    [HttpGet("{companyId:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsTenantServiceAccountsSetupDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long companyId, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new GetFgsTenantServiceAccountsSetupByIdQuery(companyId), cancellationToken));

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsTenantServiceAccountsSetupSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = null,
        CancellationToken cancellationToken = default) =>
        FromApiResponse(await Mediator.Send(
            new ListFgsTenantServiceAccountsSetupsQuery(
                new IdentityListQuery(page, pageSize, sortBy, sortDirection, search, isActive)),
            cancellationToken));

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsTenantServiceAccountsSetupLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default) =>
        FromApiResponse(await Mediator.Send(new LookupFgsTenantServiceAccountsSetupsQuery(activeOnly), cancellationToken));

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
