using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Credentials.Options;
using Fgs.Credentials.Redis;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;

namespace Fgs.Credentials.Tests;

public sealed class RemoteCredentialConfigurationLoaderTests
{
    [Fact]
    public async Task LoadAsync_WhenRedisSnapshotAvailable_DoesNotCallSetup()
    {
        var holder = new CredentialConfigurationHolder();
        var setupClient = new Mock<ISetupClient>(MockBehavior.Strict);
        var snapshotCache = new Mock<ICredentialSnapshotRedisCache>();
        snapshotCache
            .Setup(c => c.GetAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["Global:DATABASE:FgsUser"] = "Host=db",
                ["Global:REDIS:ConnectionString"] = "redis:6379",
                ["Global:SENDGRID:ApiKey"] = "sg-key"
            });

        var loader = CreateLoader(setupClient.Object, snapshotCache.Object, holder, ["DATABASE", "REDIS"]);

        await loader.LoadAsync();

        holder.Values.Should().ContainKey("Global:DATABASE:FgsUser");
        holder.Values.Should().ContainKey("Global:REDIS:ConnectionString");
        holder.Values.Should().NotContainKey("Global:SENDGRID:ApiKey");
        setupClient.Verify(
            c => c.GetResolvedCredentialsAsync(
                It.IsAny<string>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task LoadAsync_WhenRedisEmpty_FallsBackToSetup()
    {
        var holder = new CredentialConfigurationHolder();
        var setupClient = new Mock<ISetupClient>();
        setupClient
            .Setup(c => c.GetResolvedCredentialsAsync(
                "service-key",
                "fgs-test-service",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<ResolvedCredentialConfigurationDto>.Ok(
                new ResolvedCredentialConfigurationDto(
                    new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        ["Global:DATABASE:FgsUser"] = "Host=from-setup",
                        ["Global:REDIS:ConnectionString"] = "redis:6379"
                    })));

        var snapshotCache = new Mock<ICredentialSnapshotRedisCache>();
        snapshotCache
            .Setup(c => c.GetAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((IReadOnlyDictionary<string, string>?)null);

        var loader = CreateLoader(setupClient.Object, snapshotCache.Object, holder, ["DATABASE"]);

        await loader.LoadAsync();

        holder.Values.Should().ContainKey("Global:DATABASE:FgsUser");
        holder.Values["Global:DATABASE:FgsUser"].Should().Be("Host=from-setup");
        setupClient.Verify(
            c => c.GetResolvedCredentialsAsync(
                "service-key",
                "fgs-test-service",
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task LoadAsync_WhenRedisThrows_FallsBackToSetup()
    {
        var holder = new CredentialConfigurationHolder();
        var setupClient = new Mock<ISetupClient>();
        setupClient
            .Setup(c => c.GetResolvedCredentialsAsync(
                It.IsAny<string>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<ResolvedCredentialConfigurationDto>.Ok(
                new ResolvedCredentialConfigurationDto(
                    new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        ["Global:DATABASE:FgsUser"] = "Host=from-setup",
                        ["Global:REDIS:ConnectionString"] = "redis:6379"
                    })));

        var snapshotCache = new Mock<ICredentialSnapshotRedisCache>();
        snapshotCache
            .Setup(c => c.GetAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("redis down"));

        var loader = CreateLoader(setupClient.Object, snapshotCache.Object, holder, ["DATABASE"]);

        await loader.LoadAsync();

        holder.Values["Global:DATABASE:FgsUser"].Should().Be("Host=from-setup");
        setupClient.Verify(
            c => c.GetResolvedCredentialsAsync(
                It.IsAny<string>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private static RemoteCredentialConfigurationLoader CreateLoader(
        ISetupClient setupClient,
        ICredentialSnapshotRedisCache snapshotCache,
        CredentialConfigurationHolder holder,
        string[] requiredProviders) =>
        new(
            setupClient,
            snapshotCache,
            holder,
            new CredentialOptionsChangeNotifier(),
            Microsoft.Extensions.Options.Options.Create(new CredentialDistributionOptions
            {
                InternalServiceKey = "service-key"
            }),
            Microsoft.Extensions.Options.Options.Create(new CredentialConsumerOptions
            {
                ServiceName = "fgs-test-service",
                RequiredProviders = requiredProviders
            }),
            NullLogger<RemoteCredentialConfigurationLoader>.Instance);
}
