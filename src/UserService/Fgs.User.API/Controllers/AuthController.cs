using Asp.Versioning;
using Fgs.Foundation.Api;
using Fgs.Security.Abstractions;
using Fgs.Security.Models;
using Fgs.User.Application.Features.Auth.Queries.EntraCallback;
using Fgs.Foundation.Result;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.User.API.Controllers;

/// <summary>
/// Authentication endpoints (Microsoft Entra External ID).
/// </summary>
[ApiController]
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("auth")]
[Produces("application/json")]
public sealed class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// OAuth2 callback after Entra login: exchanges code, validates email vs invitation, stores Entra object id, returns Entra access token.
    /// </summary>
    /// <remarks>
    /// On success, responds with HTTP 302 to the configured dashboard URL with Entra access token in query string (browser flow).
    /// On failure, returns the standard JSON error envelope.
    /// </remarks>
    [AllowAnonymous]
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

    /// <summary>Returns the authenticated FGS user profile resolved from Entra identity and database roles.</summary>
    [HttpGet("me")]
    [ProducesResponseType(typeof(FgsAuthenticatedUserProfile), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Me([FromServices] IFgsUserContext userContext)
    {
        if (!userContext.IsAuthenticated
            || userContext.UserId is null
            || userContext.EntraObjectId is null
            || userContext.TenantId is null
            || userContext.CompanyId is null
            || string.IsNullOrWhiteSpace(userContext.Email))
        {
            return Unauthorized();
        }

        return Ok(new FgsAuthenticatedUserProfile(
            userContext.UserId.Value,
            userContext.Email,
            userContext.EntraObjectId,
            userContext.TenantId.Value,
            userContext.CompanyId.Value,
            userContext.Roles));
    }
}
