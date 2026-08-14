using Fgs.Credentials.Mapping;
using FluentAssertions;

namespace Fgs.Credentials.Tests;

public sealed class CredentialSectionMapperTests
{
    [Fact]
    public void TryMap_DatabaseKey_MapsToConnectionString()
    {
        CredentialSectionMapper.TryMap("Global:DATABASE:FgsUser", out var key, out _).Should().BeTrue();
        key.Should().Be("ConnectionStrings:FgsUser");
    }

    [Fact]
    public void TryResolveValue_DatabaseKey_ReturnsConnectionString()
    {
        var values = new Dictionary<string, string>
        {
            ["Global:DATABASE:FgsUser"] = "Host=localhost;Database=fgs"
        };

        CredentialSectionMapper.TryResolveValue(
                "Global:DATABASE:FgsUser",
                "ConnectionStrings:FgsUser",
                values,
                out var resolved)
            .Should().BeTrue();

        resolved.Should().Be("Host=localhost;Database=fgs");
    }

    [Fact]
    public void TryMap_DatabaseReadOnlyKeys_MapsToConnectionString()
    {
        CredentialSectionMapper.TryMap("Global:DATABASE:FgsUserReadOnly", out var userKey, out _).Should().BeTrue();
        userKey.Should().Be("ConnectionStrings:FgsUserReadOnly");

        CredentialSectionMapper.TryMap("Global:DATABASE:FgsSetupReadOnly", out var setupKey, out _).Should().BeTrue();
        setupKey.Should().Be("ConnectionStrings:FgsSetupReadOnly");

        CredentialSectionMapper.TryMap("Global:DATABASE:FgsAssetReadOnly", out var assetKey, out _).Should().BeTrue();
        assetKey.Should().Be("ConnectionStrings:FgsAssetReadOnly");
    }

    [Fact]
    public void TryResolveValue_DatabaseReadOnlyKeys_ReturnConnectionString()
    {
        var values = new Dictionary<string, string>
        {
            ["Global:DATABASE:FgsUserReadOnly"] = "Host=localhost;Database=fgs_user_ro",
            ["Global:DATABASE:FgsSetupReadOnly"] = "Host=localhost;Database=fgs_setup_ro",
            ["Global:DATABASE:FgsAssetReadOnly"] = "Host=localhost;Database=fgs_asset_ro"
        };

        CredentialSectionMapper.TryResolveValue(
                "Global:DATABASE:FgsUserReadOnly",
                "ConnectionStrings:FgsUserReadOnly",
                values,
                out var userResolved)
            .Should().BeTrue();
        userResolved.Should().Be("Host=localhost;Database=fgs_user_ro");

        CredentialSectionMapper.TryResolveValue(
                "Global:DATABASE:FgsSetupReadOnly",
                "ConnectionStrings:FgsSetupReadOnly",
                values,
                out var setupResolved)
            .Should().BeTrue();
        setupResolved.Should().Be("Host=localhost;Database=fgs_setup_ro");

        CredentialSectionMapper.TryResolveValue(
                "Global:DATABASE:FgsAssetReadOnly",
                "ConnectionStrings:FgsAssetReadOnly",
                values,
                out var assetResolved)
            .Should().BeTrue();
        assetResolved.Should().Be("Host=localhost;Database=fgs_asset_ro");
    }

    [Fact]
    public void TryMap_RabbitMqKey_MapsToRabbitMqUserName()
    {
        CredentialSectionMapper.TryMap("Global:RABBITMQ:Username", out var key, out _).Should().BeTrue();
        key.Should().Be("RabbitMq:UserName");
    }

    [Fact]
    public void TryMap_RedisKey_MapsToRedisConnectionString()
    {
        CredentialSectionMapper.TryMap("Global:REDIS:ConnectionString", out var key, out _).Should().BeTrue();
        key.Should().Be("Redis:ConnectionString");
    }

    [Fact]
    public void TryResolveValue_RedisKey_ReturnsConnectionString()
    {
        var values = new Dictionary<string, string>
        {
            ["Global:REDIS:ConnectionString"] = "redis:6379",
            ["Global:REDIS:Enabled"] = "True",
            ["Global:REDIS:InstanceName"] = "fgs:"
        };

        CredentialSectionMapper.TryResolveValue(
                "Global:REDIS:ConnectionString",
                "Redis:ConnectionString",
                values,
                out var resolved)
            .Should().BeTrue();

        resolved.Should().Be("redis:6379");
    }

    [Fact]
    public void TryMap_DatadogKey_MapsToDatadogApiKey()
    {
        CredentialSectionMapper.TryMap("Global:DATADOG:ApiKey", out var key, out _).Should().BeTrue();
        key.Should().Be("Datadog:ApiKey");
    }

    [Fact]
    public void TryResolveValue_DatadogKey_ReturnsApiKey()
    {
        var values = new Dictionary<string, string>
        {
            ["Global:DATADOG:ApiKey"] = "dd-api-key",
            ["Global:DATADOG:Site"] = "datadoghq.com"
        };

        CredentialSectionMapper.TryResolveValue(
                "Global:DATADOG:ApiKey",
                "Datadog:ApiKey",
                values,
                out var resolved)
            .Should().BeTrue();

        resolved.Should().Be("dd-api-key");
    }

    [Fact]
    public void Filter_KeepsOnlyRequiredProviders()
    {
        var values = new Dictionary<string, string>
        {
            ["Global:DATABASE:FgsUser"] = "db",
            ["Global:SENDGRID:ApiKey"] = "sg",
            ["Global:AWS:SecretAccessKey"] = "aws"
        };

        var filtered = CredentialConfigurationFilter.Filter(values, ["DATABASE", "SENDGRID"]);

        filtered.Should().HaveCount(2);
        filtered.Should().ContainKey("Global:DATABASE:FgsUser");
        filtered.Should().ContainKey("Global:SENDGRID:ApiKey");
    }
}
