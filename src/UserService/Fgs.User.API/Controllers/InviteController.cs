using Asp.Versioning;
using Fgs.Foundation.Api;
using Fgs.User.API.Constants;
using Fgs.User.Application.Features.Invitations.Commands.StartInvitation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.User.API.Controllers;

/// <summary>
/// Email invitation deep links (pre–Entra redirect).
/// </summary>
[ApiController]
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("invite")]
public sealed class InviteController : ControllerBase
{
    private readonly IMediator _mediator;

    public InviteController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Validates the invitation token and redirects the browser to Microsoft Entra External ID authorization.
    /// </summary>
    /// <param name="token">Opaque token from the signup invitation email.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <remarks>
    /// Returns 302 to Entra on success (including when the invitation was already accepted — user is sent to Entra sign-in);
    /// 400 with error body on invalid or expired token.
    /// </remarks>
    [AllowAnonymous]
    [HttpGet("start")]
    [ProducesResponseType(StatusCodes.Status302Found)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Start([FromQuery] string token, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new StartInvitationCommand(token), cancellationToken);
        if (!result.Success || string.IsNullOrWhiteSpace(result.RedirectUrl))
        {
            return BadRequest(new { success = false, errors = new[] { result.ErrorMessage ?? ApiErrorMessages.InvalidInvitation } });
        }

        return Redirect(result.RedirectUrl);
    }
}
