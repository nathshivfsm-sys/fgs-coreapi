using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.ResolutionCodes.Commands.CreateResolutionCode;
using Fgs.Setup.Application.Features.ResolutionCodes.Commands.PatchResolutionCode;
using Fgs.Setup.Application.Features.ResolutionCodes.Commands.UpdateResolutionCode;
using Fgs.Setup.Application.Features.ResolutionCodes.Queries.GetResolutionCodeById;
using Fgs.Setup.Application.Features.ResolutionCodes.Queries.ListResolutionCodes;
using Fgs.Setup.Application.Features.ResolutionCodes.Queries.LookupResolutionCodes;
using Fgs.Setup.Application.Features.ResolutionCodes.Dtos;
using MediatR;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Setup.API.Controllers;

/// <summary>
/// Tenant-scoped resolution code catalog management.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("resolutioncode")]
[Produces("application/json")]
public sealed class ResolutionCodeController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<ResolutionCodeDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetResolutionCodeByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ResolutionCodeSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] string? resolutionCode = null,
        [FromQuery] string? resolutionName = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListResolutionCodesQuery(
                new SetupListQuery(page, pageSize, sortBy, sortDirection, search, isActive),
                new ResolutionCodeListFilters(resolutionCode, resolutionName)),
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ResolutionCodeLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(
        [FromQuery] bool activeOnly = true,
        [FromQuery] bool? isMobileVisible = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new LookupResolutionCodesQuery(activeOnly, isMobileVisible), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.SetupCreate)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ResolutionCodeDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] ResolutionCodeCreateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateResolutionCodeCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.SetupEdit)]
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<ResolutionCodeDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] ResolutionCodeUpdateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateResolutionCodeCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.SetupEdit)]
    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<ResolutionCodeDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] ResolutionCodePatchDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new PatchResolutionCodeCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
