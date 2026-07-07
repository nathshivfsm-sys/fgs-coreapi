using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Commands.CreateFgsUniversalMatrixFrequencyDiscount;
using Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Commands.PatchFgsUniversalMatrixFrequencyDiscount;
using Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Commands.UpdateFgsUniversalMatrixFrequencyDiscount;
using Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Queries.GetFgsUniversalMatrixFrequencyDiscountById;
using Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Queries.ListUniversalMatrixFrequencyDiscounts;
using Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Queries.LookupUniversalMatrixFrequencyDiscounts;
using Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Setup.API.Controllers;

/// <summary>
/// Tenant-scoped universal matrix frequency discount catalog management.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("universalmatrixfrequencydiscount")]
[Produces("application/json")]
public sealed class UniversalMatrixFrequencyDiscountController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsUniversalMatrixFrequencyDiscountDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetFgsUniversalMatrixFrequencyDiscountByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsUniversalMatrixFrequencyDiscountSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] string? name = null,
        [FromQuery] long? universalPricingServiceId = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListUniversalMatrixFrequencyDiscountsQuery(
                new SetupListQuery(page, pageSize, sortBy, sortDirection, search, isActive),
                new FgsUniversalMatrixFrequencyDiscountListFilters(name, universalPricingServiceId)),
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsUniversalMatrixFrequencyDiscountLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(
        [FromQuery] bool activeOnly = true,
        [FromQuery] long? universalPricingServiceId = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new LookupUniversalMatrixFrequencyDiscountsQuery(activeOnly, universalPricingServiceId), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsUniversalMatrixFrequencyDiscountDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsUniversalMatrixFrequencyDiscountCreateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateFgsUniversalMatrixFrequencyDiscountCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsUniversalMatrixFrequencyDiscountDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] FgsUniversalMatrixFrequencyDiscountUpdateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateFgsUniversalMatrixFrequencyDiscountCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsUniversalMatrixFrequencyDiscountDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] FgsUniversalMatrixFrequencyDiscountPatchDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new PatchFgsUniversalMatrixFrequencyDiscountCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
