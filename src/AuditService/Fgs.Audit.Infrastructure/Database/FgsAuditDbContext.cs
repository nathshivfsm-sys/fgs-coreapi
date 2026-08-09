using Fgs.Audit.Domain.Entities;
using Fgs.Audit.Domain.Enums;
using Fgs.Audit.Infrastructure.Database.Schemas;
using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Audit.Infrastructure.Database;

public class FgsAuditDbContext(
    DbContextOptions<FgsAuditDbContext> options,
    ITenantContextAccessor tenantContextAccessor)
    : FgsTenantFilteredDbContext(options, tenantContextAccessor)
{
    public const string MigrationHistorySchema = FgsDatabaseSchemas.MigrationHistory;

    public DbSet<FgsCredentialAudit> FgsCredentialAudits => Set<FgsCredentialAudit>();

    public DbSet<FgsEvent> FgsEvents => Set<FgsEvent>();

    public DbSet<FgsEventDetail> FgsEventDetails => Set<FgsEventDetail>();

    public DbSet<FgsEventAttachment> FgsEventAttachments => Set<FgsEventAttachment>();

    public DbSet<FgsArchiveCatalog> FgsArchiveCatalogs => Set<FgsArchiveCatalog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<AuditRecordType>(FgsDatabaseSchemas.Audit, "record_type", nameTranslator: new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
        modelBuilder.HasPostgresEnum<AuditEventSource>(FgsDatabaseSchemas.Audit, "event_source", nameTranslator: new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
        modelBuilder.HasPostgresEnum<AuditEventDetailType>(FgsDatabaseSchemas.Audit, "event_detail_type", nameTranslator: new Npgsql.NameTranslation.NpgsqlNullNameTranslator());

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FgsAuditDbContext).Assembly);
        EntitySchemaRegistry.ApplySchemas(modelBuilder);
        ApplyFgsTenantQueryFilters(modelBuilder);
    }
}
