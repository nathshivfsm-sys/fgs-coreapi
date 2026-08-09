using Fgs.Publisher.Infrastructure.Options;

namespace Fgs.Publisher.Tests;

public sealed class OutboxSourceOptionsTests
{
    [Fact]
    public void Enabled_DefaultsToTrue()
    {
        new OutboxSourceOptions().Enabled.Should().BeTrue();
    }

    [Fact]
    public void GetEnabledSources_ExcludesDisabled()
    {
        var options = new OutboxSourcesOptions
        {
            Sources =
            [
                new OutboxSourceOptions
                {
                    SourceKey = "tenant",
                    ConnectionStringName = "FgsUser",
                    Schema = "tenant",
                    Table = "TenantOutboxMessage",
                    Enabled = true
                },
                new OutboxSourceOptions
                {
                    SourceKey = "crm",
                    ConnectionStringName = "FgsCrm",
                    Schema = "crm",
                    Table = "CrmOutboxMessage",
                    Enabled = false
                }
            ]
        };

        options.GetEnabledSources().Select(s => s.SourceKey).Should().Equal("tenant");
    }

    [Fact]
    public void ResolveConnectionStringName_FallsBackToConnectionStringName()
    {
        var source = new OutboxSourceOptions
        {
            ConnectionStringName = "FgsUser",
            OutboxConnectionStringName = null
        };

        source.ResolveConnectionStringName().Should().Be("FgsUser");
    }

    [Fact]
    public void ResolveConnectionStringName_PrefersOutboxConnectionStringName()
    {
        var source = new OutboxSourceOptions
        {
            ConnectionStringName = "FgsUser",
            OutboxConnectionStringName = "FgsUserOutbox"
        };

        source.ResolveConnectionStringName().Should().Be("FgsUserOutbox");
    }
}
