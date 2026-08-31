using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Commands.CreateFgsEntityDefaultTermsCondition;
using Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Commands.PatchFgsEntityDefaultTermsCondition;
using Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Commands.UpdateFgsEntityDefaultTermsCondition;
using Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Dtos;
using Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Queries.GetFgsEntityDefaultTermsConditionById;
using Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Queries.ListFgsEntityDefaultTermsConditions;
using Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Queries.LookupFgsEntityDefaultTermsConditions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Setup.API.Controllers;

/// <summary>
/// Tenant-scoped default terms and conditions assignment per entity type.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("entitydefaulttermscondition")]
[Produces("application/json")]
public sealed class EntityDefaultTermsConditionController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsEntityDefaultTermsConditionDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetFgsEntityDefaultTermsConditionByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsEntityDefaultTermsConditionSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] string? entityType = null,
        [FromQuery] long? termsConditionId = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListFgsEntityDefaultTermsConditionsQuery(
                new SetupListQuery(page, pageSize, sortBy, sortDirection, search, isActive),
                new FgsEntityDefaultTermsConditionListFilters(entityType, termsConditionId)),
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsEntityDefaultTermsConditionLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new LookupFgsEntityDefaultTermsConditionsQuery(activeOnly),
            cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.SetupCreate)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsEntityDefaultTermsConditionDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsEntityDefaultTermsConditionCreateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(
            new CreateFgsEntityDefaultTermsConditionCommand(request),
            cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.SetupEdit)]
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsEntityDefaultTermsConditionDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] FgsEntityDefaultTermsConditionUpdateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(
            new UpdateFgsEntityDefaultTermsConditionCommand(id, request),
            cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.SetupEdit)]
    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsEntityDefaultTermsConditionDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] FgsEntityDefaultTermsConditionPatchDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(
            new PatchFgsEntityDefaultTermsConditionCommand(id, request),
            cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
