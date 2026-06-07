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
    public void TryMap_RabbitMqUsername_MapsToUserName()
    {
        CredentialSectionMapper.TryMap("Global:RABBITMQ:Username", out var key, out _).Should().BeTrue();
        key.Should().Be("RabbitMq:UserName");
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
