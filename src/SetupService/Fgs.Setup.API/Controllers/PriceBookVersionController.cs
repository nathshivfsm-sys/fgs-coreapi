using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPriceBookVersions.Commands.CreateFgsSetupPriceBookVersion;
using Fgs.Setup.Application.Features.SetupPriceBookVersions.Commands.PatchFgsSetupPriceBookVersion;
using Fgs.Setup.Application.Features.SetupPriceBookVersions.Commands.UpdateFgsSetupPriceBookVersion;
using Fgs.Setup.Application.Features.SetupPriceBookVersions.Dtos;
using Fgs.Setup.Application.Features.SetupPriceBookVersions.Queries.GetFgsSetupPriceBookVersionById;
using Fgs.Setup.Application.Features.SetupPriceBookVersions.Queries.ListSetupPriceBookVersions;
using Fgs.Setup.Application.Features.SetupPriceBookVersions.Queries.LookupSetupPriceBookVersions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Setup.API.Controllers;

/// <summary>
/// Tenant-scoped price book version calendar management.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("pricebookversion")]
[Produces("application/json")]
public sealed class PriceBookVersionController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsSetupPriceBookVersionDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetFgsSetupPriceBookVersionByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsSetupPriceBookVersionSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] string? version = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListSetupPriceBookVersionsQuery(
                new SetupListQuery(page, pageSize, sortBy, sortDirection, search, isActive),
                new FgsSetupPriceBookVersionListFilters(version)),
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsSetupPriceBookVersionLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new LookupSetupPriceBookVersionsQuery(activeOnly), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.SetupCreate)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsSetupPriceBookVersionDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsSetupPriceBookVersionCreateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateFgsSetupPriceBookVersionCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.SetupEdit)]
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsSetupPriceBookVersionDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] FgsSetupPriceBookVersionUpdateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateFgsSetupPriceBookVersionCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [RequirePermission(FgsPermissionCodes.SetupEdit)]
    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsSetupPriceBookVersionDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] FgsSetupPriceBookVersionPatchDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new PatchFgsSetupPriceBookVersionCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
