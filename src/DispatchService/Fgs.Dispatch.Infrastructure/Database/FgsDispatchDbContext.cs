using Fgs.Dispatch.Domain.Entities;
using Fgs.Dispatch.Infrastructure.Database.Schemas;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Dispatch.Infrastructure.Database;

public sealed class FgsDispatchDbContext(DbContextOptions<FgsDispatchDbContext> options) : DbContext(options)
{
    public const string MigrationHistorySchema = FgsDatabaseSchemas.MigrationHistory;

    public DbSet<FgsTenantCompanyCache> FgsTenantCompanyCaches => Set<FgsTenantCompanyCache>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(FgsDatabaseSchemas.Dispatch);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FgsDispatchDbContext).Assembly);
    }
}
