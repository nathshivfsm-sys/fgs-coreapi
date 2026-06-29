using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Commands.CreateLeadDisqualificationReason;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Commands.DeleteLeadDisqualificationReason;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Commands.PatchLeadDisqualificationReason;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Commands.UpdateLeadDisqualificationReason;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Queries.GetLeadDisqualificationReasonById;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Queries.ListLeadDisqualificationReasons;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Queries.LookupLeadDisqualificationReasons;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Setup.API.Controllers;

/// <summary>
/// Tenant-scoped lead disqualification reason catalog management.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("leaddisqualificationreasons")]
[Produces("application/json")]
public sealed class LeadDisqualificationReasonsController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<LeadDisqualificationReasonDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetLeadDisqualificationReasonByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<LeadDisqualificationReasonSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] string? reasonCode = null,
        [FromQuery] string? reasonName = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListLeadDisqualificationReasonsQuery(
                new SetupListQuery(page, pageSize, sortBy, sortDirection, search, isActive),
                new LeadDisqualificationReasonListFilters(reasonCode, reasonName)),
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<LeadDisqualificationReasonLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new LookupLeadDisqualificationReasonsQuery(activeOnly), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<LeadDisqualificationReasonDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] LeadDisqualificationReasonCreateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateLeadDisqualificationReasonCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<LeadDisqualificationReasonDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] LeadDisqualificationReasonUpdateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateLeadDisqualificationReasonCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<LeadDisqualificationReasonDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] LeadDisqualificationReasonPatchDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new PatchLeadDisqualificationReasonCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<LeadDisqualificationReasonDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new DeleteLeadDisqualificationReasonCommand(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
