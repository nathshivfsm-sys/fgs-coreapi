using Asp.Versioning;
using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Contracts.Requests;
using Fgs.Credentials;
using Fgs.Credentials.Options;
using Fgs.Foundation.Api;
using Fgs.Notification.Application.Features.Notifications.Commands.DispatchNotification;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Fgs.Notification.API.Controllers;

/// <summary>
/// Internal notification dispatch. Authenticated via internal service key (not public JWT).
/// </summary>
[ApiVersion(FgsApiVersions.V1)]
[FgsVersionedRoute("notification")]
public sealed class NotificationController(
    IMediator mediator,
    IOptions<CredentialDistributionOptions> distributionOptions) : FgsApiControllerBase(mediator)
{
    [AllowAnonymous]
    [HttpPost("dispatch")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Dispatch(
        [FromBody] DispatchNotificationRequest request,
        [FromHeader(Name = InternalServiceHeaders.ServiceKey)] string? serviceKey,
        CancellationToken cancellationToken)
    {
        if (!InternalServiceAuthorization.IsAuthorized(serviceKey, distributionOptions.Value))
        {
            return StatusCode(
                StatusCodes.Status401Unauthorized,
                ApiResponse<object>.Fail(
                    ["Internal service key is missing or invalid."],
                    ApiStatusCodes.Unauthorized));
        }

        return FromApiResponse(
            await Mediator.Send(new DispatchNotificationCommand(request), cancellationToken));
    }
}
