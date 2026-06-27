using Fgs.Notification.Domain.Entities;
using Fgs.Notification.Domain.Notifications;

namespace Fgs.Notification.Application.Notifications.History;

public interface INotificationHistoryRepository
{
    Task AddAsync(FgsNotificationHistory entry, CancellationToken cancellationToken = default);

    Task UpdateStatusAsync(
        Guid id,
        NotificationDeliveryStatus status,
        string? providerMessageId,
        string? error,
        DateTimeOffset? sentOn,
        CancellationToken cancellationToken = default);
}
