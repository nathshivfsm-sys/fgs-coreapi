using Fgs.Audit.Domain.Enums;
using Fgs.Audit.Infrastructure.Database.Schemas;
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
            {
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", FgsAuditDbContext.MigrationHistorySchema);
                npgsql.MapEnum<AuditRecordType>("record_type", FgsDatabaseSchemas.Audit, nameTranslator: new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
                npgsql.MapEnum<AuditEventSource>("event_source", FgsDatabaseSchemas.Audit, nameTranslator: new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
                npgsql.MapEnum<AuditEventDetailType>("event_detail_type", FgsDatabaseSchemas.Audit, nameTranslator: new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
            })
            .Options;

        return new FgsAuditDbContext(options, new DesignTimeTenantContextAccessor());
    }

    private static string? TryLoadConnectionStringFromApiAppsettings()
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (dir is not null)
        {
            var directDir = Path.Combine(dir.FullName, "Fgs.Audit.API");
            if (Directory.Exists(directDir))
            {
                return BuildApiConfiguration(directDir).GetConnectionString("FgsAudit");
            }

            var underSrcDir = Path.Combine(dir.FullName, "src/AuditService/Fgs.Audit.API");
            if (Directory.Exists(underSrcDir))
            {
                return BuildApiConfiguration(underSrcDir).GetConnectionString("FgsAudit");
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
