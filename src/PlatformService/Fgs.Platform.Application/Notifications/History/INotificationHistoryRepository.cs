using Fgs.Platform.Domain.Entities;
using Fgs.Platform.Domain.Notifications;

namespace Fgs.Platform.Application.Notifications.History;

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
