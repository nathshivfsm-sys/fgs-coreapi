using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.InventoryCategories.Commands.CreateFgsInventoryCategory;
using Fgs.Inventory.Application.Features.InventoryCategories.Commands.PatchFgsInventoryCategory;
using Fgs.Inventory.Application.Features.InventoryCategories.Commands.UpdateFgsInventoryCategory;
using Fgs.Inventory.Application.Features.InventoryCategories.Dtos;
using Fgs.Inventory.Application.Features.InventoryCategories.Queries.GetFgsInventoryCategoryById;
using Fgs.Inventory.Application.Features.InventoryCategories.Queries.ListInventoryCategories;
using Fgs.Inventory.Application.Features.InventoryCategories.Queries.LookupInventoryCategories;
using MediatR;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Inventory.API.Controllers;

[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("inventorycategory")]
[ApiController]
[Produces("application/json")]
public sealed class InventoryCategoryController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsInventoryCategoryDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetFgsInventoryCategoryByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsInventoryCategorySummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = true,
        [FromQuery] string? categoryCode = null,
        [FromQuery] string? name = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListInventoryCategoriesQuery(
                new InventoryListQuery(page, pageSize, sortBy, sortDirection, search, isActive),
                new FgsInventoryCategoryListFilters(categoryCode, name)),
            cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsInventoryCategoryLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup([FromQuery] bool activeOnly = true, CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new LookupInventoryCategoriesQuery(activeOnly), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.InventoryItemCreate)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsInventoryCategoryDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] FgsInventoryCategoryCreateDto request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateFgsInventoryCategoryCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.InventoryItemEdit)]
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsInventoryCategoryDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(long id, [FromBody] FgsInventoryCategoryUpdateDto request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateFgsInventoryCategoryCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.InventoryItemEdit)]
    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsInventoryCategoryDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(long id, [FromBody] FgsInventoryCategoryPatchDto request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new PatchFgsInventoryCategoryCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
