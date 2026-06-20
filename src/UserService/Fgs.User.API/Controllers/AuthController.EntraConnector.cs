using Fgs.User.Application.Features.Auth.Commands.EntraApiConnector;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.User.API.Controllers;

public sealed partial class AuthController
{
    /// <summary>
    /// Entra External ID API Connector: resolves signup email to tenant and company claims for token issuance.
    /// </summary>
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
}
