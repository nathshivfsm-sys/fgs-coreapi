using Fgs.Contracts.Api;
using Fgs.Notification.Application.Notifications.Channels;
using Fgs.Notification.Application.Notifications.Channels.Models;
using Fgs.Notification.Application.Notifications.Queues;
using MediatR;

namespace Fgs.Notification.Application.Features.Notifications.Commands.ProcessNotificationQueueMessage;

public sealed class ProcessNotificationQueueMessageCommandHandler(
    IIntegrationEventMapper mapper,
    IIdempotencyStore idempotency,
    INotificationDispatcher dispatcher)
    : IRequestHandler<ProcessNotificationQueueMessageCommand, ApiResponse<object>>
{
    public async Task<ApiResponse<object>> Handle(
        ProcessNotificationQueueMessageCommand request,
        CancellationToken cancellationToken)
    {
        if (!mapper.CanMap(request.RoutingKey))
        {
            return ApiResponse<object>.Ok(new object(), ApiStatusCodes.NoContent);
        }

        if (!await idempotency.TryMarkProcessedAsync(
                request.MessageId,
                request.RoutingKey,
                cancellationToken))
        {
            return ApiResponse<object>.Ok(new object(), ApiStatusCodes.NoContent);
        }

        var dispatchRequest = mapper.Map(
            request.RoutingKey,
            request.Body,
            request.CorrelationId,
            request.MessageId);

        if (dispatchRequest is null)
        {
            return ApiResponse<object>.Ok(new object(), ApiStatusCodes.NoContent);
        }

        var result = await dispatcher.DispatchAsync(dispatchRequest, cancellationToken);
        if (!result.Success)
        {
            return ApiResponse<object>.Fail(
                [result.Error ?? "Notification dispatch failed."],
                ApiStatusCodes.InternalServerError);
        }

        return ApiResponse<object>.Ok(new object(), ApiStatusCodes.NoContent);
    }
}
