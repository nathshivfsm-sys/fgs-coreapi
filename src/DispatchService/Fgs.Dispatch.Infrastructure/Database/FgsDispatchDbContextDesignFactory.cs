using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Fgs.Dispatch.Infrastructure.Database;

public sealed class FgsDispatchDbContextDesignFactory : IDesignTimeDbContextFactory<FgsDispatchDbContext>
{
    public FgsDispatchDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("FGS_DISPATCH_DB")
            ?? TryLoadConnectionStringFromApiAppsettings()
            ?? throw new InvalidOperationException(
                "Set FGS_DISPATCH_DB or run dotnet ef with --startup-project Fgs.Dispatch.API.");

        var options = new DbContextOptionsBuilder<FgsDispatchDbContext>()
            .UseNpgsql(connectionString, npgsql =>
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", FgsDispatchDbContext.MigrationHistorySchema))
            .Options;

        return new FgsDispatchDbContext(options);
    }

    private static string? TryLoadConnectionStringFromApiAppsettings()
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (dir is not null)
        {
            var direct = Path.Combine(dir.FullName, "Fgs.Dispatch.API", "appsettings.json");
            if (File.Exists(direct))
            {
                return new ConfigurationBuilder().AddJsonFile(direct).Build().GetConnectionString("FgsDispatch");
            }

            var underSrc = Path.Combine(dir.FullName, "src", "DispatchService", "Fgs.Dispatch.API", "appsettings.json");
            if (File.Exists(underSrc))
            {
                return new ConfigurationBuilder().AddJsonFile(underSrc).Build().GetConnectionString("FgsDispatch");
            }

            dir = dir.Parent;
        }

        return null;
    }
}
