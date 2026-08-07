using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Commands.CreateFgsSetupTaxAuthority;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Commands.PatchFgsSetupTaxAuthority;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Commands.UpdateFgsSetupTaxAuthority;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Queries.GetFgsSetupTaxAuthorityById;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Queries.ListSetupTaxAuthorities;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Queries.LookupSetupTaxAuthorities;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Dtos;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Setup.API.Controllers;

/// <summary>
/// Tenant-scoped tax authority catalog management.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("taxauthority")]
[Produces("application/json")]
public sealed class TaxAuthorityController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsSetupTaxAuthorityDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetFgsSetupTaxAuthorityByIdQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsSetupTaxAuthoritySummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] string? code = null,
        [FromQuery] string? name = null,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(
            new ListSetupTaxAuthoritiesQuery(
                new SetupListQuery(page, pageSize, sortBy, sortDirection, search, isActive),
                new FgsSetupTaxAuthorityListFilters(code, name)),
            cancellationToken);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsSetupTaxAuthorityLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new LookupSetupTaxAuthoritiesQuery(activeOnly), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    [RequirePermission(FgsPermissionCodes.SetupCreate)]
    [ProducesResponseType(typeof(ApiResponse<FgsSetupTaxAuthorityDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsSetupTaxAuthorityCreateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateFgsSetupTaxAuthorityCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("{id:long}")]
    [RequirePermission(FgsPermissionCodes.SetupEdit)]
    [ProducesResponseType(typeof(ApiResponse<FgsSetupTaxAuthorityDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] FgsSetupTaxAuthorityUpdateDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateFgsSetupTaxAuthorityCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPatch("{id:long}")]
    [RequirePermission(FgsPermissionCodes.SetupEdit)]
    [ProducesResponseType(typeof(ApiResponse<FgsSetupTaxAuthorityDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] FgsSetupTaxAuthorityPatchDto request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new PatchFgsSetupTaxAuthorityCommand(id, request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
