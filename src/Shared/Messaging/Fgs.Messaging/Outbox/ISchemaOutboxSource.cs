using Fgs.Messaging.Models;

namespace Fgs.Messaging.Outbox;

public sealed record ClaimedOutboxRow(PendingOutboxMessage Message, DateTimeOffset CreatedOn);

public interface ISchemaOutboxSource
{
    string SourceKey { get; }

    Task<IReadOnlyList<ClaimedOutboxRow>> ClaimPendingBatchAsync(
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
