using Fgs.Notification.Application.Notifications.Queues;
using Fgs.Notification.Domain.Entities;
using Fgs.Notification.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Notification.Infrastructure.Notifications.Queues;

public sealed class IdempotencyStore(FgsNotificationDbContext context) : IIdempotencyStore
{
    public async Task<bool> TryMarkProcessedAsync(
        string messageId,
        string eventType,
        CancellationToken cancellationToken = default)
    {
        if (await context.ProcessedIntegrationEvents.AnyAsync(
                e => e.MessageId == messageId,
                cancellationToken))
        {
            return false;
        }

        await context.ProcessedIntegrationEvents.AddAsync(
            new FgsProcessedIntegrationEvent
            {
                Id = Guid.NewGuid(),
                MessageId = messageId,
                EventType = eventType,
                ProcessedOn = DateTimeOffset.UtcNow
            },
            cancellationToken);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }

    public async Task ReleaseAsync(
        string messageId,
        CancellationToken cancellationToken = default)
    {
        var existing = await context.ProcessedIntegrationEvents
            .FirstOrDefaultAsync(e => e.MessageId == messageId, cancellationToken);

        if (existing is null)
        {
            return;
        }

        context.ProcessedIntegrationEvents.Remove(existing);
        await context.SaveChangesAsync(cancellationToken);
    }
}
