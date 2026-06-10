using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Foundation.CatalogCrud.Commands;
using Fgs.Foundation.CatalogCrud.Queries;
using Fgs.Setup.Application.Common.Catalog;
using Fgs.Setup.Application.Features.Generated.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Setup.API.Controllers.Generated;

/// <summary>Manage FgsInventoryItemDependency catalog records.</summary>
[Authorize]
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("inventoryitemdependencies")]
[Tags("Setup - Inventory")]
public sealed class InventoryItemDependencyController : CatalogCrudControllerBase
{
    public InventoryItemDependencyController(MediatR.IMediator mediator) : base(mediator) { }

    /// <summary>Gets a record by identifier.</summary>
    /// <param name="id">The FgsInventoryItemDependency identifier.</param>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsInventoryItemDependencyDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(long id, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new GetCatalogEntityQuery<FgsInventoryItemDependencyDetailDto>(EntityKeys.InventoryItemDependency, id.ToString()), cancellationToken));

    /// <summary>Lists records with pagination, sorting, and search.</summary>
    /// <param name="page">Page number (1-based).</param>
    /// <param name="pageSize">Number of records per page.</param>
    /// <param name="sortBy">Property name to sort by.</param>
    /// <param name="sortDirection">Sort direction.</param>
    /// <param name="search">Free-text search across searchable fields.</param>
    /// <param name="isActive">Filter by active status when supported.</param>
    /// <param name="dependencyType">Filter by DependencyType.</param>
    /// <param name="notes">Filter by Notes.</param>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsInventoryItemDependencySummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = true,
        [FromQuery] string? dependencyType = null,
        [FromQuery] string? notes = null,
        CancellationToken cancellationToken = default)
    {
        var filters = new FgsInventoryItemDependencyListFilters(dependencyType, notes);
        var response = await Mediator.Send(new ListCatalogEntitiesQuery<FgsInventoryItemDependencySummaryDto>(EntityKeys.InventoryItemDependency, new PagedQuery(page, pageSize, sortBy, sortDirection, search, isActive), filters), cancellationToken);
        return FromApiResponse(response);
    }

    /// <summary>Creates a new record.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsInventoryItemDependencyDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] FgsInventoryItemDependencyCreateDto request, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new CreateCatalogEntityCommand<FgsInventoryItemDependencyCreateDto, FgsInventoryItemDependencyDetailDto>(EntityKeys.InventoryItemDependency, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>Replaces an existing record.</summary>
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsInventoryItemDependencyDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(long id, [FromBody] FgsInventoryItemDependencyUpdateDto request, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new UpdateCatalogEntityCommand<FgsInventoryItemDependencyUpdateDto, FgsInventoryItemDependencyDetailDto>(EntityKeys.InventoryItemDependency, id.ToString(), request), cancellationToken));

    /// <summary>Partially updates an existing record.</summary>
    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsInventoryItemDependencyDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Patch(long id, [FromBody] FgsInventoryItemDependencyPatchDto request, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new PatchCatalogEntityCommand<FgsInventoryItemDependencyPatchDto, FgsInventoryItemDependencyDetailDto>(EntityKeys.InventoryItemDependency, id.ToString(), request), cancellationToken));

    /// <summary>Deletes a record (soft delete when supported).</summary>
    [HttpDelete("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new DeleteCatalogEntityCommand(EntityKeys.InventoryItemDependency, id.ToString()), cancellationToken));
}

internal sealed record FgsInventoryItemDependencyListFilters(string? DependencyType, string? Notes);
