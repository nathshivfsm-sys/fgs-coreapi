using Fgs.Messaging.Abstractions;
using Fgs.Messaging.Models;

namespace Fgs.Publisher.Infrastructure.Outbox;

public sealed class CompositeOutboxStore : IOutboxStore
{
    private readonly IReadOnlyDictionary<string, ISchemaOutboxSource> _sources;

    public CompositeOutboxStore(IEnumerable<ISchemaOutboxSource> sources)
    {
        _sources = sources.ToDictionary(source => source.SourceKey, StringComparer.OrdinalIgnoreCase);
    }

    public async Task<IReadOnlyList<PendingOutboxMessage>> ClaimPendingBatchAsync(
        int batchSize,
        CancellationToken cancellationToken)
    {
        if (batchSize <= 0 || _sources.Count == 0)
        {
            return [];
        }

        var perSourceBatchSize = Math.Max(1, batchSize / _sources.Count);
        var claimed = new List<ClaimedOutboxRow>();

        foreach (var source in _sources.Values)
        {
            var batch = await source.ClaimPendingBatchAsync(perSourceBatchSize, cancellationToken);
            claimed.AddRange(batch);
        }

        return claimed
            .OrderBy(row => row.CreatedOn)
            .Take(batchSize)
            .Select(row => row.Message)
            .ToList();
    }

    public Task MarkPublishedAsync(
        string sourceKey,
        long messageId,
        DateTimeOffset processedOn,
        CancellationToken cancellationToken) =>
        Resolve(sourceKey).MarkPublishedAsync(messageId, processedOn, cancellationToken);

    public Task MarkRetryOrFailedAsync(
        string sourceKey,
        long messageId,
        int retryCount,
        string lastError,
        bool isFailed,
        DateTimeOffset? nextRetryOn,
        CancellationToken cancellationToken) =>
        Resolve(sourceKey).MarkRetryOrFailedAsync(
            messageId,
            retryCount,
            lastError,
            isFailed,
            nextRetryOn,
            cancellationToken);

    private ISchemaOutboxSource Resolve(string sourceKey) =>
        _sources.TryGetValue(sourceKey, out var source)
            ? source
            : throw new KeyNotFoundException($"Outbox source '{sourceKey}' is not configured.");
}
