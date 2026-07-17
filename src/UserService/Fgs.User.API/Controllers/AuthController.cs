using System.Text.Json;
using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.User.Application.Common;
using Fgs.User.Application.Features.Auth.Commands.EntraCallback;
using Fgs.User.Application.Features.Auth.Commands.EntraLoginCallback;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.User.API.Controllers;

/// <summary>
/// Authentication endpoints (Microsoft Entra External ID).
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("auth")]
public sealed partial class AuthController(IMediator mediator) : FgsApiControllerBase(mediator)
{
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

    private static string BuildSignInRedirectHtml(string destinationUrl) =>
        $"""
         <!DOCTYPE html>
         <html lang="en">
         <head>
           <meta charset="utf-8" />
           <meta name="viewport" content="width=device-width, initial-scale=1" />
           <title>Signing inâ€¦</title>
         </head>
         <body>
           <p>Signing you inâ€¦</p>
           <script>window.location.replace({JsonSerializer.Serialize(destinationUrl)});</script>
         </body>
         </html>
         """;
}
