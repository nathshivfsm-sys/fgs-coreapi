using Fgs.Messaging.Abstractions;
using Fgs.Messaging.Models;
using Fgs.User.Domain.Enums;
using Fgs.User.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.User.Infrastructure.Outbox;

public sealed class TenantOutboxStore(FgsUserDbContext context) : IOutboxStore
{
    public async Task<IReadOnlyList<PendingOutboxMessage>> ClaimPendingBatchAsync(
        int batchSize,
        CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;

        var messages = await context.TenantOutboxMessages
            .Where(m => (m.Status == OutboxMessageStatus.Pending || m.Status == OutboxMessageStatus.Retry)
                && (m.NextRetryOn == null || m.NextRetryOn <= now)
                && m.RetryCount < m.MaxRetryCount)
            .OrderBy(m => m.CreatedOn)
            .Take(batchSize)
            .ToListAsync(cancellationToken);

        foreach (var message in messages)
        {
            message.Status = OutboxMessageStatus.Processing;
        }

        if (messages.Count > 0)
        {
            await context.SaveChangesAsync(cancellationToken);
        }

        return messages
            .Select(m => new PendingOutboxMessage(
                m.Id,
                m.EventType,
                m.Payload,
                m.CorrelationId,
                m.ExchangeName,
                m.RoutingKey,
                m.RetryCount,
                m.MaxRetryCount))
            .ToList();
    }

    public async Task MarkPublishedAsync(
        long messageId,
        DateTimeOffset processedOn,
        CancellationToken cancellationToken)
    {
        var message = await context.TenantOutboxMessages
            .FirstAsync(m => m.Id == messageId, cancellationToken);

        message.Status = OutboxMessageStatus.Published;
        message.ProcessedOn = processedOn;
        message.LastError = null;
        message.NextRetryOn = null;

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task MarkRetryOrFailedAsync(
        long messageId,
        int retryCount,
        string lastError,
        bool isFailed,
        DateTimeOffset? nextRetryOn,
        CancellationToken cancellationToken)
    {
        var message = await context.TenantOutboxMessages
            .FirstAsync(m => m.Id == messageId, cancellationToken);

        message.RetryCount = retryCount;
        message.Status = isFailed ? OutboxMessageStatus.Failed : OutboxMessageStatus.Retry;
        message.LastError = lastError;
        message.NextRetryOn = nextRetryOn;

        await context.SaveChangesAsync(cancellationToken);
    }
}
