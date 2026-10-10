using Fgs.Contracts.Api;
using Fgs.Notification.Application.Notifications.Channels;
using Fgs.Notification.Application.Notifications.Channels.Models;
using Fgs.Notification.Application.Notifications.Dispatch;
using Fgs.Notification.Application.Notifications.Queues;
using Fgs.Notification.Application.Notifications.Templates;
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

        var response = await DispatchAsync(resolved, cancellationToken);
        if (!response.Success && resolved.RequiresIdempotency)
        {
            await idempotency.ReleaseAsync(resolved.MessageId!, cancellationToken);
        }

        return response;
    }

    private async Task<ApiResponse<object>> DispatchAsync(
        NotificationDispatchResolveResult resolved,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await dispatcher.DispatchAsync(resolved.DispatchRequest!, cancellationToken);
            if (!result.Success)
            {
                return ApiResponse<object>.Fail(
                    [result.Error ?? "Notification dispatch failed."],
                    ApiStatusCodes.InternalServerError);
            }

            return ApiResponse<object>.Ok(new object());
        }
        catch (CommunicationTemplateNotFoundException ex)
        {
            return ApiResponse<object>.Fail([ex.Message], ApiStatusCodes.NotFound);
        }
        catch (TemplateRenderingException ex)
        {
            var errors = new List<string> { ex.Message };
            errors.AddRange(ex.MissingTokens);
            return ApiResponse<object>.Fail(errors, ApiStatusCodes.BadRequest);
        }
    }
}
