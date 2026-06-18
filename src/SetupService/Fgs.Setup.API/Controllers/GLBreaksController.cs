using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.GLBreaks.Commands.CreateGLBreak;
using Fgs.Setup.Application.Features.GLBreaks.Commands.DeleteGLBreak;
using Fgs.Setup.Application.Features.GLBreaks.Commands.PatchGLBreak;
using Fgs.Setup.Application.Features.GLBreaks.Commands.UpdateGLBreak;
using Fgs.Setup.Application.Features.GLBreaks.Dtos;
using Fgs.Setup.Application.Features.GLBreaks.Queries.GetGLBreakById;
using Fgs.Setup.Application.Features.GLBreaks.Queries.ListActiveGLBreaks;
using Fgs.Setup.Application.Features.GLBreaks.Queries.ListGLBreaks;
using Fgs.Setup.Application.Features.GLBreaks.Queries.LookupGLBreaks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Setup.API.Controllers;

/// <summary>
/// Tenant-scoped GL break catalog management.
/// </summary>
[AllowAnonymous]
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("glbreaks")]
[Produces("application/json")]
public sealed class GLBreaksController(IMediator mediator) : ControllerBase
{
    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<GLBreakLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new LookupGLBreaksQuery(activeOnly), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("active")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<GLBreakSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListActive(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] string? code = null,
        [FromQuery] string? name = null,
        [FromQuery] short? breakLevel = null,
        [FromQuery] string? tradeCode = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListActiveGLBreaksQuery(
                page,
                pageSize,
                sortBy,
                sortDirection,
                search,
                new GLBreakListFilters(code, name, breakLevel, tradeCode)),
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<GLBreakDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetGLBreakByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<GLBreakSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = true,
        [FromQuery] string? code = null,
        [FromQuery] string? name = null,
        [FromQuery] short? breakLevel = null,
        [FromQuery] string? tradeCode = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListGLBreaksQuery(
                new SetupListQuery(page, pageSize, sortBy, sortDirection, search, isActive),
                new GLBreakListFilters(code, name, breakLevel, tradeCode)),
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<GLBreakDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] GLBreakCreateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateGLBreakCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<GLBreakDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] GLBreakUpdateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateGLBreakCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<GLBreakDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] GLBreakPatchDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new PatchGLBreakCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<GLBreakDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new DeleteGLBreakCommand(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
