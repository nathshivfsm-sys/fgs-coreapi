using System.Text.Json;
using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.User.Application.Common;
using Fgs.User.Application.Features.Auth.Commands.EntraApiConnector;
using Fgs.User.Application.Features.Auth.Commands.EntraCallback;
using Fgs.User.Application.Features.Auth.Commands.EntraLoginCallback;
using Fgs.User.Application.Features.Auth.Commands.ExchangeLoginCode;
using Fgs.User.Application.Features.Auth.Commands.RefreshAuthToken;
using Fgs.User.Application.Features.Auth.Commands.StartLogin;
using Fgs.User.Application.Features.Auth.Dtos;
using Fgs.User.Application.Features.Auth.Queries.GetAuthMe;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.User.API.Controllers;

/// <summary>
/// Authentication endpoints (Microsoft Entra External ID).
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("auth")]
public sealed class AuthController(IMediator mediator) : FgsApiControllerBase(mediator)
{
    /// <summary>
    /// UI login: validates active platform user and returns Entra authorization URL (no invitation logic).
    /// </summary>
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<StartLoginResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> StartLogin(
        [FromBody] StartLoginCommand command,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(command, cancellationToken));

    /// <summary>
    /// OAuth2 callback after Entra login: exchanges code, validates email vs invitation, stores Entra object id, returns Entra access token.
    /// </summary>
    /// <remarks>
    /// On success, returns a small HTML page that navigates to the configured dashboard URL with the Entra access token
    /// in the query string (avoids oversized Location headers through the gateway). On failure, returns the standard JSON error envelope.
    /// </remarks>
    [AllowAnonymous]
    [HttpGet("entra/callback")]
    [Produces("text/html", "application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<EntraCallbackResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EntraCallback(
        [FromQuery] string code,
        [FromQuery] string state,
        CancellationToken cancellationToken)
    {
        if (state.StartsWith(OAuthStatePrefixes.UserLogin, StringComparison.Ordinal))
        {
            var loginResponse = await Mediator.Send(new EntraLoginCallbackCommand(code, state), cancellationToken);
            if (!loginResponse.Success || loginResponse.Data is null)
            {
                return StatusCode(loginResponse.StatusCode, loginResponse);
            }

            var loginDestination = $"{loginResponse.Data.RedirectUrl}?token={Uri.EscapeDataString(loginResponse.Data.AccessToken)}";
            return Content(BuildSignInRedirectHtml(loginDestination), "text/html; charset=utf-8");
        }

        var response = await Mediator.Send(new EntraCallbackCommand(code, state), cancellationToken);
        if (!response.Success || response.Data is null)
        {
            return StatusCode(response.StatusCode, response);
        }

        var destination = $"{response.Data.RedirectUrl}?token={Uri.EscapeDataString(response.Data.AccessToken)}";
        return Content(BuildSignInRedirectHtml(destination), "text/html; charset=utf-8");
    }

    /// <summary>
    /// Entra External ID API Connector: resolves signup email to tenant and company claims for token issuance.
    /// </summary>
    [AllowAnonymous]
    [HttpPost("entra/connector")]
    [ProducesResponseType(typeof(EntraApiConnectorResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EntraConnector(
        [FromBody] EntraApiConnectorRequestDto request,
        CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(
            new EntraApiConnectorCommand(request.Email, request.ObjectId),
            cancellationToken);

        return Ok(response);
    }

    /// <summary>
    /// Returns the authenticated user's platform profile (tenant, company, roles).
    /// </summary>
    [HttpGet("me")]
    [ProducesResponseType(typeof(ApiResponse<AuthMeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMe(CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new GetAuthMeQuery(), cancellationToken));

    /// <summary>
    /// SPA Option A: exchange Entra authorization code (+ OAuth state) for Login Profile JSON.
    /// </summary>
    [AllowAnonymous]
    [HttpPost("entra/token")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ApiResponse<LoginProfileDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ExchangeLoginCode(
        [FromBody] ExchangeLoginCodeCommand command,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(command, cancellationToken));

    /// <summary>
    /// Refresh Entra tokens and return an updated Login Profile.
    /// </summary>
    [AllowAnonymous]
    [HttpPost("refresh")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ApiResponse<LoginProfileDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh(
        [FromBody] RefreshAuthTokenCommand command,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(command, cancellationToken));

    private static string BuildSignInRedirectHtml(string destinationUrl) =>
        $"""
         <!DOCTYPE html>
         <html lang="en">
         <head>
           <meta charset="utf-8" />
           <meta name="viewport" content="width=device-width, initial-scale=1" />
           <title>Signing in...</title>
         </head>
         <body>
           <p>Signing you in...</p>
           <script>window.location.replace({JsonSerializer.Serialize(destinationUrl)});</script>
         </body>
         </html>
         """;
}
