using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.InventoryItemTypes.Commands.CreateFgsInventoryItemType;
using Fgs.Inventory.Application.Features.InventoryItemTypes.Commands.PatchFgsInventoryItemType;
using Fgs.Inventory.Application.Features.InventoryItemTypes.Commands.UpdateFgsInventoryItemType;
using Fgs.Inventory.Application.Features.InventoryItemTypes.Dtos;
using Fgs.Inventory.Application.Features.InventoryItemTypes.Queries.GetFgsInventoryItemTypeById;
using Fgs.Inventory.Application.Features.InventoryItemTypes.Queries.ListInventoryItemTypes;
using Fgs.Inventory.Application.Features.InventoryItemTypes.Queries.LookupInventoryItemTypes;
using MediatR;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Inventory.API.Controllers;

[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("inventoryitemtype")]
[ApiController]
[Produces("application/json")]
public sealed class InventoryItemTypeController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsInventoryItemTypeDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetFgsInventoryItemTypeByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsInventoryItemTypeSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = true,
        [FromQuery] string? itemTypeCode = null,
        [FromQuery] string? name = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListInventoryItemTypesQuery(
                new InventoryListQuery(page, pageSize, sortBy, sortDirection, search, isActive),
                new FgsInventoryItemTypeListFilters(itemTypeCode, name)),
            cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsInventoryItemTypeLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup([FromQuery] bool activeOnly = true, CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new LookupInventoryItemTypesQuery(activeOnly), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.InventoryItemCreate)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsInventoryItemTypeDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] FgsInventoryItemTypeCreateDto request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateFgsInventoryItemTypeCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.InventoryItemEdit)]
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsInventoryItemTypeDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(long id, [FromBody] FgsInventoryItemTypeUpdateDto request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateFgsInventoryItemTypeCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.InventoryItemEdit)]
    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsInventoryItemTypeDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(long id, [FromBody] FgsInventoryItemTypePatchDto request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new PatchFgsInventoryItemTypeCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
