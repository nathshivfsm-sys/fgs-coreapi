using System.Text.Json;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Fgs.Credentials.Redis;

public sealed class CredentialSnapshotRedisCache : ICredentialSnapshotRedisCache, IDisposable, IAsyncDisposable
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly CredentialConfigurationHolder _holder;
    private readonly ILogger<CredentialSnapshotRedisCache> _logger;
    private readonly SemaphoreSlim _connectLock = new(1, 1);
    private readonly SemaphoreSlim _reloadLock = new(1, 1);

    private IConnectionMultiplexer? _multiplexer;
    private string? _connectedConnectionString;

    public CredentialSnapshotRedisCache(
        CredentialConfigurationHolder holder,
        ILogger<CredentialSnapshotRedisCache> logger)
    {
        _holder = holder;
        _logger = logger;
    }

    public async Task PublishAsync(
        IReadOnlyDictionary<string, string> values,
        CancellationToken cancellationToken = default)
    {
        var db = await GetDatabaseAsync(cancellationToken);
        if (db is null)
        {
            _logger.LogWarning(
                "Skipping credential snapshot publish: Redis connection string is not available in loaded credentials.");
            return;
        }

        var payload = JsonSerializer.Serialize(values, JsonOptions);
        await db.StringSetAsync(CredentialSnapshotRedisKeys.Snapshot, payload);

        var subscriber = _multiplexer!.GetSubscriber();
        await subscriber.PublishAsync(
            RedisChannel.Literal(CredentialSnapshotRedisKeys.ChangedChannel),
            "1");

        _logger.LogInformation(
            "Published credential snapshot to Redis ({Count} entries).",
            values.Count);
    }

    public async Task<IReadOnlyDictionary<string, string>?> GetAsync(
        CancellationToken cancellationToken = default)
    {
        var db = await GetDatabaseAsync(cancellationToken);
        if (db is null)
        {
            return null;
        }

        var payload = await db.StringGetAsync(CredentialSnapshotRedisKeys.Snapshot);
        if (payload.IsNullOrEmpty)
        {
            return null;
        }

        return JsonSerializer.Deserialize<Dictionary<string, string>>((string)payload!, JsonOptions);
    }

    public async Task SubscribeAsync(
        Func<CancellationToken, Task> onChanged,
        CancellationToken cancellationToken = default)
    {
        var multiplexer = await GetMultiplexerAsync(cancellationToken);
        if (multiplexer is null)
        {
            _logger.LogWarning(
                "Credential snapshot Redis subscription not started: Redis connection string is not available.");
            return;
        }

        var subscriber = multiplexer.GetSubscriber();
        await subscriber.SubscribeAsync(
            RedisChannel.Literal(CredentialSnapshotRedisKeys.ChangedChannel),
            (_, _) =>
            {
                _ = HandleChangedAsync(onChanged, cancellationToken);
            });

        _logger.LogInformation("Subscribed to credential snapshot Redis change channel.");

        try
        {
            await Task.Delay(Timeout.Infinite, cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            // Host is stopping.
        }
    }

    public void Dispose()
    {
        _multiplexer?.Dispose();
        _multiplexer = null;
        _connectLock.Dispose();
        _reloadLock.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (_multiplexer is not null)
        {
            await _multiplexer.DisposeAsync();
            _multiplexer = null;
        }

        _connectLock.Dispose();
        _reloadLock.Dispose();
    }

    private async Task HandleChangedAsync(
        Func<CancellationToken, Task> onChanged,
        CancellationToken cancellationToken)
    {
        if (!await _reloadLock.WaitAsync(0, cancellationToken))
        {
            return;
        }

        try
        {
            await onChanged(cancellationToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, "Failed to reload credentials from Redis snapshot.");
        }
        finally
        {
            _reloadLock.Release();
        }
    }

    private async Task<IDatabase?> GetDatabaseAsync(CancellationToken cancellationToken)
    {
        var multiplexer = await GetMultiplexerAsync(cancellationToken);
        return multiplexer?.GetDatabase();
    }

    private async Task<IConnectionMultiplexer?> GetMultiplexerAsync(CancellationToken cancellationToken)
    {
        var connectionString = ResolveRedisConnectionString(_holder.Values);
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return null;
        }

        var normalized = NormalizeConnectionString(connectionString);
        if (_multiplexer is { IsConnected: true }
            && string.Equals(_connectedConnectionString, normalized, StringComparison.Ordinal))
        {
            return _multiplexer;
        }

        await _connectLock.WaitAsync(cancellationToken);
        try
        {
            connectionString = ResolveRedisConnectionString(_holder.Values);
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                return null;
            }

            normalized = NormalizeConnectionString(connectionString);
            if (_multiplexer is { IsConnected: true }
                && string.Equals(_connectedConnectionString, normalized, StringComparison.Ordinal))
            {
                return _multiplexer;
            }

            if (_multiplexer is not null)
            {
                await _multiplexer.DisposeAsync();
                _multiplexer = null;
            }

            _multiplexer = await ConnectionMultiplexer.ConnectAsync(normalized);
            _connectedConnectionString = normalized;
            return _multiplexer;
        }
        finally
        {
            _connectLock.Release();
        }
    }

    internal static string? ResolveRedisConnectionString(IReadOnlyDictionary<string, string> values)
    {
        if (values.TryGetValue("Global:REDIS:ConnectionString", out var connectionString)
            && !string.IsNullOrWhiteSpace(connectionString))
        {
            return connectionString;
        }

        return null;
    }

    private static string NormalizeConnectionString(string connectionString)
    {
        if (connectionString.Contains("abortConnect=", StringComparison.OrdinalIgnoreCase))
        {
            return connectionString;
        }

        return $"{connectionString.TrimEnd(',')},abortConnect=false";
    }
}
