using Fgs.Crm.Domain.Entities;
using Fgs.Crm.Infrastructure.Database.Schemas;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Crm.Infrastructure.Database;

public sealed class FgsCrmDbContext(DbContextOptions<FgsCrmDbContext> options) : DbContext(options)
{
    public const string MigrationHistorySchema = FgsDatabaseSchemas.MigrationHistory;

    public DbSet<FgsTenantCompanyCache> FgsTenantCompanyCaches => Set<FgsTenantCompanyCache>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(FgsDatabaseSchemas.Crm);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FgsCrmDbContext).Assembly);
    }
}
