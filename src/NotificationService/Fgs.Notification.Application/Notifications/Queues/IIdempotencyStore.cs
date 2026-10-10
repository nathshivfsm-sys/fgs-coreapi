namespace Fgs.Notification.Application.Notifications.Queues;

public interface IIdempotencyStore
{
    Task<bool> TryMarkProcessedAsync(
        string messageId,
        string eventType,
        CancellationToken cancellationToken = default);

    Task ReleaseAsync(
        string messageId,
        CancellationToken cancellationToken = default);
}
