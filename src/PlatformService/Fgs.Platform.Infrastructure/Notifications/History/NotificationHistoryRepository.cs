using Fgs.Platform.Application.Notifications.History;
using Fgs.Platform.Domain.Entities;
using Fgs.Platform.Domain.Notifications;
using Fgs.Platform.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Platform.Infrastructure.Notifications.History;

public sealed class NotificationHistoryRepository(FgsPlatformDbContext context) : INotificationHistoryRepository
{
    public async Task AddAsync(FgsNotificationHistory entry, CancellationToken cancellationToken = default)
    {
        await context.NotificationHistory.AddAsync(entry, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateStatusAsync(
        Guid id,
        NotificationDeliveryStatus status,
        string? providerMessageId,
        string? error,
        DateTimeOffset? sentOn,
        CancellationToken cancellationToken = default)
    {
        var entry = await context.NotificationHistory
            .FirstOrDefaultAsync(h => h.Id == id, cancellationToken);

        if (entry is null)
        {
            return;
        }

        entry.Status = status;
        entry.ProviderMessageId = providerMessageId;
        entry.Error = error;
        entry.SentOn = sentOn;
        await context.SaveChangesAsync(cancellationToken);
    }
}
