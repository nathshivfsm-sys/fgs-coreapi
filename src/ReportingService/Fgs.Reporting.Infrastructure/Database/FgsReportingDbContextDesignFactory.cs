using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Fgs.Reporting.Infrastructure.Database;

public sealed class FgsReportingDbContextDesignFactory : IDesignTimeDbContextFactory<FgsReportingDbContext>
{
    public FgsReportingDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("FGS_REPORTING_DB")
            ?? TryLoadConnectionStringFromApiAppsettings()
            ?? throw new InvalidOperationException(
                "Set FGS_REPORTING_DB or run dotnet ef with --startup-project Fgs.Reporting.API.");

        var options = new DbContextOptionsBuilder<FgsReportingDbContext>()
            .UseNpgsql(connectionString, npgsql =>
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", FgsReportingDbContext.MigrationHistorySchema))
            .Options;

        return new FgsReportingDbContext(options);
    }

    private static string? TryLoadConnectionStringFromApiAppsettings()
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (dir is not null)
        {
            var direct = Path.Combine(dir.FullName, "Fgs.Reporting.API", "appsettings.json");
            if (File.Exists(direct))
            {
                return new ConfigurationBuilder().AddJsonFile(direct).Build().GetConnectionString("FgsReporting");
            }

            var underSrc = Path.Combine(dir.FullName, "src", "ReportingService", "Fgs.Reporting.API", "appsettings.json");
            if (File.Exists(underSrc))
            {
                return new ConfigurationBuilder().AddJsonFile(underSrc).Build().GetConnectionString("FgsReporting");
            }

            dir = dir.Parent;
        }

        return null;
    }
}
