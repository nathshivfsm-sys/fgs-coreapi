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
            var direct = Path.Combine(dir.FullName, "Fgs.File.API", "appsettings.json");
            if (File.Exists(direct))
            {
                return new ConfigurationBuilder().AddJsonFile(direct).Build().GetConnectionString("FgsFile");
            }

            var underSrc = Path.Combine(dir.FullName, "src", "FileService", "Fgs.File.API", "appsettings.json");
            if (File.Exists(underSrc))
            {
                return new ConfigurationBuilder().AddJsonFile(underSrc).Build().GetConnectionString("FgsFile");
            }

            dir = dir.Parent;
        }

        return null;
    }
}
