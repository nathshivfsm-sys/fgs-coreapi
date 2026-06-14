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

/// <summary>Manage FgsSetupServiceAgreementTemplatePricingComponent catalog records.</summary>
[Authorize]
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("serviceagreementtemplatepricingcomponents")]
[Tags("Setup - ServiceAgreements")]
public sealed class SetupServiceAgreementTemplatePricingComponentController : CatalogCrudControllerBase
{
    public SetupServiceAgreementTemplatePricingComponentController(MediatR.IMediator mediator) : base(mediator) { }

    /// <summary>Gets a record by identifier.</summary>
    /// <param name="id">The FgsSetupServiceAgreementTemplatePricingComponent identifier.</param>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsSetupServiceAgreementTemplatePricingComponentDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(long id, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new GetCatalogEntityQuery<FgsSetupServiceAgreementTemplatePricingComponentDetailDto>(EntityKeys.SetupServiceAgreementTemplatePricingComponent, id.ToString()), cancellationToken));

    /// <summary>Lists records with pagination, sorting, and search.</summary>
    /// <param name="page">Page number (1-based).</param>
    /// <param name="pageSize">Number of records per page.</param>
    /// <param name="sortBy">Property name to sort by.</param>
    /// <param name="sortDirection">Sort direction.</param>
    /// <param name="search">Free-text search across searchable fields.</param>
    /// <param name="isActive">Filter by active status when supported.</param>
    /// <param name="pricingComponentCode">Filter by PricingComponentCode.</param>
    /// <param name="name">Filter by Name.</param>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsSetupServiceAgreementTemplatePricingComponentSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = true,
        [FromQuery] string? pricingComponentCode = null,
        [FromQuery] string? name = null,
        CancellationToken cancellationToken = default)
    {
        var filters = new FgsSetupServiceAgreementTemplatePricingComponentListFilters(pricingComponentCode, name);
        var response = await Mediator.Send(new ListCatalogEntitiesQuery<FgsSetupServiceAgreementTemplatePricingComponentSummaryDto>(EntityKeys.SetupServiceAgreementTemplatePricingComponent, new PagedQuery(page, pageSize, sortBy, sortDirection, search, isActive), filters), cancellationToken);
        return FromApiResponse(response);
    }

    /// <summary>Creates a new record.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsSetupServiceAgreementTemplatePricingComponentDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] FgsSetupServiceAgreementTemplatePricingComponentCreateDto request, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new CreateCatalogEntityCommand<FgsSetupServiceAgreementTemplatePricingComponentCreateDto, FgsSetupServiceAgreementTemplatePricingComponentDetailDto>(EntityKeys.SetupServiceAgreementTemplatePricingComponent, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>Replaces an existing record.</summary>
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsSetupServiceAgreementTemplatePricingComponentDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(long id, [FromBody] FgsSetupServiceAgreementTemplatePricingComponentUpdateDto request, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new UpdateCatalogEntityCommand<FgsSetupServiceAgreementTemplatePricingComponentUpdateDto, FgsSetupServiceAgreementTemplatePricingComponentDetailDto>(EntityKeys.SetupServiceAgreementTemplatePricingComponent, id.ToString(), request), cancellationToken));

    /// <summary>Partially updates an existing record.</summary>
    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsSetupServiceAgreementTemplatePricingComponentDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Patch(long id, [FromBody] FgsSetupServiceAgreementTemplatePricingComponentPatchDto request, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new PatchCatalogEntityCommand<FgsSetupServiceAgreementTemplatePricingComponentPatchDto, FgsSetupServiceAgreementTemplatePricingComponentDetailDto>(EntityKeys.SetupServiceAgreementTemplatePricingComponent, id.ToString(), request), cancellationToken));

    /// <summary>Deletes a record (soft delete when supported).</summary>
    [HttpDelete("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new DeleteCatalogEntityCommand(EntityKeys.SetupServiceAgreementTemplatePricingComponent, id.ToString()), cancellationToken));
}

internal sealed record FgsSetupServiceAgreementTemplatePricingComponentListFilters(string? PricingComponentCode, string? Name);
