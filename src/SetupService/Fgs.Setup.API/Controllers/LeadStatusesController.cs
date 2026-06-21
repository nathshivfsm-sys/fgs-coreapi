using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.LeadStatuses.Commands.CreateLeadStatus;
using Fgs.Setup.Application.Features.LeadStatuses.Commands.DeleteLeadStatus;
using Fgs.Setup.Application.Features.LeadStatuses.Commands.PatchLeadStatus;
using Fgs.Setup.Application.Features.LeadStatuses.Commands.UpdateLeadStatus;
using Fgs.Setup.Application.Features.LeadStatuses.Queries.GetLeadStatusById;
using Fgs.Setup.Application.Features.LeadStatuses.Queries.ListLeadStatuses;
using Fgs.Setup.Application.Features.LeadStatuses.Queries.ListActiveLeadStatuses;
using Fgs.Setup.Application.Features.LeadStatuses.Queries.LookupLeadStatuses;
using Fgs.Setup.Application.Features.LeadStatuses.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Setup.API.Controllers;

/// <summary>
/// Tenant-scoped lead status catalog management.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("leadstatuses")]
[Produces("application/json")]
public sealed class LeadStatusesController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<LeadStatusDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetLeadStatusByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<LeadStatusSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = true,
        [FromQuery] string? statusCode = null,
        [FromQuery] string? statusName = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListLeadStatusesQuery(
                new SetupListQuery(page, pageSize, sortBy, sortDirection, search, isActive),
                new LeadStatusListFilters(statusCode, statusName)),
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<LeadStatusLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new LookupLeadStatusesQuery(activeOnly), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("active")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<LeadStatusSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListActive(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] string? statusCode = null,
        [FromQuery] string? statusName = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListActiveLeadStatusesQuery(
                page,
                pageSize,
                sortBy,
                sortDirection,
                search,
                new LeadStatusListFilters(statusCode, statusName)),
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<LeadStatusDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] LeadStatusCreateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateLeadStatusCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<LeadStatusDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] LeadStatusUpdateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateLeadStatusCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<LeadStatusDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] LeadStatusPatchDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new PatchLeadStatusCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<LeadStatusDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new DeleteLeadStatusCommand(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
