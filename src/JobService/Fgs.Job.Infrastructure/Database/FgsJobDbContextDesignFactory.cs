using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Fgs.Job.Infrastructure.Database;

public sealed class FgsJobDbContextDesignFactory : IDesignTimeDbContextFactory<FgsJobDbContext>
{
    public FgsJobDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("FGS_JOB_DB")
            ?? TryLoadConnectionStringFromApiAppsettings()
            ?? throw new InvalidOperationException(
                "Set FGS_JOB_DB or run dotnet ef with --startup-project Fgs.Job.API.");

        var options = new DbContextOptionsBuilder<FgsJobDbContext>()
            .UseNpgsql(connectionString, npgsql =>
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", FgsJobDbContext.MigrationHistorySchema))
            .Options;

        return new FgsJobDbContext(options);
    }

    private static string? TryLoadConnectionStringFromApiAppsettings()
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (dir is not null)
        {
            var direct = Path.Combine(dir.FullName, "Fgs.Job.API", "appsettings.json");
            if (File.Exists(direct))
            {
                return new ConfigurationBuilder().AddJsonFile(direct).Build().GetConnectionString("FgsJob");
            }

            var underSrc = Path.Combine(dir.FullName, "src", "JobService", "Fgs.Job.API", "appsettings.json");
            if (File.Exists(underSrc))
            {
                return new ConfigurationBuilder().AddJsonFile(underSrc).Build().GetConnectionString("FgsJob");
            }

            dir = dir.Parent;
        }

        return null;
    }
}
