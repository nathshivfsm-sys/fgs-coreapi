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

/// <summary>Manage FgsSetupPricingMatrix catalog records.</summary>
[Authorize]
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("pricingmatrixs")]
[Tags("Setup - Pricing")]
public sealed class SetupPricingMatrixController : CatalogCrudControllerBase
{
    public SetupPricingMatrixController(MediatR.IMediator mediator) : base(mediator) { }

    /// <summary>Gets a record by identifier.</summary>
    /// <param name="id">The FgsSetupPricingMatrix identifier.</param>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<FgsSetupPricingMatrixDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new GetCatalogEntityQuery<FgsSetupPricingMatrixDetailDto>(EntityKeys.SetupPricingMatrix, id.ToString()), cancellationToken));

    /// <summary>Lists records with pagination, sorting, and search.</summary>
    /// <param name="page">Page number (1-based).</param>
    /// <param name="pageSize">Number of records per page.</param>
    /// <param name="sortBy">Property name to sort by.</param>
    /// <param name="sortDirection">Sort direction.</param>
    /// <param name="search">Free-text search across searchable fields.</param>
    /// <param name="isActive">Filter by active status when supported.</param>
    /// <param name="code">Filter by Code.</param>
    /// <param name="name">Filter by Name.</param>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsSetupPricingMatrixSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = true,
        [FromQuery] string? code = null,
        [FromQuery] string? name = null,
        CancellationToken cancellationToken = default)
    {
        var filters = new FgsSetupPricingMatrixListFilters(code, name);
        var response = await Mediator.Send(new ListCatalogEntitiesQuery<FgsSetupPricingMatrixSummaryDto>(EntityKeys.SetupPricingMatrix, new PagedQuery(page, pageSize, sortBy, sortDirection, search, isActive), filters), cancellationToken);
        return FromApiResponse(response);
    }

    /// <summary>Creates a new record.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsSetupPricingMatrixDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] FgsSetupPricingMatrixCreateDto request, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new CreateCatalogEntityCommand<FgsSetupPricingMatrixCreateDto, FgsSetupPricingMatrixDetailDto>(EntityKeys.SetupPricingMatrix, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>Replaces an existing record.</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<FgsSetupPricingMatrixDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] FgsSetupPricingMatrixUpdateDto request, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new UpdateCatalogEntityCommand<FgsSetupPricingMatrixUpdateDto, FgsSetupPricingMatrixDetailDto>(EntityKeys.SetupPricingMatrix, id.ToString(), request), cancellationToken));

    /// <summary>Partially updates an existing record.</summary>
    [HttpPatch("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<FgsSetupPricingMatrixDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Patch(Guid id, [FromBody] FgsSetupPricingMatrixPatchDto request, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new PatchCatalogEntityCommand<FgsSetupPricingMatrixPatchDto, FgsSetupPricingMatrixDetailDto>(EntityKeys.SetupPricingMatrix, id.ToString(), request), cancellationToken));

    /// <summary>Deletes a record (soft delete when supported).</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new DeleteCatalogEntityCommand(EntityKeys.SetupPricingMatrix, id.ToString()), cancellationToken));
}

internal sealed record FgsSetupPricingMatrixListFilters(string? Code, string? Name);
