using Fgs.Scheduling.Domain.Entities;
using Fgs.Scheduling.Infrastructure.Database.Schemas;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Scheduling.Infrastructure.Database;

public sealed class FgsSchedulingDbContext(DbContextOptions<FgsSchedulingDbContext> options) : DbContext(options)
{
    public const string MigrationHistorySchema = FgsDatabaseSchemas.MigrationHistory;

    public DbSet<FgsTenantCompanyCache> FgsTenantCompanyCaches => Set<FgsTenantCompanyCache>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(FgsDatabaseSchemas.Dispatch);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FgsSchedulingDbContext).Assembly);
    }
}
