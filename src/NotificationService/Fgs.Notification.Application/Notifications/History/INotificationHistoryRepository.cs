using Fgs.Notification.Domain.Entities;
using Fgs.Notification.Domain.Enums;

namespace Fgs.Notification.Application.Notifications.History;

public interface INotificationHistoryRepository
{
    Task<long> AddEmailAsync(FgsEmailHistory entry, CancellationToken cancellationToken = default);

    Task UpdateEmailStatusAsync(
        long id,
        NotificationStatus status,
        string? providerMessageId,
        string? providerName,
        string? failureReason,
        DateTimeOffset? sentOn,
        DateTimeOffset? failedOn,
        CancellationToken cancellationToken = default);

    Task<long> AddSmsAsync(FgsSmsHistory entry, CancellationToken cancellationToken = default);

    Task UpdateSmsStatusAsync(
        long id,
        NotificationStatus status,
        string? providerMessageId,
        string? providerName,
        string? failureReason,
        DateTimeOffset? sentOn,
        DateTimeOffset? failedOn,
        CancellationToken cancellationToken = default);
}
