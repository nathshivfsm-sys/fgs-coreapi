using Fgs.Reporting.Domain.Entities;
using Fgs.Reporting.Infrastructure.Database.Schemas;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Reporting.Infrastructure.Database;

public sealed class FgsReportingDbContext(DbContextOptions<FgsReportingDbContext> options) : DbContext(options)
{
    public const string MigrationHistorySchema = FgsDatabaseSchemas.MigrationHistory;

    public DbSet<FgsTenantCompanyCache> FgsTenantCompanyCaches => Set<FgsTenantCompanyCache>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(FgsDatabaseSchemas.Reporting);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FgsReportingDbContext).Assembly);
    }
}
