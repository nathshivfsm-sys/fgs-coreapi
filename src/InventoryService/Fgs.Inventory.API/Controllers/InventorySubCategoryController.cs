using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.InventorySubCategories.Commands.CreateFgsInventorySubCategory;
using Fgs.Inventory.Application.Features.InventorySubCategories.Commands.PatchFgsInventorySubCategory;
using Fgs.Inventory.Application.Features.InventorySubCategories.Commands.UpdateFgsInventorySubCategory;
using Fgs.Inventory.Application.Features.InventorySubCategories.Dtos;
using Fgs.Inventory.Application.Features.InventorySubCategories.Queries.GetFgsInventorySubCategoryById;
using Fgs.Inventory.Application.Features.InventorySubCategories.Queries.ListInventorySubCategories;
using Fgs.Inventory.Application.Features.InventorySubCategories.Queries.LookupInventorySubCategories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Inventory.API.Controllers;

[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("inventorysubcategory")]
[ApiController]
[Produces("application/json")]
public sealed class InventorySubCategoryController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsInventorySubCategoryDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetFgsInventorySubCategoryByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsInventorySubCategorySummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = true,
        [FromQuery] string? subCategoryCode = null,
        [FromQuery] string? name = null,
        [FromQuery] long? inventoryCategoryId = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListInventorySubCategoriesQuery(
                new InventoryListQuery(page, pageSize, sortBy, sortDirection, search, isActive),
                new FgsInventorySubCategoryListFilters(subCategoryCode, name, inventoryCategoryId)),
            cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsInventorySubCategoryLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup([FromQuery] bool activeOnly = true, CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new LookupInventorySubCategoriesQuery(activeOnly), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsInventorySubCategoryDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] FgsInventorySubCategoryCreateDto request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateFgsInventorySubCategoryCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsInventorySubCategoryDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(long id, [FromBody] FgsInventorySubCategoryUpdateDto request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateFgsInventorySubCategoryCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsInventorySubCategoryDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(long id, [FromBody] FgsInventorySubCategoryPatchDto request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new PatchFgsInventorySubCategoryCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
