using Fgs.Notification.Application.Notifications.History;
using Fgs.Notification.Domain.Entities;
using Fgs.Notification.Domain.Enums;
using Fgs.Notification.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Notification.Infrastructure.Notifications.History;

public sealed class NotificationHistoryRepository(FgsNotificationDbContext context) : INotificationHistoryRepository
{
    public async Task<long> AddEmailAsync(FgsEmailHistory entry, CancellationToken cancellationToken = default)
    {
        await context.FgsEmailHistories.AddAsync(entry, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return entry.Id;
    }

    public async Task UpdateEmailStatusAsync(
        long id,
        NotificationStatus status,
        string? providerMessageId,
        string? providerName,
        string? failureReason,
        DateTimeOffset? sentOn,
        DateTimeOffset? failedOn,
        CancellationToken cancellationToken = default)
    {
        var entry = await context.FgsEmailHistories
            .FirstOrDefaultAsync(h => h.Id == id, cancellationToken);

        if (entry is null)
        {
            return;
        }

        entry.Status = status;
        entry.ProviderMessageId = providerMessageId;
        entry.ProviderName = providerName ?? entry.ProviderName;
        entry.FailureReason = failureReason;
        entry.SentOn = sentOn;
        entry.FailedOn = failedOn;
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<long> AddSmsAsync(FgsSmsHistory entry, CancellationToken cancellationToken = default)
    {
        await context.FgsSmsHistories.AddAsync(entry, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return entry.Id;
    }

    public async Task UpdateSmsStatusAsync(
        long id,
        NotificationStatus status,
        string? providerMessageId,
        string? providerName,
        string? failureReason,
        DateTimeOffset? sentOn,
        DateTimeOffset? failedOn,
        CancellationToken cancellationToken = default)
    {
        var entry = await context.FgsSmsHistories
            .FirstOrDefaultAsync(h => h.Id == id, cancellationToken);

        if (entry is null)
        {
            return;
        }

        entry.Status = status;
        entry.ProviderMessageId = providerMessageId;
        entry.ProviderName = providerName ?? entry.ProviderName;
        entry.FailureReason = failureReason;
        entry.SentOn = sentOn;
        entry.FailedOn = failedOn;
        await context.SaveChangesAsync(cancellationToken);
    }
}
