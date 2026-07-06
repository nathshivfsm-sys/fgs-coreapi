using Fgs.Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Inventory.Infrastructure.Database.Configurations;

internal sealed class FgsInventoryCategoryConfiguration : IEntityTypeConfiguration<FgsInventoryCategory>
{
    public void Configure(EntityTypeBuilder<FgsInventoryCategory> entity)
    {
        entity.ToTable(
            "FgsInventoryCategory",
            t => t.HasComment("Stores the first level of inventory classification."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn().HasComment("Unique identifier.");
        entity.ConfigureCatalogColumns();

        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");
        entity.Property(e => e.CategoryCode).HasMaxLength(50).IsRequired()
            .HasComment("Unique category code within a company.");
        entity.Property(e => e.Name).HasMaxLength(150).IsRequired()
            .HasComment("Category display name.");
        entity.Property(e => e.Description).HasColumnType("text")
            .HasComment("Optional description.");
        entity.Property(e => e.TextColor).HasMaxLength(20)
            .HasComment("UI text color.");
        entity.Property(e => e.BackgroundColor).HasMaxLength(20)
            .HasComment("UI background color.");
        entity.Property(e => e.DisplayIconFileId)
            .HasComment("Display icon stored in FgsFile.");
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1)
            .HasComment("Display order.");
        entity.Property(e => e.IsSystem).HasDefaultValue(false)
            .HasComment("Seeded system record.");
        entity.Property(e => e.IsActive).HasComment("Active flag.");
        entity.Property(e => e.CreatedOn).HasComment("Created timestamp.");
        entity.Property(e => e.CreatedBy).HasComment("Created by user.");
        entity.Property(e => e.UpdatedOn).HasComment("Updated timestamp.");
        entity.Property(e => e.UpdatedBy).HasComment("Updated by user.");

        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.CategoryCode })
            .HasName("UQ_FgsInventoryCategory_TenantId_CompanyId_CategoryCode");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsInventoryCategory_TenantId_CompanyId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Name })
            .HasDatabaseName("IX_FgsInventoryCategory_TenantId_CompanyId_Name");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsActive })
            .HasDatabaseName("IX_FgsInventoryCategory_TenantId_CompanyId_IsActive");
    }
}
