using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Fgs.Scheduling.Infrastructure.Database;

public sealed class FgsSchedulingDbContextDesignFactory : IDesignTimeDbContextFactory<FgsSchedulingDbContext>
{
    public FgsSchedulingDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("FGS_DISPATCH_DB")
            ?? TryLoadConnectionStringFromApiAppsettings()
            ?? throw new InvalidOperationException(
                "Set FGS_DISPATCH_DB or run dotnet ef with --startup-project Fgs.Scheduling.API.");

        var options = new DbContextOptionsBuilder<FgsSchedulingDbContext>()
            .UseNpgsql(connectionString, npgsql =>
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", FgsSchedulingDbContext.MigrationHistorySchema))
            .Options;

        return new FgsSchedulingDbContext(options, new Fgs.MultiTenancy.Persistence.DesignTimeTenantContextAccessor());
    }

    private static string? TryLoadConnectionStringFromApiAppsettings()
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (dir is not null)
        {
            var direct = Path.Combine(dir.FullName, "Fgs.Scheduling.API", "appsettings.json");
            if (File.Exists(direct))
            {
                return new ConfigurationBuilder().AddJsonFile(direct).Build().GetConnectionString("FgsDispatch");
            }

            var underSrc = Path.Combine(dir.FullName, "src", "SchedulingService", "Fgs.Scheduling.API", "appsettings.json");
            if (File.Exists(underSrc))
            {
                return new ConfigurationBuilder().AddJsonFile(underSrc).Build().GetConnectionString("FgsDispatch");
            }

            dir = dir.Parent;
        }

        return null;
    }
}
