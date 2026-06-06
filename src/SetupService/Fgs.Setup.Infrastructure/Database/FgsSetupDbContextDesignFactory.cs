using Fgs.MultiTenancy.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Fgs.Setup.Infrastructure.Database;

/// <summary>
/// Design-time factory for <c>dotnet ef</c> migrations.
/// Connection string order: <c>FGS_SETUP_DB</c>, then <c>FgsSetup</c> from <c>Fgs.Setup.API/appsettings.json</c> if found by walking up from the current directory.
/// </summary>
public sealed class FgsSetupDbContextDesignFactory : IDesignTimeDbContextFactory<FgsSetupDbContext>
{
    public FgsSetupDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("FGS_SETUP_DB")
            ?? TryLoadConnectionStringFromApiAppsettings()
            ?? throw new InvalidOperationException(
                "Set environment variable FGS_SETUP_DB or run dotnet ef from a directory under the repo so Fgs.Setup.API/appsettings.json can be found (or use --startup-project Fgs.Setup.API).");

        var options = new DbContextOptionsBuilder<FgsSetupDbContext>()
            .UseNpgsql(connectionString, npgsql => npgsql.MigrationsHistoryTable("__EFMigrationsHistory", FgsSetupDbContext.MigrationHistorySchema))
            .Options;

        return new FgsSetupDbContext(options, new DesignTimeTenantContextAccessor());
    }

    private static string? TryLoadConnectionStringFromApiAppsettings()
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (dir is not null)
        {
            var directDir = Path.Combine(dir.FullName, "Fgs.Setup.API");
            if (Directory.Exists(directDir))
            {
                return BuildApiConfiguration(directDir).GetConnectionString("FgsSetup");
            }

            var underSrcDir = Path.Combine(dir.FullName, "src/SetupService/Fgs.Setup.API");
            if (Directory.Exists(underSrcDir))
            {
                return BuildApiConfiguration(underSrcDir).GetConnectionString("FgsSetup");
            }

            dir = dir.Parent;
        }

        return null;
    }

    private static IConfiguration BuildApiConfiguration(string apiDirectory) =>
        new ConfigurationBuilder()
            .SetBasePath(apiDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
}
