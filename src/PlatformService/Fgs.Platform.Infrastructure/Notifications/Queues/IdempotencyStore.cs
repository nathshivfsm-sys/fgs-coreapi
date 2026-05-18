using Fgs.Platform.Application.Notifications.Queues;
using Fgs.Platform.Domain.Entities;
using Fgs.Platform.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Platform.Infrastructure.Notifications.Queues;

public sealed class IdempotencyStore(FgsPlatformDbContext context) : IIdempotencyStore
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
}
