using Fgs.MultiTenancy.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Fgs.Audit.Infrastructure.Database;

public sealed class FgsAuditDbContextDesignFactory : IDesignTimeDbContextFactory<FgsAuditDbContext>
{
    public FgsAuditDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("FGS_AUDIT_DB")
            ?? TryLoadConnectionStringFromApiAppsettings()
            ?? throw new InvalidOperationException(
                "Set FGS_AUDIT_DB or run dotnet ef with --startup-project Fgs.Audit.API.");

        var options = new DbContextOptionsBuilder<FgsAuditDbContext>()
            .UseNpgsql(connectionString, npgsql =>
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", FgsAuditDbContext.MigrationHistorySchema))
            .Options;

        return new FgsAuditDbContext(options, new DesignTimeTenantContextAccessor());
    }

    private static string? TryLoadConnectionStringFromApiAppsettings()
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (dir is not null)
        {
            var direct = Path.Combine(dir.FullName, "Fgs.Audit.API", "appsettings.json");
            if (File.Exists(direct))
            {
                return new ConfigurationBuilder().AddJsonFile(direct).Build().GetConnectionString("FgsAudit");
            }

            var underSrc = Path.Combine(dir.FullName, "src", "AuditService", "Fgs.Audit.API", "appsettings.json");
            if (File.Exists(underSrc))
            {
                return new ConfigurationBuilder().AddJsonFile(underSrc).Build().GetConnectionString("FgsAudit");
            }

            dir = dir.Parent;
        }

        return null;
    }
}
