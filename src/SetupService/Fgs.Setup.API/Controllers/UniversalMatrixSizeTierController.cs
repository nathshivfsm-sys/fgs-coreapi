using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Commands.CreateFgsUniversalMatrixSizeTier;
using Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Commands.PatchFgsUniversalMatrixSizeTier;
using Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Commands.UpdateFgsUniversalMatrixSizeTier;
using Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Dtos;
using Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Queries.GetFgsUniversalMatrixSizeTierById;
using Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Queries.ListUniversalMatrixSizeTiers;
using Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Queries.LookupUniversalMatrixSizeTiers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Setup.API.Controllers;

/// <summary>
/// Tenant-scoped universal matrix size tier management.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("universalmatrixsizetier")]
[Produces("application/json")]
public sealed class UniversalMatrixSizeTierController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsUniversalMatrixSizeTierDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetFgsUniversalMatrixSizeTierByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsUniversalMatrixSizeTierSummaryDto>>), StatusCodes.Status200OK)]
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
            new ListUniversalMatrixSizeTiersQuery(
                new SetupListQuery(page, pageSize, sortBy, sortDirection, search, isActive),
                new FgsUniversalMatrixSizeTierListFilters(universalPricingServiceId, name)),
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsUniversalMatrixSizeTierLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(
        [FromQuery] bool activeOnly = true,
        [FromQuery] long? universalPricingServiceId = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new LookupUniversalMatrixSizeTiersQuery(activeOnly, universalPricingServiceId),
            cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsUniversalMatrixSizeTierDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsUniversalMatrixSizeTierCreateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateFgsUniversalMatrixSizeTierCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsUniversalMatrixSizeTierDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] FgsUniversalMatrixSizeTierUpdateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateFgsUniversalMatrixSizeTierCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsUniversalMatrixSizeTierDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] FgsUniversalMatrixSizeTierPatchDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new PatchFgsUniversalMatrixSizeTierCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
