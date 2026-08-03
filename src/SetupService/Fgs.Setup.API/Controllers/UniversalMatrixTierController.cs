using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.UniversalMatrixTiers.Commands.CreateFgsUniversalMatrixTier;
using Fgs.Setup.Application.Features.UniversalMatrixTiers.Commands.PatchFgsUniversalMatrixTier;
using Fgs.Setup.Application.Features.UniversalMatrixTiers.Commands.UpdateFgsUniversalMatrixTier;
using Fgs.Setup.Application.Features.UniversalMatrixTiers.Dtos;
using Fgs.Setup.Application.Features.UniversalMatrixTiers.Queries.GetFgsUniversalMatrixTierById;
using Fgs.Setup.Application.Features.UniversalMatrixTiers.Queries.ListUniversalMatrixTiers;
using Fgs.Setup.Application.Features.UniversalMatrixTiers.Queries.LookupUniversalMatrixTiers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Setup.API.Controllers;

/// <summary>
/// Tenant-scoped universal matrix tier management.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("universalmatrixtier")]
[Produces("application/json")]
public sealed class UniversalMatrixTierController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsUniversalMatrixTierDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetFgsUniversalMatrixTierByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsUniversalMatrixTierSummaryDto>>), StatusCodes.Status200OK)]
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
            new ListUniversalMatrixTiersQuery(
                new SetupListQuery(page, pageSize, sortBy, sortDirection, search, isActive),
                new FgsUniversalMatrixTierListFilters(universalPricingServiceId, name)),
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsUniversalMatrixTierLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(
        [FromQuery] bool activeOnly = true,
        [FromQuery] long? universalPricingServiceId = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new LookupUniversalMatrixTiersQuery(activeOnly, universalPricingServiceId),
            cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsUniversalMatrixTierDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsUniversalMatrixTierCreateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateFgsUniversalMatrixTierCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsUniversalMatrixTierDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] FgsUniversalMatrixTierUpdateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateFgsUniversalMatrixTierCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsUniversalMatrixTierDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] FgsUniversalMatrixTierPatchDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new PatchFgsUniversalMatrixTierCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
