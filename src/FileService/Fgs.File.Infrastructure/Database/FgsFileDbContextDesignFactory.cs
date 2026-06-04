using Fgs.MultiTenancy.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Fgs.File.Infrastructure.Database;

public sealed class FgsFileDbContextDesignFactory : IDesignTimeDbContextFactory<FgsFileDbContext>
{
    public FgsFileDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("FGS_FILE_DB")
            ?? TryLoadConnectionStringFromApiAppsettings()
            ?? throw new InvalidOperationException(
                "Set FGS_FILE_DB or run dotnet ef with --startup-project Fgs.File.API.");

        var options = new DbContextOptionsBuilder<FgsFileDbContext>()
            .UseNpgsql(connectionString, npgsql =>
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", FgsFileDbContext.MigrationHistorySchema))
            .Options;

        return new FgsFileDbContext(options, new DesignTimeTenantContextAccessor());
    }

    private static string? TryLoadConnectionStringFromApiAppsettings()
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (dir is not null)
        {
            var directDir = Path.Combine(dir.FullName, "Fgs.File.API");
            if (Directory.Exists(directDir))
            {
                return BuildApiConfiguration(directDir).GetConnectionString("FgsFile");
            }

            var underSrcDir = Path.Combine(dir.FullName, "src/FileService/Fgs.File.API");
            if (Directory.Exists(underSrcDir))
            {
                return BuildApiConfiguration(underSrcDir).GetConnectionString("FgsFile");
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
