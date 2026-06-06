using Fgs.Contracts.Api;
using Fgs.Notification.Application.Notifications.Channels;
using Fgs.Notification.Application.Notifications.Channels.Models;
using Fgs.Notification.Application.Notifications.Dispatch;
using Fgs.Notification.Application.Notifications.Queues;
using MediatR;

namespace Fgs.Notification.Application.Features.Notifications.Commands.DispatchNotification;

public sealed class DispatchNotificationCommandHandler(
    INotificationDispatchRequestResolver resolver,
    IIdempotencyStore idempotency,
    INotificationDispatcher dispatcher)
    : IRequestHandler<DispatchNotificationCommand, ApiResponse<object>>
{
    public async Task<ApiResponse<object>> Handle(
        DispatchNotificationCommand command,
        CancellationToken cancellationToken)
    {
        var resolved = resolver.Resolve(command.Request);
        if (resolved.IsNoContent)
        {
            return ApiResponse<object>.Ok(new object());
        }

        if (resolved.IsFailure)
        {
            return ApiResponse<object>.Fail(resolved.Errors, ApiStatusCodes.BadRequest);
        }

        if (resolved.RequiresIdempotency
            && !await idempotency.TryMarkProcessedAsync(
                resolved.MessageId!,
                resolved.IdempotencyKey!,
                cancellationToken))
        {
            return ApiResponse<object>.Ok(new object());
        }

        return await DispatchAsync(resolved.DispatchRequest!, cancellationToken);
    }

    private async Task<ApiResponse<object>> DispatchAsync(
        NotificationDispatchRequest dispatchRequest,
        CancellationToken cancellationToken)
    {
        var result = await dispatcher.DispatchAsync(dispatchRequest, cancellationToken);
        if (!result.Success)
        {
            return ApiResponse<object>.Fail(
                [result.Error ?? "Notification dispatch failed."],
                ApiStatusCodes.InternalServerError);
        }

        return ApiResponse<object>.Ok(new object());
    }
}
