using Fgs.Messaging.Abstractions;
using Fgs.Messaging.Models;
using Microsoft.Extensions.Logging;

namespace Fgs.Messaging.Outbox;

public sealed class CompositeOutboxStore : IOutboxStore
{
    private readonly IReadOnlyDictionary<string, ISchemaOutboxSource> _sources;
    private readonly ILogger<CompositeOutboxStore> _logger;

    public CompositeOutboxStore(
        IEnumerable<ISchemaOutboxSource> sources,
        ILogger<CompositeOutboxStore> logger)
    {
        _sources = sources.ToDictionary(source => source.SourceKey, StringComparer.OrdinalIgnoreCase);
        _logger = logger;
    }

    public async Task<IReadOnlyList<PendingOutboxMessage>> ClaimPendingBatchAsync(
        int batchSize,
        CancellationToken cancellationToken)
    {
        if (batchSize <= 0 || _sources.Count == 0)
        {
            return [];
        }

        // Fair first pass, then fill remaining budget only from sources that still had more rows.
        // Never claim more than batchSize total and never discard a claimed row
        // (surplus would stay Processing until stale reclaim).
        var perSourceBatchSize = Math.Max(1, batchSize / _sources.Count);
        var claimed = new List<ClaimedOutboxRow>();
        var remaining = batchSize;
        var sourcesWithMore = new List<ISchemaOutboxSource>();

        foreach (var source in _sources.Values)
        {
            if (remaining <= 0)
            {
                break;
            }

            var take = Math.Min(perSourceBatchSize, remaining);
            var before = claimed.Count;
            remaining = await ClaimFromSourceAsync(source, take, claimed, remaining, cancellationToken);
            var taken = claimed.Count - before;
            if (taken >= take)
            {
                sourcesWithMore.Add(source);
            }
        }

        foreach (var source in sourcesWithMore)
        {
            if (remaining <= 0)
            {
                break;
            }

            remaining = await ClaimFromSourceAsync(source, remaining, claimed, remaining, cancellationToken);
        }

        return claimed
            .OrderBy(row => row.CreatedOn)
            .Select(row => row.Message)
            .ToList();
    }

    public Task HeartbeatProcessingAsync(
        string sourceKey,
        long messageId,
        DateTimeOffset updatedOn,
        CancellationToken cancellationToken) =>
        Resolve(sourceKey).HeartbeatProcessingAsync(messageId, updatedOn, cancellationToken);

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

    private async Task<int> ClaimFromSourceAsync(
        ISchemaOutboxSource source,
        int take,
        List<ClaimedOutboxRow> claimed,
        int remaining,
        CancellationToken cancellationToken)
    {
        try
        {
            var batch = await source.ClaimPendingBatchAsync(take, cancellationToken);
            claimed.AddRange(batch);
            return remaining - batch.Count;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(
                ex,
                "Skipping outbox source {SourceKey} during claim batch; other sources will continue",
                source.SourceKey);
            return remaining;
        }
    }

    private ISchemaOutboxSource Resolve(string sourceKey) =>
        _sources.TryGetValue(sourceKey, out var source)
            ? source
            : throw new KeyNotFoundException($"Outbox source '{sourceKey}' is not configured.");
}
