using Fgs.Contracts.Requests;
using Fgs.Notification.Application.Notifications.Channels.Models;
using Fgs.Notification.Application.Notifications.Queues;
using Fgs.Notification.Domain.Notifications;

namespace Fgs.Notification.Application.Notifications.Dispatch;

public sealed class NotificationDispatchRequestResolver(IIntegrationEventMapper mapper)
    : INotificationDispatchRequestResolver
{
    private const string InvalidShapeMessage =
        "Provide either RoutingKey, Payload, and MessageId for event dispatch, "
        + "or TenantId, Channel, TemplateCode, and Recipient for explicit dispatch.";

    public NotificationDispatchResolveResult Resolve(DispatchNotificationRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.RoutingKey)
            && !string.IsNullOrWhiteSpace(request.Payload))
        {
            return ResolveEvent(request);
        }

        if (request.TenantId is > 0
            && !string.IsNullOrWhiteSpace(request.Channel)
            && !string.IsNullOrWhiteSpace(request.TemplateCode)
            && !string.IsNullOrWhiteSpace(request.Recipient))
        {
            return ResolveExplicit(request);
        }

        return NotificationDispatchResolveResult.Fail(InvalidShapeMessage);
    }

    private NotificationDispatchResolveResult ResolveEvent(DispatchNotificationRequest request)
    {
        var routingKey = request.RoutingKey!;
        var payload = request.Payload!;

        if (string.IsNullOrWhiteSpace(request.MessageId))
        {
            return NotificationDispatchResolveResult.Fail(
                "RoutingKey, Payload, and MessageId are required for event dispatch.");
        }

        if (!mapper.CanMap(routingKey))
        {
            return NotificationDispatchResolveResult.NoContent();
        }

        var dispatchRequest = mapper.Map(
            routingKey,
            payload,
            request.CorrelationId,
            request.MessageId);

        if (dispatchRequest is null)
        {
            return NotificationDispatchResolveResult.NoContent();
        }

        return NotificationDispatchResolveResult.Success(
            dispatchRequest,
            requiresIdempotency: true,
            messageId: request.MessageId,
            idempotencyKey: routingKey);
    }

    private static NotificationDispatchResolveResult ResolveExplicit(DispatchNotificationRequest request)
    {
        if (!Enum.TryParse<NotificationChannel>(request.Channel, ignoreCase: true, out var channel))
        {
            return NotificationDispatchResolveResult.Fail(
                $"Unsupported notification channel '{request.Channel}'.");
        }

        var dispatchRequest = new NotificationDispatchRequest(
            request.TenantId!.Value,
            request.CompanyId,
            channel,
            request.TemplateCode!,
            request.Recipient!,
            request.Tokens ?? new Dictionary<string, string>(),
            request.CorrelationId,
            request.MessageId);

        var requiresIdempotency = !string.IsNullOrWhiteSpace(request.MessageId);
        var idempotencyKey = requiresIdempotency
            ? FirstNonEmpty(request.RoutingKey, request.TemplateCode)
            : null;

        return NotificationDispatchResolveResult.Success(
            dispatchRequest,
            requiresIdempotency,
            request.MessageId,
            idempotencyKey);
    }

    private static string? FirstNonEmpty(params string?[] values)
    {
        foreach (var value in values)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }
        }

        return null;
    }
}
