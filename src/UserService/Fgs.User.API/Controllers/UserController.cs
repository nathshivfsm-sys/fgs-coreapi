using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.Users.Commands.InviteFgsUser;
using Fgs.User.Application.Features.Users.Commands.PatchFgsUser;
using Fgs.User.Application.Features.Users.Commands.ResendFgsUserInvite;
using Fgs.User.Application.Features.Users.Commands.UpdateFgsUser;
using Fgs.User.Application.Features.Users.Dtos;
using Fgs.User.Application.Features.Users.Queries.GetFgsUserById;
using Fgs.User.Application.Features.Users.Queries.ListFgsUsers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.User.API.Controllers;

/// <summary>
/// Tenant-scoped back-office user invite and management.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("user")]
[Produces("application/json")]
public sealed class UserController(IMediator mediator) : FgsApiControllerBase(mediator)
{
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<FgsUserDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new GetFgsUserByIdQuery(id), cancellationToken));

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FgsUserSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? sortBy = null,
        [FromQuery] SortDirection sortDirection = SortDirection.Asc,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] string? email = null,
        [FromQuery] string? displayName = null,
        [FromQuery] long? roleId = null,
        CancellationToken cancellationToken = default) =>
        FromApiResponse(await Mediator.Send(
            new ListFgsUsersQuery(
                new IdentityListQuery(page, pageSize, sortBy, sortDirection, search, isActive),
                new FgsUserListFilters(email, displayName, roleId)),
            cancellationToken));

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FgsUserDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Invite(
        [FromBody] FgsUserInviteDto request,
        CancellationToken cancellationToken) =>
        CreatedFromApiResponse(await Mediator.Send(new InviteFgsUserCommand(request), cancellationToken));

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<FgsUserDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] FgsUserUpdateDto request,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new UpdateFgsUserCommand(id, request), cancellationToken));

    [HttpPatch("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<FgsUserDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Patch(
        Guid id,
        [FromBody] FgsUserPatchDto request,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new PatchFgsUserCommand(id, request), cancellationToken));

    [HttpPost("{id:guid}/resendinvite")]
    [ProducesResponseType(typeof(ApiResponse<FgsUserDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ResendInvite(Guid id, CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new ResendFgsUserInviteCommand(id), cancellationToken));
}
