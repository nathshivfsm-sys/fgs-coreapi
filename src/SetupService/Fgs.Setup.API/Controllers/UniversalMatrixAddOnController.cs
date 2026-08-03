using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Commands.CreateFgsUniversalMatrixAddOn;
using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Commands.PatchFgsUniversalMatrixAddOn;
using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Commands.UpdateFgsUniversalMatrixAddOn;
using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Dtos;
using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Queries.GetFgsUniversalMatrixAddOnById;
using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Queries.ListUniversalMatrixAddOns;
using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Queries.LookupUniversalMatrixAddOns;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Setup.API.Controllers;

/// <summary>
/// Tenant-scoped universal matrix add-on management.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("universalmatrixaddon")]
[Produces("application/json")]
public sealed class UniversalMatrixAddOnController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsUniversalMatrixAddOnDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetFgsUniversalMatrixAddOnByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsUniversalMatrixAddOnSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] long? universalPricingServiceId = null,
        [FromQuery] string? name = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListUniversalMatrixAddOnsQuery(
                new SetupListQuery(page, pageSize, sortBy, sortDirection, search, isActive),
                new FgsUniversalMatrixAddOnListFilters(universalPricingServiceId, name)),
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsUniversalMatrixAddOnLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(
        [FromQuery] bool activeOnly = true,
        [FromQuery] long? universalPricingServiceId = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new LookupUniversalMatrixAddOnsQuery(activeOnly, universalPricingServiceId),
            cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsUniversalMatrixAddOnDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsUniversalMatrixAddOnCreateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateFgsUniversalMatrixAddOnCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsUniversalMatrixAddOnDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] FgsUniversalMatrixAddOnUpdateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateFgsUniversalMatrixAddOnCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsUniversalMatrixAddOnDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] FgsUniversalMatrixAddOnPatchDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new PatchFgsUniversalMatrixAddOnCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
