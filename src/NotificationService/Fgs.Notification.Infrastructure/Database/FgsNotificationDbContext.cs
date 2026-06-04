using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.Notification.Domain.Entities;
using Fgs.Notification.Infrastructure.Database.Schemas;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Notification.Infrastructure.Database;

public sealed class FgsNotificationDbContext : FgsTenantFilteredDbContext
{
    public const string MigrationHistorySchema = FgsDatabaseSchemas.MigrationHistory;

    public FgsNotificationDbContext(
        DbContextOptions<FgsNotificationDbContext> options,
        ITenantContextAccessor tenantContextAccessor)
        : base(options, tenantContextAccessor)
    {
    }

    public DbSet<FgsNotificationHistory> NotificationHistory => Set<FgsNotificationHistory>();

    public DbSet<FgsProcessedIntegrationEvent> ProcessedIntegrationEvents => Set<FgsProcessedIntegrationEvent>();

    public DbSet<FgsTenantCompanyCache> FgsTenantCompanyCaches => Set<FgsTenantCompanyCache>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FgsNotificationDbContext).Assembly);
        EntitySchemaRegistry.ApplySchemas(modelBuilder);
        ApplyFgsTenantQueryFilters(modelBuilder);
    }
}
