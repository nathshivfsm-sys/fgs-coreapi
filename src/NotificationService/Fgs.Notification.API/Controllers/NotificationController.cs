using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Contracts.Requests;
using Fgs.Foundation.Api;
using Fgs.Notification.Application.Features.Notifications.Commands.DispatchNotification;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fgs.Notification.API.Controllers;

[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("notification")]
public sealed class NotificationController(IMediator mediator) : FgsApiControllerBase(mediator)
{
    [HttpPost("dispatch")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Dispatch(
        [FromBody] DispatchNotificationRequest request,
        CancellationToken cancellationToken) =>
        FromApiResponse(await Mediator.Send(new DispatchNotificationCommand(request), cancellationToken));
}
