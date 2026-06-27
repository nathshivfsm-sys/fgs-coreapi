using Fgs.Contracts.Api;
using Fgs.User.Application.Features.Auth.Queries.GetAuthMe;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.User.API.Controllers;

public sealed partial class AuthController
{
    /// <summary>
    /// Returns the authenticated user's platform profile (tenant, company, roles).
    /// </summary>
    [HttpGet("me")]
    [ProducesResponseType(typeof(ApiResponse<AuthMeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMe(CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new GetAuthMeQuery(), cancellationToken));
}
