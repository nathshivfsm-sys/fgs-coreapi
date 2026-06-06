using Fgs.Contracts.Api;
using Fgs.Contracts.Requests;
using Fgs.Notification.Application.Notifications.Channels;
using Fgs.Notification.Application.Notifications.Channels.Models;
using Fgs.Notification.Application.Notifications.Queues;
using Fgs.Notification.Domain.Notifications;
using MediatR;

namespace Fgs.Notification.Application.Features.Notifications.Commands.DispatchNotification;

public sealed class DispatchNotificationCommandHandler(
    IIntegrationEventMapper mapper,
    IIdempotencyStore idempotency,
    INotificationDispatcher dispatcher)
    : IRequestHandler<DispatchNotificationCommand, ApiResponse<object>>
{
    public async Task<ApiResponse<object>> Handle(
        DispatchNotificationCommand command,
        CancellationToken cancellationToken)
    {
        return command.Request.Source switch
        {
            NotificationDispatchSource.IntegrationEvent => await HandleIntegrationEventAsync(
                command.Request,
                cancellationToken),
            NotificationDispatchSource.Direct => await HandleDirectAsync(
                command.Request,
                cancellationToken),
            _ => ApiResponse<object>.Fail(
                ["Unsupported notification dispatch source."],
                ApiStatusCodes.BadRequest)
        };
    }

    private async Task<ApiResponse<object>> HandleIntegrationEventAsync(
        DispatchNotificationRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.RoutingKey)
            || string.IsNullOrWhiteSpace(request.Payload)
            || string.IsNullOrWhiteSpace(request.MessageId))
        {
            return ApiResponse<object>.Fail(
                ["RoutingKey, Payload, and MessageId are required for integration event dispatch."],
                ApiStatusCodes.BadRequest);
        }

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
            request.Payload,
            request.CorrelationId,
            request.MessageId);

        if (dispatchRequest is null)
        {
            return ApiResponse<object>.Ok(new object(), ApiStatusCodes.NoContent);
        }

        return await DispatchAsync(dispatchRequest, cancellationToken);
    }

    private async Task<ApiResponse<object>> HandleDirectAsync(
        DispatchNotificationRequest request,
        CancellationToken cancellationToken)
    {
        if (request.TenantId is null or <= 0
            || string.IsNullOrWhiteSpace(request.Channel)
            || string.IsNullOrWhiteSpace(request.TemplateCode)
            || string.IsNullOrWhiteSpace(request.Recipient))
        {
            return ApiResponse<object>.Fail(
                ["TenantId, Channel, TemplateCode, and Recipient are required for direct dispatch."],
                ApiStatusCodes.BadRequest);
        }

        if (!Enum.TryParse<NotificationChannel>(request.Channel, ignoreCase: true, out var channel))
        {
            return ApiResponse<object>.Fail(
                [$"Unsupported notification channel '{request.Channel}'."],
                ApiStatusCodes.BadRequest);
        }

        var dispatchRequest = new NotificationDispatchRequest(
            request.TenantId.Value,
            request.CompanyId,
            channel,
            request.TemplateCode,
            request.Recipient,
            request.Tokens ?? new Dictionary<string, string>(),
            request.CorrelationId,
            request.MessageId);

        if (!string.IsNullOrWhiteSpace(request.MessageId)
            && !string.IsNullOrWhiteSpace(request.RoutingKey)
            && !await idempotency.TryMarkProcessedAsync(
                request.MessageId,
                request.RoutingKey,
                cancellationToken))
        {
            return ApiResponse<object>.Ok(new object(), ApiStatusCodes.NoContent);
        }

        return await DispatchAsync(dispatchRequest, cancellationToken);
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

        return ApiResponse<object>.Ok(new object(), ApiStatusCodes.NoContent);
    }
}
