using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Fgs.User.Infrastructure.Persistence.Database.DbContexts;

/// <summary>
/// Design-time factory for <c>dotnet ef</c> migrations.
/// Connection string order: <c>FGS_USER_DB</c>, then <c>FgsUser</c> from <c>Fgs.User.API/appsettings.json</c> if found by walking up from the current directory.
/// </summary>
public sealed class FgsUserDbContextDesignFactory : IDesignTimeDbContextFactory<FgsUserDbContext>
{
    public FgsUserDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("FGS_USER_DB")
            ?? TryLoadConnectionStringFromApiAppsettings()
            ?? throw new InvalidOperationException(
                "Set environment variable FGS_USER_DB or run dotnet ef from a directory under the repo so Fgs.User.API/appsettings.json can be found (or use --startup-project Fgs.User.API).");

        var options = new DbContextOptionsBuilder<FgsUserDbContext>()
            .UseNpgsql(connectionString, npgsql => npgsql.MigrationsHistoryTable("__EFMigrationsHistory", FgsUserDbContext.FgsSchema))
            .Options;

        return new FgsUserDbContext(options);
    }

    private static string? TryLoadConnectionStringFromApiAppsettings()
    {
        var path = FindApiAppsettingsPath();
        if (path is null) return null;

        var configuration = new ConfigurationBuilder()
            .AddJsonFile(path, optional: false)
            .Build();

        return configuration.GetConnectionString("FgsUser");
    }

    private static string? FindApiAppsettingsPath()
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (dir is not null)
        {
            var direct = Path.Combine(dir.FullName, "Fgs.User.API", "appsettings.json");
            if (File.Exists(direct)) return direct;

            var underSrc = Path.Combine(dir.FullName, "src", "UserService", "Fgs.User.API", "appsettings.json");
            if (File.Exists(underSrc)) return underSrc;

            dir = dir.Parent;
        }

        return null;
    }
}
