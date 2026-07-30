using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.TruckStockTemplateItems.Commands.CreateFgsTruckStockTemplateItem;
using Fgs.Inventory.Application.Features.TruckStockTemplateItems.Commands.DeleteFgsTruckStockTemplateItem;
using Fgs.Inventory.Application.Features.TruckStockTemplateItems.Commands.PatchFgsTruckStockTemplateItem;
using Fgs.Inventory.Application.Features.TruckStockTemplateItems.Commands.UpdateFgsTruckStockTemplateItem;
using Fgs.Inventory.Application.Features.TruckStockTemplateItems.Dtos;
using Fgs.Inventory.Application.Features.TruckStockTemplateItems.Queries.GetFgsTruckStockTemplateItemById;
using Fgs.Inventory.Application.Features.TruckStockTemplateItems.Queries.ListTruckStockTemplateItems;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Commands.CreateFgsTruckStockTemplate;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Commands.PatchFgsTruckStockTemplate;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Commands.UpdateFgsTruckStockTemplate;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Dtos;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Queries.GetFgsTruckStockTemplateById;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Queries.ListTruckStockTemplates;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Queries.LookupTruckStockTemplates;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Inventory.API.Controllers;

/// <summary>
/// Tenant-scoped truck stock template catalog management, including nested template items.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("truck-stock-template")]
[Produces("application/json")]
public sealed class TruckStockTemplateController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsTruckStockTemplateDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetFgsTruckStockTemplateByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsTruckStockTemplateSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = true,
        [FromQuery] string? templateCode = null,
        [FromQuery] string? name = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListTruckStockTemplatesQuery(
                new InventoryListQuery(page, pageSize, sortBy, sortDirection, search, isActive),
                new FgsTruckStockTemplateListFilters(templateCode, name)),
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsTruckStockTemplateLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new LookupTruckStockTemplatesQuery(activeOnly), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsTruckStockTemplateDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsTruckStockTemplateCreateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateFgsTruckStockTemplateCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsTruckStockTemplateDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] FgsTruckStockTemplateUpdateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateFgsTruckStockTemplateCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsTruckStockTemplateDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] FgsTruckStockTemplatePatchDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new PatchFgsTruckStockTemplateCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("{templateId:long}/items")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsTruckStockTemplateItemSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListItems(
        long templateId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] long? inventoryItemId = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListTruckStockTemplateItemsQuery(
                templateId,
                new InventoryListQuery(page, pageSize, sortBy, sortDirection, Search: null, IsActive: null),
                new FgsTruckStockTemplateItemListFilters(inventoryItemId)),
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("{templateId:long}/items/{itemId:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsTruckStockTemplateItemDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetItemById(
        long templateId,
        long itemId,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(
            new GetFgsTruckStockTemplateItemByIdQuery(templateId, itemId),
            cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("{templateId:long}/items")]
    [ProducesResponseType(typeof(ApiResponse<FgsTruckStockTemplateItemDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateItem(
        long templateId,
        [FromBody] FgsTruckStockTemplateItemCreateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(
            new CreateFgsTruckStockTemplateItemCommand(templateId, request),
            cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("{templateId:long}/items/{itemId:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsTruckStockTemplateItemDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateItem(
        long templateId,
        long itemId,
        [FromBody] FgsTruckStockTemplateItemUpdateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(
            new UpdateFgsTruckStockTemplateItemCommand(templateId, itemId, request),
            cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPatch("{templateId:long}/items/{itemId:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsTruckStockTemplateItemDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> PatchItem(
        long templateId,
        long itemId,
        [FromBody] FgsTruckStockTemplateItemPatchDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(
            new PatchFgsTruckStockTemplateItemCommand(templateId, itemId, request),
            cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("{templateId:long}/items/{itemId:long}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteItem(
        long templateId,
        long itemId,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(
            new DeleteFgsTruckStockTemplateItemCommand(templateId, itemId),
            cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
