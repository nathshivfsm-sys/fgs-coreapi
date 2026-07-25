using Fgs.Credentials;
using Fgs.Credentials.Options;
using Fgs.Credentials.Redis;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using CredentialConsumerOptions = Fgs.Credentials.Options.CredentialConsumerOptions;

namespace Fgs.Credentials.Tests;

public sealed class CredentialSnapshotApplierTests
{
    [Fact]
    public void Apply_FiltersToRequiredProvidersAndNotifies()
    {
        var holder = new CredentialConfigurationHolder();
        var notifier = new CredentialOptionsChangeNotifier();
        var notified = false;
        using var registration = notifier.GetChangeToken().RegisterChangeCallback(_ => notified = true, null);

        var count = CredentialSnapshotApplier.Apply(
            holder,
            notifier,
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["Global:DATABASE:FgsUser"] = "Host=db",
                ["Global:SENDGRID:ApiKey"] = "sg-key",
                ["Global:REDIS:ConnectionString"] = "redis:6379"
            },
            ["DATABASE", "REDIS"]);

        count.Should().Be(2);
        holder.Values.Should().ContainKey("Global:DATABASE:FgsUser");
        holder.Values.Should().ContainKey("Global:REDIS:ConnectionString");
        holder.Values.Should().NotContainKey("Global:SENDGRID:ApiKey");
        notified.Should().BeTrue();
    }

    [Fact]
    public void Apply_AlwaysRetainsRedisKeysEvenWhenNotRequired()
    {
        var holder = new CredentialConfigurationHolder();
        var notifier = new CredentialOptionsChangeNotifier();

        var count = CredentialSnapshotApplier.Apply(
            holder,
            notifier,
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["Global:DATABASE:FgsUser"] = "Host=db",
                ["Global:REDIS:ConnectionString"] = "redis:6379",
                ["Global:SENDGRID:ApiKey"] = "sg-key"
            },
            ["DATABASE"]);

        count.Should().Be(2);
        holder.Values.Should().ContainKey("Global:DATABASE:FgsUser");
        holder.Values.Should().ContainKey("Global:REDIS:ConnectionString");
        holder.Values.Should().NotContainKey("Global:SENDGRID:ApiKey");
    }
}

public sealed class CredentialSnapshotRedisCacheTests
{
    [Fact]
    public void ResolveRedisConnectionString_ReturnsGlobalRedisValue()
    {
        var connectionString = CredentialSnapshotRedisCache.ResolveRedisConnectionString(
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["Global:REDIS:ConnectionString"] = "redis:6379,password=secret"
            });

        connectionString.Should().Be("redis:6379,password=secret");
    }

    [Fact]
    public void ResolveRedisConnectionString_WhenMissing_ReturnsNull()
    {
        CredentialSnapshotRedisCache.ResolveRedisConnectionString(
                new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase))
            .Should()
            .BeNull();
    }

    [Fact]
    public async Task PublishAsync_WhenRedisMissing_DoesNotThrow()
    {
        var holder = new CredentialConfigurationHolder();
        var cache = new CredentialSnapshotRedisCache(
            holder,
            NullLogger<CredentialSnapshotRedisCache>.Instance);

        await cache.Invoking(c => c.PublishAsync(new Dictionary<string, string>
            {
                ["Global:DATABASE:FgsUser"] = "Host=db"
            }))
            .Should()
            .NotThrowAsync();
    }
}

public sealed class CredentialSnapshotReloadHostedServiceTests
{
    [Fact]
    public async Task ReloadFromRedis_AppliesFilteredSnapshot()
    {
        var holder = new CredentialConfigurationHolder();
        holder.ReplaceValues(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["Global:REDIS:ConnectionString"] = "redis:6379"
        });

        var snapshotCache = new Mock<ICredentialSnapshotRedisCache>();
        snapshotCache
            .Setup(c => c.GetAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["Global:DATABASE:FgsUser"] = "Host=db",
                ["Global:SENDGRID:ApiKey"] = "sg-key",
                ["Global:REDIS:ConnectionString"] = "redis:6379"
            });

        Func<CancellationToken, Task>? onChanged = null;
        var subscribed = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        snapshotCache
            .Setup(c => c.SubscribeAsync(It.IsAny<Func<CancellationToken, Task>>(), It.IsAny<CancellationToken>()))
            .Returns<Func<CancellationToken, Task>, CancellationToken>(async (callback, ct) =>
            {
                onChanged = callback;
                subscribed.TrySetResult();
                try
                {
                    await Task.Delay(Timeout.Infinite, ct);
                }
                catch (OperationCanceledException)
                {
                    // Host stopped.
                }
            });

        var notifier = new CredentialOptionsChangeNotifier();
        using var cts = new CancellationTokenSource();
        var service = new CredentialSnapshotReloadHostedService(
            snapshotCache.Object,
            holder,
            notifier,
            Microsoft.Extensions.Options.Options.Create(new CredentialConsumerOptions
            {
                ServiceName = "fgs-user-service",
                RequiredProviders = ["DATABASE", "REDIS"]
            }),
            NullLogger<CredentialSnapshotReloadHostedService>.Instance);

        await service.StartAsync(cts.Token);
        await subscribed.Task.WaitAsync(TimeSpan.FromSeconds(5));
        onChanged.Should().NotBeNull();
        await onChanged!(CancellationToken.None);

        holder.Values.Should().ContainKey("Global:DATABASE:FgsUser");
        holder.Values.Should().ContainKey("Global:REDIS:ConnectionString");
        holder.Values.Should().NotContainKey("Global:SENDGRID:ApiKey");

        await cts.CancelAsync();
        await service.StopAsync(CancellationToken.None);
    }
}
