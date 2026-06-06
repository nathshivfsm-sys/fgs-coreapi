using Fgs.Notification.Application.Notifications.Channels.Models;

namespace Fgs.Notification.Application.Notifications.Dispatch;

public sealed class NotificationDispatchResolveResult
{
    public NotificationDispatchRequest? DispatchRequest { get; init; }

    public bool IsNoContent { get; init; }

    public IReadOnlyList<string> Errors { get; init; } = [];

    public bool RequiresIdempotency { get; init; }

    public string? MessageId { get; init; }

    public string? IdempotencyKey { get; init; }

    public bool IsFailure => Errors.Count > 0;

    public static NotificationDispatchResolveResult NoContent() => new() { IsNoContent = true };

    public static NotificationDispatchResolveResult Fail(params string[] errors) => new() { Errors = errors };

    public static NotificationDispatchResolveResult Success(
        NotificationDispatchRequest dispatchRequest,
        bool requiresIdempotency,
        string? messageId,
        string? idempotencyKey) =>
        new()
        {
            DispatchRequest = dispatchRequest,
            RequiresIdempotency = requiresIdempotency,
            MessageId = messageId,
            IdempotencyKey = idempotencyKey
        };
}
