using Fgs.File.Domain.Entities;
using Fgs.File.Infrastructure.Database.Schemas;
using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fgs.File.Infrastructure.Database;

public class FgsFileDbContext(
    DbContextOptions<FgsFileDbContext> options,
    ITenantContextAccessor tenantContextAccessor)
    : FgsTenantFilteredDbContext(options, tenantContextAccessor)
{
    public const string MigrationHistorySchema = FgsDatabaseSchemas.MigrationHistory;

    public DbSet<FgsFile> FgsFiles => Set<FgsFile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FgsFileDbContext).Assembly);
        EntitySchemaRegistry.ApplySchemas(modelBuilder);
        ApplyFgsTenantQueryFilters(modelBuilder);
    }
}
