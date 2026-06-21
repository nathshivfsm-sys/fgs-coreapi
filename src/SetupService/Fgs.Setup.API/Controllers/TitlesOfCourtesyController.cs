using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Commands.CreateTitleOfCourtesy;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Commands.DeleteTitleOfCourtesy;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Commands.PatchTitleOfCourtesy;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Commands.UpdateTitleOfCourtesy;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Dtos;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Queries.GetTitleOfCourtesyById;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Queries.ListActiveTitlesOfCourtesy;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Queries.ListTitlesOfCourtesy;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Queries.LookupTitlesOfCourtesy;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Setup.API.Controllers;

/// <summary>
/// Tenant-scoped title of courtesy catalog management.
/// </summary>
//[Authorize]
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("titlesofcourtesy")]
[Produces("application/json")]
public sealed class TitlesOfCourtesyController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<TitleOfCourtesyDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetTitleOfCourtesyByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<TitleOfCourtesySummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = true,
        [FromQuery] string? code = null,
        [FromQuery] string? displayName = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListTitlesOfCourtesyQuery(
                new SetupListQuery(page, pageSize, sortBy, sortDirection, search, isActive),
                new TitleOfCourtesyListFilters(code, displayName)),
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<TitleOfCourtesyLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new LookupTitlesOfCourtesyQuery(activeOnly), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("active")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<TitleOfCourtesySummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListActive(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] string? code = null,
        [FromQuery] string? displayName = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListActiveTitlesOfCourtesyQuery(
                page,
                pageSize,
                sortBy,
                sortDirection,
                search,
                new TitleOfCourtesyListFilters(code, displayName)),
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<TitleOfCourtesyDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] TitleOfCourtesyCreateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateTitleOfCourtesyCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<TitleOfCourtesyDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] TitleOfCourtesyUpdateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateTitleOfCourtesyCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<TitleOfCourtesyDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] TitleOfCourtesyPatchDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new PatchTitleOfCourtesyCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<TitleOfCourtesyDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new DeleteTitleOfCourtesyCommand(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
