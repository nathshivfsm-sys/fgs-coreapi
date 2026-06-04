using Fgs.Inventory.Domain.Entities;
using Fgs.Inventory.Infrastructure.Database.Schemas;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Inventory.Infrastructure.Database;

public sealed class FgsInventoryDbContext(DbContextOptions<FgsInventoryDbContext> options) : DbContext(options)
{
    public const string MigrationHistorySchema = FgsDatabaseSchemas.MigrationHistory;

    public DbSet<FgsTenantCompanyCache> FgsTenantCompanyCaches => Set<FgsTenantCompanyCache>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(FgsDatabaseSchemas.Inventory);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FgsInventoryDbContext).Assembly);
    }
}
