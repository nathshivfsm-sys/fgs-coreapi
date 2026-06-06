using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Contracts.Requests;
using Fgs.Foundation.Api;
using Fgs.Notification.Application.Features.Notifications.Commands.DispatchNotification;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Notification.API.Controllers;

[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("notifications")]
[Authorize]
public sealed class NotificationsController(IMediator mediator) : ControllerBase
{
    [HttpPost("dispatch")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Dispatch(
        [FromBody] DispatchNotificationRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new DispatchNotificationCommand(request), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
