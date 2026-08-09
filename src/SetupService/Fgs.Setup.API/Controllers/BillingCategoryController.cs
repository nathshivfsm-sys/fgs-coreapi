using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.BillingCategories.Commands.CreateBillingCategory;
using Fgs.Setup.Application.Features.BillingCategories.Commands.PatchBillingCategory;
using Fgs.Setup.Application.Features.BillingCategories.Commands.UpdateBillingCategory;
using Fgs.Setup.Application.Features.BillingCategories.Queries.GetBillingCategoryById;
using Fgs.Setup.Application.Features.BillingCategories.Queries.ListBillingCategories;
using Fgs.Setup.Application.Features.BillingCategories.Queries.LookupBillingCategories;
using Fgs.Setup.Application.Features.BillingCategories.Dtos;
using MediatR;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Setup.API.Controllers;

/// <summary>
/// Tenant-scoped billing category catalog management.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("billingcategory")]
[Produces("application/json")]
public sealed class BillingCategoryController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<BillingCategoryDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetBillingCategoryByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<BillingCategorySummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] string? billingCategoryType = null,
        [FromQuery] string? billingCategoryName = null,
        [FromQuery] bool? showToFieldTech = null,
        [FromQuery] bool? allowToPick = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListBillingCategoriesQuery(
                new SetupListQuery(page, pageSize, sortBy, sortDirection, search, isActive),
                new BillingCategoryListFilters(billingCategoryType, billingCategoryName, showToFieldTech, allowToPick)),
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<BillingCategoryLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(
        [FromQuery] bool activeOnly = true,
        [FromQuery] bool? showToFieldTech = null,
        [FromQuery] bool? allowToPick = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new LookupBillingCategoriesQuery(activeOnly, showToFieldTech, allowToPick), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.SetupCreate)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<BillingCategoryDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] BillingCategoryCreateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateBillingCategoryCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.SetupEdit)]
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<BillingCategoryDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] BillingCategoryUpdateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateBillingCategoryCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.SetupEdit)]
    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<BillingCategoryDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] BillingCategoryPatchDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new PatchBillingCategoryCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
