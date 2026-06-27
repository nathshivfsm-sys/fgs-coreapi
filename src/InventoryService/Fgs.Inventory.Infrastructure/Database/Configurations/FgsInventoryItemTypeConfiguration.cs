using Fgs.Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Inventory.Infrastructure.Database.Configurations;

internal sealed class FgsInventoryItemTypeConfiguration : IEntityTypeConfiguration<FgsInventoryItemType>
{
    public void Configure(EntityTypeBuilder<FgsInventoryItemType> entity)
    {
        entity.ToTable(
            "FgsInventoryItemType",
            t => t.HasComment(
                "Stores inventory item types used to classify inventory items and determine whether quantity is tracked."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn().HasComment("Unique identifier for the inventory item type.");
        entity.ConfigureCatalogColumns();

        entity.Property(e => e.TenantId).HasComment("Tenant that owns this record.");
        entity.Property(e => e.CompanyId).HasComment("Company that owns this record.");
        entity.Property(e => e.ItemTypeCode).HasMaxLength(30).IsRequired()
            .HasComment("Unique code for the inventory item type within a company.");
        entity.Property(e => e.Name).HasMaxLength(50).IsRequired()
            .HasComment("Display name of the inventory item type.");
        entity.Property(e => e.Description).HasColumnType("text")
            .HasComment("Optional description of the inventory item type.");
        entity.Property(e => e.TracksQuantity).HasDefaultValue(false)
            .HasComment("Indicates whether inventory quantities are maintained for this item type.");
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1)
            .HasComment("Controls display order in user interfaces.");
        entity.Property(e => e.IsSystem).HasDefaultValue(false)
            .HasComment("Indicates whether this is a system-defined record.");
        entity.Property(e => e.IsActive).HasComment("Indicates whether the record is active.");
        entity.Property(e => e.CreatedOn).HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).HasComment("User who created the record.");
        entity.Property(e => e.UpdatedOn).HasComment("Date and time the record was last updated.");
        entity.Property(e => e.UpdatedBy).HasComment("User who last updated the record.");

        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.ItemTypeCode })
            .HasName("UQ_FgsInventoryItemType_TenantId_CompanyId_ItemTypeCode");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsInventoryItemType_TenantId_CompanyId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Name })
            .HasDatabaseName("IX_FgsInventoryItemType_TenantId_CompanyId_Name");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsActive })
            .HasDatabaseName("IX_FgsInventoryItemType_TenantId_CompanyId_IsActive");
    }
}
