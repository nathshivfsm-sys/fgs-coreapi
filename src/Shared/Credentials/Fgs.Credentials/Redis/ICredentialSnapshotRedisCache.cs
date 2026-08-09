namespace Fgs.Credentials.Redis;

public interface ICredentialSnapshotRedisCache
{
    /// <summary>
    /// Writes the full credential snapshot and publishes a change notification (no secrets in the message).
    /// </summary>
    Task PublishAsync(
        IReadOnlyDictionary<string, string> values,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyDictionary<string, string>?> GetAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Subscribes to change notifications until <paramref name="cancellationToken"/> is cancelled.
    /// </summary>
    Task SubscribeAsync(
        Func<CancellationToken, Task> onChanged,
        CancellationToken cancellationToken = default);
}
