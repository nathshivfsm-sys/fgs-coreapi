using Fgs.MultiTenancy.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Fgs.User.Infrastructure.Database;

public sealed class FgsUserDbContextDesignFactory : IDesignTimeDbContextFactory<FgsUserDbContext>
{
    public FgsUserDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("FGS_USER_DB")
            ?? TryLoadConnectionStringFromApiAppsettings()
            ?? throw new InvalidOperationException(
                "Set FGS_USER_DB or run dotnet ef with --startup-project Fgs.User.API.");

        var options = new DbContextOptionsBuilder<FgsUserDbContext>()
            .UseNpgsql(connectionString, npgsql =>
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", FgsUserDbContext.MigrationHistorySchema))
            .Options;

        return new FgsUserDbContext(options, new DesignTimeTenantContextAccessor());
    }

    private static string? TryLoadConnectionStringFromApiAppsettings()
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (dir is not null)
        {
            var directDir = Path.Combine(dir.FullName, "Fgs.User.API");
            if (Directory.Exists(directDir))
            {
                return BuildApiConfiguration(directDir).GetConnectionString("FgsUser");
            }

            var underSrc = Path.Combine(dir.FullName, "src", "UserService", "Fgs.User.API");
            if (Directory.Exists(underSrc))
            {
                return BuildApiConfiguration(underSrc).GetConnectionString("FgsUser");
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
