using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.TechTrades.Commands.CreateTechTrade;
using Fgs.Setup.Application.Features.TechTrades.Commands.PatchTechTrade;
using Fgs.Setup.Application.Features.TechTrades.Commands.UpdateTechTrade;
using Fgs.Setup.Application.Features.TechTrades.Dtos;
using Fgs.Setup.Application.Features.TechTrades.Queries.GetTechTradeById;
using Fgs.Setup.Application.Features.TechTrades.Queries.ListTechTrades;
using Fgs.Setup.Application.Features.TechTrades.Queries.LookupTechTrades;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Setup.API.Controllers;

/// <summary>
/// Tenant-scoped tech trade catalog management.
/// </summary>
//[Authorize]
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("techtrade")]
[Produces("application/json")]
public sealed class TechTradeController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<TechTradeDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetTechTradeByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<TechTradeSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] string? tradeCode = null,
        [FromQuery] string? name = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListTechTradesQuery(
                new SetupListQuery(page, pageSize, sortBy, sortDirection, search, isActive),
                new TechTradeListFilters(tradeCode, name)),
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<TechTradeLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new LookupTechTradesQuery(activeOnly), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.SetupCreate)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<TechTradeDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] TechTradeCreateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateTechTradeCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.SetupEdit)]
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<TechTradeDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] TechTradeUpdateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateTechTradeCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.SetupEdit)]
    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<TechTradeDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] TechTradePatchDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new PatchTechTradeCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
