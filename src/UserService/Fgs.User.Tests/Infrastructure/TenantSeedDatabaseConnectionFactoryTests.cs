using Fgs.User.Infrastructure.Common.Options;
using Fgs.Security.Options;
using Fgs.User.Infrastructure.Provisioning;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Fgs.User.Tests.Infrastructure;

public sealed class TenantSeedDatabaseConnectionFactoryTests
{
    [Fact]
    public void BuildConnectionString_ReplacesDatabaseOnBaseConnection()
    {
        var factory = new TenantSeedDatabaseConnectionFactory(
            "Host=localhost;Port=5432;Database=primary_db;Username=postgres;Password=secret");

        var connectionString = factory.BuildConnectionString("catalog_db");
        var builder = new NpgsqlConnectionStringBuilder(connectionString);

        builder.Database.Should().Be("catalog_db");
        builder.Host.Should().Be("localhost");
    }

    [Fact]
    public void BuildConnectionString_UsesConfiguredOverrideWhenPresent()
    {
        var factory = new TenantSeedDatabaseConnectionFactory(
            "Host=localhost;Database=primary_db;Username=postgres;Password=secret",
            Options.Create(new TenantProvisioningOptions
            {
                DatabaseConnectionStrings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["remote_catalog"] = "Host=remote.example;Database=remote_catalog;Username=reader;Password=other"
                }
            }));

        var connectionString = factory.BuildConnectionString("remote_catalog");
        var builder = new NpgsqlConnectionStringBuilder(connectionString);

        builder.Host.Should().Be("remote.example");
        builder.Database.Should().Be("remote_catalog");
    }

    [Fact]
    public void ResolveDatabaseName_UsesDefaultWhenMappingValueMissing()
    {
        var factory = new TenantSeedDatabaseConnectionFactory(
            "Host=localhost;Database=primary_db;Username=postgres;Password=secret");

        factory.ResolveDatabaseName(null, "primary_db").Should().Be("primary_db");
        factory.ResolveDatabaseName("  ", "primary_db").Should().Be("primary_db");
        factory.ResolveDatabaseName("other_db", "primary_db").Should().Be("other_db");
    }
}
