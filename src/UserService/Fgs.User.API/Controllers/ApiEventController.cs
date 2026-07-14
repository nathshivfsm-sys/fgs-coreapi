using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.ApiEvents.Commands.CreateFgsApiEvent;
using Fgs.User.Application.Features.ApiEvents.Commands.PatchFgsApiEvent;
using Fgs.User.Application.Features.ApiEvents.Commands.UpdateFgsApiEvent;
using Fgs.User.Application.Features.ApiEvents.Dtos;
using Fgs.User.Application.Features.ApiEvents.Queries.GetFgsApiEventById;
using Fgs.User.Application.Features.ApiEvents.Queries.ListFgsApiEvents;
using Fgs.User.Application.Features.ApiEvents.Queries.LookupFgsApiEvents;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.User.API.Controllers;

/// <summary>
/// Global API event catalog management.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("apievent")]
[Produces("application/json")]
public sealed class ApiEventController(IMediator mediator) : FgsApiControllerBase(mediator)
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsApiEventDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new GetFgsApiEventByIdQuery(id), cancellationToken));

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsApiEventSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] string? eventCode = null,
        [FromQuery] string? eventCategory = null,
        [FromQuery] string? name = null,
        CancellationToken cancellationToken = default) =>
        FromApiResponse(await Mediator.Send(
            new ListFgsApiEventsQuery(
                new IdentityListQuery(page, pageSize, sortBy, sortDirection, search, isActive),
                new FgsApiEventListFilters(eventCode, eventCategory, name)),
            cancellationToken));

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsApiEventLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default) =>
        FromApiResponse(await Mediator.Send(new LookupFgsApiEventsQuery(activeOnly), cancellationToken));

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsApiEventDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsApiEventCreateDto request,
        CancellationToken cancellationToken) =>
        CreatedFromApiResponse(await Mediator.Send(new CreateFgsApiEventCommand(request), cancellationToken));

    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsApiEventDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] FgsApiEventUpdateDto request,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new UpdateFgsApiEventCommand(id, request), cancellationToken));

    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsApiEventDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] FgsApiEventPatchDto request,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new PatchFgsApiEventCommand(id, request), cancellationToken));
}
