namespace Fgs.Credentials.Redis;

public interface ICredentialSnapshotRedisCache
{
    /// <summary>
    /// Writes the Global credential snapshot and publishes a change notification (no secrets in the message).
    /// Returns <c>false</c> when Redis is unavailable and the snapshot was not published.
    /// </summary>
    Task<bool> PublishAsync(
        IReadOnlyDictionary<string, string> values,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyDictionary<string, string>?> GetAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Subscribes to change notifications until <paramref name="cancellationToken"/> is cancelled.
    /// Overlapping signals coalesce into a follow-up reload instead of being dropped.
    /// </summary>
    Task SubscribeAsync(
        Func<CancellationToken, Task> onChanged,
        CancellationToken cancellationToken = default);
}
