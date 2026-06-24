using Fgs.Credentials.Configuration;
using Fgs.Foundation.Caching.Options;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fgs.Credentials.Tests;

public sealed class CredentialApplicationConfigurationProviderTests
{
    [Fact]
    public void Configuration_BindsRedisOptionsFromMappedCredentialKeys()
    {
        var holder = new CredentialConfigurationHolder();
        holder.ReplaceValues(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["Global:REDIS:ConnectionString"] = "redis:6379",
            ["Global:REDIS:Enabled"] = "True",
            ["Global:REDIS:InstanceName"] = "fgs:",
            ["Global:REDIS:DefaultAbsoluteExpirationMinutes"] = "30"
        });

        var configuration = new ConfigurationBuilder()
            .Add(new CredentialApplicationConfigurationSource(holder))
            .Build();

        var options = configuration.GetSection(RedisCacheOptions.SectionName).Get<RedisCacheOptions>();

        options.Should().NotBeNull();
        options!.ConnectionString.Should().Be("redis:6379");
        options.Enabled.Should().BeTrue();
        options.InstanceName.Should().Be("fgs:");
        options.DefaultAbsoluteExpirationMinutes.Should().Be(30);
    }

    [Fact]
    public void Configure_BindsRedisOptionsThroughOptionsPattern()
    {
        var holder = new CredentialConfigurationHolder();
        holder.ReplaceValues(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["Global:REDIS:ConnectionString"] = "redis:6379",
            ["Global:REDIS:Enabled"] = "True"
        });

        var configuration = new ConfigurationBuilder()
            .Add(new CredentialApplicationConfigurationSource(holder))
            .Build();

        var services = new ServiceCollection();
        services.Configure<RedisCacheOptions>(configuration.GetSection(RedisCacheOptions.SectionName));

        using var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<RedisCacheOptions>>().Value;

        options.ConnectionString.Should().Be("redis:6379");
        options.Enabled.Should().BeTrue();
    }
}
