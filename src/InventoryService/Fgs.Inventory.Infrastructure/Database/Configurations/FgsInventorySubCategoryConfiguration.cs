using Fgs.Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Inventory.Infrastructure.Database.Configurations;

internal sealed class FgsInventorySubCategoryConfiguration : IEntityTypeConfiguration<FgsInventorySubCategory>
{
    public void Configure(EntityTypeBuilder<FgsInventorySubCategory> entity)
    {
        entity.ToTable(
            "FgsInventorySubCategory",
            t => t.HasComment(
                "Stores inventory sub-categories used to classify inventory items under a parent category."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn().HasComment("Unique identifier.");
        entity.ConfigureCatalogColumns();

        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");
        entity.Property(e => e.InventoryCategoryId).HasComment("Parent inventory category.");
        entity.Property(e => e.SubCategoryCode).HasMaxLength(50).IsRequired()
            .HasComment("Unique code within a category.");
        entity.Property(e => e.Name).HasMaxLength(150).IsRequired()
            .HasComment("Display name.");
        entity.Property(e => e.Description).HasColumnType("text")
            .HasComment("Description.");
        entity.Property(e => e.TextColor).HasMaxLength(20)
            .HasComment("UI text color.");
        entity.Property(e => e.BackgroundColor).HasMaxLength(20)
            .HasComment("UI background color.");
        entity.Property(e => e.DisplayIconFileId)
            .HasComment("Icon file identifier.");
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1)
            .HasComment("Display order.");
        entity.Property(e => e.IsSystem).HasDefaultValue(false)
            .HasComment("System record.");
        entity.Property(e => e.IsActive).HasComment("Active flag.");
        entity.Property(e => e.CreatedOn).HasComment("Created date/time.");
        entity.Property(e => e.CreatedBy).HasComment("Created by.");
        entity.Property(e => e.UpdatedOn).HasComment("Updated date/time.");
        entity.Property(e => e.UpdatedBy).HasComment("Updated by.");

        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.InventoryCategoryId, e.SubCategoryCode })
            .HasName("UQ_FgsInventorySubCategory_TenantId_CompanyId_InventoryCategoryId_SubCategoryCode");

        entity.HasOne<FgsInventoryCategory>()
            .WithMany()
            .HasForeignKey(e => e.InventoryCategoryId)
            .HasConstraintName("FK_FgsInventorySubCategory_FgsInventoryCategory")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsInventorySubCategory_TenantId_CompanyId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.InventoryCategoryId })
            .HasDatabaseName("IX_FgsInventorySubCategory_TenantId_CompanyId_InventoryCategoryId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Name })
            .HasDatabaseName("IX_FgsInventorySubCategory_TenantId_CompanyId_Name");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsActive })
            .HasDatabaseName("IX_FgsInventorySubCategory_TenantId_CompanyId_IsActive");
    }
}
