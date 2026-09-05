using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.User.Application.Features.Auth.Commands.EntraApiConnector;
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
}
