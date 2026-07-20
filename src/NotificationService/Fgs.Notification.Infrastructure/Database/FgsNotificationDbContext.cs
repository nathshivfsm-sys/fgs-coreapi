using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.Notification.Domain.Entities;
using Fgs.Notification.Domain.Enums;
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

    public DbSet<FgsEmailHistory> FgsEmailHistories => Set<FgsEmailHistory>();

    public DbSet<FgsSmsHistory> FgsSmsHistories => Set<FgsSmsHistory>();

    public DbSet<FgsProcessedIntegrationEvent> ProcessedIntegrationEvents => Set<FgsProcessedIntegrationEvent>();

    public DbSet<FgsTenantCompanyCache> FgsTenantCompanyCaches => Set<FgsTenantCompanyCache>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (Database.IsNpgsql())
        {
            var nullTranslator = new Npgsql.NameTranslation.NpgsqlNullNameTranslator();
            modelBuilder.HasPostgresEnum<NotificationStatus>(
                FgsDatabaseSchemas.Notification, "notification_status", nameTranslator: nullTranslator);
            modelBuilder.HasPostgresEnum<NotificationSourceApplication>(
                FgsDatabaseSchemas.Notification, "source_application", nameTranslator: nullTranslator);
        }

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FgsNotificationDbContext).Assembly);
        EntitySchemaRegistry.ApplySchemas(modelBuilder);
        ApplyFgsTenantQueryFilters(modelBuilder);
    }
}
