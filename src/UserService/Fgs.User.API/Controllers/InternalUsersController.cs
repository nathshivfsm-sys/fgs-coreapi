using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Contracts.Auth;
using Fgs.Contracts.Clients;
using Fgs.Credentials;
using Fgs.Credentials.Options;
using Fgs.Foundation.Api;
using Fgs.User.Application.Features.Auth.Queries.GetUserAuthProfile;
using Fgs.User.Application.Features.Users.Queries.GetUserIdsByRoles;
using Fgs.User.Application.Features.Users.Queries.GetUserListEnrichment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Fgs.User.API.Controllers;

/// <summary>
/// Internal user endpoints. <c>auth-profile</c> remains dual-auth (JWT or service key)
/// for ActiveUserAuthorizationMiddleware / profile bootstrap. <c>ids-by-roles</c> and
/// <c>list-enrichment</c> require a validated caller JWT like other User APIs.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("internal/users")]
public sealed class InternalUsersController(
    IMediator mediator,
    IOptions<CredentialDistributionOptions> distributionOptions) : FgsApiControllerBase(mediator)
{
    /// <summary>
    /// Auth bootstrap / profile load. Must remain usable with service key when no user JWT
    /// is available (S2S profile store). Prefer Bearer when present.
    /// </summary>
    [AllowAnonymous]
    [HttpGet("auth-profile")]
    [ProducesResponseType(typeof(ApiResponse<UserAuthProfileDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAuthProfile(
        [FromQuery] string entraObjectId,
        [FromHeader(Name = InternalServiceHeaders.ServiceKey)] string? serviceKey,
        CancellationToken cancellationToken)
    {
        var unauthorized = UnauthorizedIfNotInternalOrAuthenticated(serviceKey);
        if (unauthorized is not null)
        {
            return unauthorized;
        }

        var response = await Mediator.Send(new GetUserAuthProfileQuery(entraObjectId), cancellationToken);
        if (!response.Success || response.Data is null)
        {
            return StatusCode(response.StatusCode, response);
        }

        var dto = new UserAuthProfileDto(
            response.Data.UserId,
            response.Data.Email,
            response.Data.EntraObjectId,
            response.Data.TenantId,
            response.Data.CompanyId,
            response.Data.IsActive,
            response.Data.IsDeleted,
            response.Data.Roles,
            response.Data.Permissions,
            response.Data.DataAccess,
            response.Data.PublicEndpoints);

        return Ok(ApiResponse<UserAuthProfileDto>.Ok(dto));
    }

    [HttpGet("ids-by-roles")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<Guid>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUserIdsByRoles(
        [FromQuery] IReadOnlyList<long>? roleIds,
        CancellationToken cancellationToken) =>
        FromApiResponse(
            await Mediator.Send(new GetUserIdsByRolesQuery(roleIds ?? []), cancellationToken));

    [HttpGet("list-enrichment")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<FgsUserListEnrichmentDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetListEnrichment(
        [FromQuery] IReadOnlyList<Guid>? userIds,
        CancellationToken cancellationToken) =>
        FromApiResponse(
            await Mediator.Send(new GetUserListEnrichmentQuery(userIds ?? []), cancellationToken));

    private IActionResult? UnauthorizedIfNotInternalOrAuthenticated(string? serviceKey)
    {
        if (InternalServiceAuthorization.IsAuthorizedOrUserAuthenticated(
                serviceKey,
                distributionOptions.Value,
                User))
        {
            return null;
        }

        return StatusCode(
            StatusCodes.Status401Unauthorized,
            ApiResponse<object>.Fail(
                ["Authentication required. Provide a valid JWT or internal service key."],
                ApiStatusCodes.Unauthorized));
    }
}
