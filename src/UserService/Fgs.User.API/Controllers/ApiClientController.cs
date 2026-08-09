using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.ApiClients.Commands.CreateFgsApiClient;
using Fgs.User.Application.Features.ApiClients.Commands.PatchFgsApiClient;
using Fgs.User.Application.Features.ApiClients.Commands.UpdateFgsApiClient;
using Fgs.User.Application.Features.ApiClients.Dtos;
using Fgs.User.Application.Features.ApiClients.Queries.GetFgsApiClientById;
using Fgs.User.Application.Features.ApiClients.Queries.ListFgsApiClients;
using Fgs.User.Application.Features.ApiClients.Queries.LookupFgsApiClients;
using MediatR;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.User.API.Controllers;

/// <summary>
/// Tenant-scoped API client application management.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("apiclient")]
[Produces("application/json")]
public sealed class ApiClientController(IMediator mediator) : FgsApiControllerBase(mediator)
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsApiClientDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new GetFgsApiClientByIdQuery(id), cancellationToken));

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsApiClientSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] string? applicationName = null,
        [FromQuery] string? contactEmail = null,
        CancellationToken cancellationToken = default) =>
        FromApiResponse(await Mediator.Send(
            new ListFgsApiClientsQuery(
                new IdentityListQuery(page, pageSize, sortBy, sortDirection, search, isActive),
                new FgsApiClientListFilters(applicationName, contactEmail)),
            cancellationToken));

    [HttpGet("lookup")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsApiClientLookupDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(
        [FromQuery] bool activeOnly = true,
        CancellationToken cancellationToken = default) =>
        FromApiResponse(await Mediator.Send(new LookupFgsApiClientsQuery(activeOnly), cancellationToken));

    [RequirePermission(FgsPermissionCodes.UserCreate)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsApiClientDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsApiClientCreateDto request,
        CancellationToken cancellationToken) =>
        CreatedFromApiResponse(await Mediator.Send(new CreateFgsApiClientCommand(request), cancellationToken));

    [RequirePermission(FgsPermissionCodes.UserEdit)]
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsApiClientDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] FgsApiClientUpdateDto request,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new UpdateFgsApiClientCommand(id, request), cancellationToken));

    [RequirePermission(FgsPermissionCodes.UserEdit)]
    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsApiClientDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] FgsApiClientPatchDto request,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new PatchFgsApiClientCommand(id, request), cancellationToken));
}
