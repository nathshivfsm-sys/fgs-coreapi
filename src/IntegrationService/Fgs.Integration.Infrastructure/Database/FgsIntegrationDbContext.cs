using Fgs.Integration.Domain.Entities;
using Fgs.Integration.Infrastructure.Database.Configurations;
using Fgs.Integration.Infrastructure.Database.Schemas;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Integration.Infrastructure.Database;

public sealed class FgsIntegrationDbContext(DbContextOptions<FgsIntegrationDbContext> options) : DbContext(options)
{
    public const string MigrationHistorySchema = FgsDatabaseSchemas.MigrationHistory;

    public DbSet<FgsTenantCompanyCache> FgsTenantCompanyCaches => Set<FgsTenantCompanyCache>();

    public DbSet<FgsPaymentTransactionPayload> FgsPaymentTransactionPayloads => Set<FgsPaymentTransactionPayload>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(FgsDatabaseSchemas.Integration);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FgsIntegrationDbContext).Assembly);
        FgsIntegrationDbContextConfigurationExtensions.ApplyTenantCompanyCacheForeignKeys(modelBuilder);
    }
}
