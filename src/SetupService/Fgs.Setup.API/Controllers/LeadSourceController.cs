using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.LeadSources.Commands.CreateLeadSource;
using Fgs.Setup.Application.Features.LeadSources.Commands.PatchLeadSource;
using Fgs.Setup.Application.Features.LeadSources.Commands.UpdateLeadSource;
using Fgs.Setup.Application.Features.LeadSources.Queries.GetLeadSourceById;
using Fgs.Setup.Application.Features.LeadSources.Queries.ListLeadSources;
using Fgs.Setup.Application.Features.LeadSources.Queries.LookupLeadSources;
using Fgs.Setup.Application.Features.LeadSources.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Setup.API.Controllers;

/// <summary>
/// Tenant-scoped lead source catalog management.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("leadsource")]
[Produces("application/json")]
public sealed class LeadSourceController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<LeadSourceDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetLeadSourceByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<LeadSourceSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] string? sourceCode = null,
        [FromQuery] string? sourceName = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListLeadSourcesQuery(
                new SetupListQuery(page, pageSize, sortBy, sortDirection, search, isActive),
                new LeadSourceListFilters(sourceCode, sourceName)),
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<LeadSourceLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new LookupLeadSourcesQuery(activeOnly), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<LeadSourceDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] LeadSourceCreateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateLeadSourceCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<LeadSourceDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] LeadSourceUpdateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateLeadSourceCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<LeadSourceDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] LeadSourcePatchDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new PatchLeadSourceCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
