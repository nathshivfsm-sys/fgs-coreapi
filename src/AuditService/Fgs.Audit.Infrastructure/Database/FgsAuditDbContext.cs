using Fgs.Audit.Domain.Entities;
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FgsAuditDbContext).Assembly);
        EntitySchemaRegistry.ApplySchemas(modelBuilder);
        ApplyFgsTenantQueryFilters(modelBuilder);
    }
}
