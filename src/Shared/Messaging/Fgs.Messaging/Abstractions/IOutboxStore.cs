using Fgs.Messaging.Models;

namespace Fgs.Messaging.Abstractions;

public interface IOutboxStore
{
    Task<IReadOnlyList<PendingOutboxMessage>> ClaimPendingBatchAsync(
        int batchSize,
        CancellationToken cancellationToken);

    Task MarkPublishedAsync(
        long messageId,
        DateTimeOffset processedOn,
        CancellationToken cancellationToken);

    Task MarkRetryOrFailedAsync(
        long messageId,
        int retryCount,
        string lastError,
        bool isFailed,
        DateTimeOffset? nextRetryOn,
        CancellationToken cancellationToken);
}
