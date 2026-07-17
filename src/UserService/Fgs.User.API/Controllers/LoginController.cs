using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Foundation.Api;
using Fgs.User.Application.Features.Auth.Commands.StartLogin;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.User.API.Controllers;

/// <summary>
/// UI login: validates active platform user and returns Entra authorization URL (no invitation logic).
/// </summary>
[AllowAnonymous]
[ApiController]
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("login")]
[Produces("application/json")]
public sealed class LoginController(IMediator mediator) : FgsApiControllerBase(mediator)
{
    /// <summary>
    /// Validates the user email is active in the platform database and returns the Entra redirect URL for the UI.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<StartLoginResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Start(
        [FromBody] StartLoginCommand command,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(command, cancellationToken));
}
