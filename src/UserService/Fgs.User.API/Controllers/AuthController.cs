using Fgs.User.Application.Features.Auth.Queries.EntraCallback;
using Fgs.User.Application.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.User.API.Controllers;

/// <summary>
/// Authentication endpoints (Microsoft Entra External ID).
/// </summary>
[ApiController]
[Route("api/auth")]
[Produces("application/json")]
public sealed class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// OAuth2 callback after Entra login: exchanges code, validates email vs invitation, stores Entra object id, issues app JWT.
    /// </summary>
    /// <remarks>
    /// On success, responds with HTTP 302 to the configured dashboard URL with access token in query string (browser flow).
    /// On failure, returns the standard JSON error envelope.
    /// </remarks>
    [HttpGet("entra/callback")]
    [ProducesResponseType(StatusCodes.Status302Found)]
    [ProducesResponseType(typeof(ApiResponse<EntraCallbackResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EntraCallback(
        [FromQuery] string code,
        [FromQuery] string state,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new EntraCallbackQuery(code, state), cancellationToken);
        if (!response.Success || response.Data is null)
        {
            return StatusCode(response.StatusCode, response);
        }

        return Redirect($"{response.Data.RedirectUrl}?token={Uri.EscapeDataString(response.Data.AccessToken)}");
    }
}
