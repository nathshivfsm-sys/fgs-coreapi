using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Fgs.Notification.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Notification.Infrastructure.Database;

public sealed class FgsNotificationDbContext : FgsTenantFilteredDbContext
{
    public const string FgsSchema = "dbo";

    public FgsNotificationDbContext(
        DbContextOptions<FgsNotificationDbContext> options,
        ITenantContextAccessor tenantContextAccessor)
        : base(options, tenantContextAccessor)
    {
    }

    public DbSet<FgsNotificationHistory> NotificationHistory => Set<FgsNotificationHistory>();

    public DbSet<FgsProcessedIntegrationEvent> ProcessedIntegrationEvents => Set<FgsProcessedIntegrationEvent>();

    public DbSet<FgsSetupCommunicationTemplate> CommunicationTemplates => Set<FgsSetupCommunicationTemplate>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(FgsSchema);

        modelBuilder.Entity<FgsNotificationHistory>(entity =>
        {
            entity.ToTable("FgsNotificationHistory");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TemplateName).HasMaxLength(128);
            entity.Property(e => e.Recipient).HasMaxLength(512);
            entity.Property(e => e.CorrelationId).HasMaxLength(64);
            entity.Property(e => e.ProviderMessageId).HasMaxLength(256);
            entity.Property(e => e.Error).HasMaxLength(2000);
            entity.HasIndex(e => new { e.TenantId, e.CreatedOn });
        });

        modelBuilder.Entity<FgsProcessedIntegrationEvent>(entity =>
        {
            entity.ToTable("FgsProcessedIntegrationEvent");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.MessageId).HasMaxLength(128);
            entity.Property(e => e.EventType).HasMaxLength(128);
            entity.HasIndex(e => e.MessageId).IsUnique();
        });

        modelBuilder.Entity<FgsSetupCommunicationTemplate>(entity =>
        {
            entity.ToTable("FgsSetupCommunicationTemplate");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.TemplateType).IsRequired();
            entity.Property(e => e.Code).IsRequired();
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Body).IsRequired();
            entity.HasIndex(e => new { e.TenantId, e.CompanyId });
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.TemplateType, e.Code })
                .IsUnique();
        });

        ApplyFgsTenantQueryFilters(modelBuilder);
        ApplyFgsNullableTenantCompanyQueryFilter<FgsSetupCommunicationTemplate>(modelBuilder);
    }
}
