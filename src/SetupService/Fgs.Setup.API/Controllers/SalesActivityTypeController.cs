using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SalesActivityTypes.Commands.CreateFgsSalesActivityType;
using Fgs.Setup.Application.Features.SalesActivityTypes.Commands.PatchFgsSalesActivityType;
using Fgs.Setup.Application.Features.SalesActivityTypes.Commands.UpdateFgsSalesActivityType;
using Fgs.Setup.Application.Features.SalesActivityTypes.Queries.GetFgsSalesActivityTypeById;
using Fgs.Setup.Application.Features.SalesActivityTypes.Queries.ListSalesActivityTypes;
using Fgs.Setup.Application.Features.SalesActivityTypes.Queries.LookupSalesActivityTypes;
using Fgs.Setup.Application.Features.SalesActivityTypes.Dtos;
using MediatR;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Setup.API.Controllers;

/// <summary>
/// Tenant-scoped sales activity type catalog management.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("salesactivitytype")]
[Produces("application/json")]
public sealed class SalesActivityTypeController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsSalesActivityTypeDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetFgsSalesActivityTypeByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsSalesActivityTypeSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] string? activityTypeCode = null,
        [FromQuery] string? activityTypeName = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListSalesActivityTypesQuery(
                new SetupListQuery(page, pageSize, sortBy, sortDirection, search, isActive),
                new FgsSalesActivityTypeListFilters(activityTypeCode, activityTypeName)),
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsSalesActivityTypeLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new LookupSalesActivityTypesQuery(activeOnly), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.SetupCreate)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsSalesActivityTypeDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsSalesActivityTypeCreateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateFgsSalesActivityTypeCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.SetupEdit)]
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsSalesActivityTypeDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] FgsSalesActivityTypeUpdateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateFgsSalesActivityTypeCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.SetupEdit)]
    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsSalesActivityTypeDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] FgsSalesActivityTypePatchDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new PatchFgsSalesActivityTypeCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
