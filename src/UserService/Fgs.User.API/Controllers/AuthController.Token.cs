using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.User.Application.Features.Auth.Commands.ExchangeLoginCode;
using Fgs.User.Application.Features.Auth.Commands.RefreshAuthToken;
using Fgs.User.Application.Features.Auth.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.User.API.Controllers;

public sealed partial class AuthController
{
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
}
