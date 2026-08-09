using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.ApiSecrets.Commands.CreateFgsApiSecret;
using Fgs.User.Application.Features.ApiSecrets.Commands.PatchFgsApiSecret;
using Fgs.User.Application.Features.ApiSecrets.Commands.RevokeFgsApiSecret;
using Fgs.User.Application.Features.ApiSecrets.Dtos;
using Fgs.User.Application.Features.ApiSecrets.Queries.GetFgsApiSecretById;
using Fgs.User.Application.Features.ApiSecrets.Queries.ListFgsApiSecrets;
using MediatR;
using Fgs.Security.Authorization;
using Fgs.Security.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.User.API.Controllers;

/// <summary>
/// Tenant-scoped API client secret management.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("apisecret")]
[Produces("application/json")]
public sealed class ApiSecretController(IMediator mediator) : FgsApiControllerBase(mediator)
{
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsApiSecretDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new GetFgsApiSecretByIdQuery(id), cancellationToken));

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsApiSecretSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] long? fgsApiClientId = null,
        CancellationToken cancellationToken = default) =>
        FromApiResponse(await Mediator.Send(
            new ListFgsApiSecretsQuery(
                new IdentityListQuery(page, pageSize, sortBy, sortDirection, search, isActive),
                new FgsApiSecretListFilters(fgsApiClientId)),
            cancellationToken));

    [RequirePermission(FgsPermissionCodes.UserCreate)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsApiSecretCreateResultDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] FgsApiSecretCreateDto request,
        CancellationToken cancellationToken) =>
        CreatedFromApiResponse(await Mediator.Send(new CreateFgsApiSecretCommand(request), cancellationToken));

    [RequirePermission(FgsPermissionCodes.UserEdit)]
    [HttpPatch("{id:long}")]
    [ProducesResponseType(typeof(ApiResponse<FgsApiSecretDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Patch(
        long id,
        [FromBody] FgsApiSecretPatchDto request,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new PatchFgsApiSecretCommand(id, request), cancellationToken));

    [RequirePermission(FgsPermissionCodes.UserCreate)]
    [HttpPost("{id:long}/revoke")]
    [ProducesResponseType(typeof(ApiResponse<FgsApiSecretDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Revoke(long id, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new RevokeFgsApiSecretCommand(id), cancellationToken));
}
