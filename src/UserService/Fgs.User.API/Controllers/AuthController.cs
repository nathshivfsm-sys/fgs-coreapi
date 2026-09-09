using Asp.Versioning;
using System.Text.Json;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.User.Application.Common;
using Fgs.User.Application.Features.Auth.Commands.EntraApiConnector;
using Fgs.User.Application.Features.Auth.Commands.EntraAttributeCollectionStart;
using Fgs.User.Application.Features.Auth.Commands.ExchangeLoginCode;
using Fgs.User.Application.Features.Auth.Commands.RefreshAuthToken;
using Fgs.User.Application.Features.Auth.Commands.StartLogin;
using Fgs.User.Application.Features.Auth.Dtos;
using Fgs.User.Application.Features.Auth.Queries.GetAuthMe;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Fgs.User.API.Controllers;

/// <summary>
/// Authentication endpoints (Microsoft Entra External ID).
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("auth")]
public sealed class AuthController(IMediator mediator, IConfiguration configuration) : FgsApiControllerBase(mediator)
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
    /// Entra OAuth redirect target: validates <c>code</c>/<c>state</c> (PKCE), then redirects the browser
    /// to <c>Application:UiPostLoginRedirectUrl</c> with access and refresh tokens.
    /// </summary>
    /// <remarks>
    /// Register this URL as a Web redirect URI in Entra. Prefer HTML navigation over a raw 302 so large
    /// tokens are not truncated by gateway Location-header limits.
    /// The UI should call <c>POST /api/v1/auth/refresh</c> with <c>refresh_token</c> to obtain a full Login Profile.
    /// </remarks>
    [AllowAnonymous]
    [HttpGet("entra/callback")]
    [Produces("text/html", "application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> EntraCallback(
        [FromQuery] string? code,
        [FromQuery] string? state,
        [FromQuery] string? error,
        [FromQuery(Name = "error_description")] string? errorDescription,
        CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(error))
        {
            var detail = string.IsNullOrWhiteSpace(errorDescription)
                ? error
                : $"{error}: {errorDescription}";
            return StatusCode(
                StatusCodes.Status400BadRequest,
                ApiResponse<object>.Fail([detail], ApiStatusCodes.BadRequest));
        }

        if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(state))
        {
            return StatusCode(
                StatusCodes.Status400BadRequest,
                ApiResponse<object>.Fail(
                    ["Authorization code and state are required."],
                    ApiStatusCodes.BadRequest));
        }

        var response = await Mediator.Send(new ExchangeLoginCodeCommand(code, state), cancellationToken);
        if (!response.Success || response.Data is null)
        {
            return StatusCode(response.StatusCode, response);
        }

        var postLoginBase = ApplicationPublicUrlResolver.ResolveUiPostLoginRedirectUrl(configuration)
            .TrimEnd('/');
        var query = new List<string>
        {
            $"token={Uri.EscapeDataString(response.Data.AccessToken)}"
        };
        if (!string.IsNullOrWhiteSpace(response.Data.RefreshToken))
        {
            query.Add($"refresh_token={Uri.EscapeDataString(response.Data.RefreshToken)}");
        }

        var destination = $"{postLoginBase}?{string.Join("&", query)}";

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
    /// Entra External ID custom authentication extension: OnAttributeCollectionStart.
    /// Prefills Display Name from the company-signup contact name (<c>FgsUser.DisplayName</c>).
    /// </summary>
    [AllowAnonymous]
    [HttpPost("entra/attribute-collection/start")]
    [ProducesResponseType(typeof(EntraAttributeCollectionStartResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> EntraAttributeCollectionStart(
        [FromBody] EntraAttributeCollectionStartRequestDto request,
        CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(
            new EntraAttributeCollectionStartCommand(request),
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
    /// Exchange Entra authorization code (+ OAuth state) for Login Profile JSON.
    /// Handles login (<c>userlogin:{userId}</c> state) and invite/signup (invitation Guid state),
    /// including invite finalize and tenant provisioning for new company signup.
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
