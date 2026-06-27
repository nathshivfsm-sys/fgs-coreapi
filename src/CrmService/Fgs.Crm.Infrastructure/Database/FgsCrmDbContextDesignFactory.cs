using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Fgs.Crm.Infrastructure.Database;

public sealed class FgsCrmDbContextDesignFactory : IDesignTimeDbContextFactory<FgsCrmDbContext>
{
    public FgsCrmDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("FGS_CRM_DB")
            ?? TryLoadConnectionStringFromApiAppsettings()
            ?? throw new InvalidOperationException(
                "Set FGS_CRM_DB or run dotnet ef with --startup-project Fgs.Crm.API.");

        var options = new DbContextOptionsBuilder<FgsCrmDbContext>()
            .UseNpgsql(connectionString, npgsql =>
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", FgsCrmDbContext.MigrationHistorySchema))
            .Options;

        return new FgsCrmDbContext(options);
    }

    private static string? TryLoadConnectionStringFromApiAppsettings()
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (dir is not null)
        {
            var direct = Path.Combine(dir.FullName, "Fgs.Crm.API", "appsettings.json");
            if (File.Exists(direct))
            {
                return new ConfigurationBuilder().AddJsonFile(direct).Build().GetConnectionString("FgsCrm");
            }

            var underSrc = Path.Combine(dir.FullName, "src", "CrmService", "Fgs.Crm.API", "appsettings.json");
            if (File.Exists(underSrc))
            {
                return new ConfigurationBuilder().AddJsonFile(underSrc).Build().GetConnectionString("FgsCrm");
            }

            dir = dir.Parent;
        }

        return null;
    }
}
