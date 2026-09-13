using Fgs.Messaging.Models;

namespace Fgs.Messaging.Abstractions;

public interface IOutboxStore
{
    Task<IReadOnlyList<PendingOutboxMessage>> ClaimPendingBatchAsync(
        int batchSize,
        CancellationToken cancellationToken);

    Task HeartbeatProcessingAsync(
        string sourceKey,
        long messageId,
        DateTimeOffset updatedOn,
        CancellationToken cancellationToken);

    Task MarkPublishedAsync(
        string sourceKey,
        long messageId,
        DateTimeOffset processedOn,
        CancellationToken cancellationToken);

    Task MarkRetryOrFailedAsync(
        string sourceKey,
        long messageId,
        int retryCount,
        string lastError,
        bool isFailed,
        DateTimeOffset? nextRetryOn,
        CancellationToken cancellationToken);
}
