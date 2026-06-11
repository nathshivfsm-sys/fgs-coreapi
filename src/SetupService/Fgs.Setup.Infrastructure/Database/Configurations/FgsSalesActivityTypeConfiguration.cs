using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsSalesActivityTypeConfiguration : IEntityTypeConfiguration<FgsSalesActivityType>
{
    public void Configure(EntityTypeBuilder<FgsSalesActivityType> entity)
    {
        entity.ToTable(
            "FgsSalesActivityType",
            t =>
            {
                t.HasComment(
                    "Stores tenant/company specific sales activity types used by Leads and Opportunities. Seeded from glo.GloSalesActivityType.");
                t.HasCheckConstraint(
                    "CK_FgsSalesActivityType_AppliesToEntity",
                    "\"AppliesToLead\" = true OR \"AppliesToOpportunity\" = true");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .UseIdentityAlwaysColumn()
            .HasComment("Unique identifier for the sales activity type.");

        entity.Property(e => e.TenantId).HasComment("Tenant identifier that owns the record.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier that owns the record.");
        entity.Property(e => e.ActivityTypeCode)
            .HasMaxLength(50)
            .IsRequired()
            .HasComment("Immutable business code for the sales activity type.");
        entity.Property(e => e.ActivityTypeName)
            .HasMaxLength(100)
            .IsRequired()
            .HasComment("User-friendly name displayed throughout the application.");
        entity.Property(e => e.Description)
            .HasMaxLength(255)
            .HasComment("Optional description explaining the sales activity type.");
        entity.Property(e => e.AppliesToLead)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the activity type can be used by Leads.");
        entity.Property(e => e.AppliesToOpportunity)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the activity type can be used by Opportunities.");
        entity.Property(e => e.AllowManualSelection)
            .HasDefaultValue(true)
            .HasComment("Indicates whether users may manually select this activity type.");
        entity.Property(e => e.DisplayOrder)
            .HasDefaultValue((short)1)
            .HasComment("Controls the order in which activity types are displayed.");
        entity.Property(e => e.IsSystem)
            .HasDefaultValue(false)
            .HasComment("Indicates whether the activity type was seeded by the system. System records should have immutable business codes.");
        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the activity type is available for use.");

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).HasComment("User who created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz").HasComment("Date and time the record was last updated.");
        entity.Property(e => e.UpdatedBy).HasComment("User who last updated the record.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsSalesActivityType_TenantId_CompanyId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ActivityTypeCode })
            .IsUnique()
            .HasDatabaseName("UX_FgsSalesActivityType_TenantId_CompanyId_ActivityTypeCode");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ActivityTypeName })
            .IsUnique()
            .HasDatabaseName("UX_FgsSalesActivityType_TenantId_CompanyId_ActivityTypeName");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.DisplayOrder })
            .HasDatabaseName("IX_FgsSalesActivityType_TenantId_CompanyId_DisplayOrder");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsActive })
            .HasDatabaseName("IX_FgsSalesActivityType_TenantId_CompanyId_IsActive");
    }
}
