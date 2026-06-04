using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Fgs.Integration.Infrastructure.Database;

public sealed class FgsIntegrationDbContextDesignFactory : IDesignTimeDbContextFactory<FgsIntegrationDbContext>
{
    public FgsIntegrationDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        var connectionString = Environment.GetEnvironmentVariable("FGS_INTEGRATION_DB")
            ?? FgsIntegrationConnectionString.ResolveRequired(configuration);

        var options = new DbContextOptionsBuilder<FgsIntegrationDbContext>()
            .UseNpgsql(connectionString, npgsql =>
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", FgsIntegrationDbContext.MigrationHistorySchema))
            .Options;

        return new FgsIntegrationDbContext(options);
    }

    private static IConfiguration BuildConfiguration()
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (dir is not null)
        {
            var directDir = Path.Combine(dir.FullName, "Fgs.Integration.API");
            if (Directory.Exists(directDir))
            {
                return new ConfigurationBuilder()
                    .SetBasePath(directDir)
                    .AddJsonFile("appsettings.json", optional: false)
                    .AddJsonFile("appsettings.Development.json", optional: true)
                    .Build();
            }

            var underSrcDir = Path.Combine(dir.FullName, "src", "IntegrationService", "Fgs.Integration.API");
            if (Directory.Exists(underSrcDir))
            {
                return new ConfigurationBuilder()
                    .SetBasePath(underSrcDir)
                    .AddJsonFile("appsettings.json", optional: false)
                    .AddJsonFile("appsettings.Development.json", optional: true)
                    .Build();
            }

            dir = dir.Parent;
        }

        throw new InvalidOperationException(
            "Run dotnet ef with --startup-project Fgs.Integration.API or set FGS_INTEGRATION_DB.");
    }
}
