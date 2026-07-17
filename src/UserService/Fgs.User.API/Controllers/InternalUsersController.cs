using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Contracts.Auth;
using Fgs.Contracts.Clients;
using Fgs.Credentials;
using Fgs.Credentials.Options;
using Fgs.Foundation.Api;
using Fgs.User.Application.Features.Auth.Queries.GetUserAuthProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Fgs.User.API.Controllers;

/// <summary>
/// Internal service-to-service user auth profile endpoints.
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("internal/users")]
public sealed class InternalUsersController(
    IMediator mediator,
    IOptions<CredentialDistributionOptions> distributionOptions) : FgsApiControllerBase(mediator)
{
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
        var unauthorized = UnauthorizedIfNotInternal(serviceKey);
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

    private IActionResult? UnauthorizedIfNotInternal(string? serviceKey)
    {
        if (InternalServiceAuthorization.IsAuthorized(serviceKey, distributionOptions.Value))
        {
            return null;
        }

        return StatusCode(
            StatusCodes.Status401Unauthorized,
            ApiResponse<object>.Fail(["Unauthorized."], ApiStatusCodes.Unauthorized));
    }
}
