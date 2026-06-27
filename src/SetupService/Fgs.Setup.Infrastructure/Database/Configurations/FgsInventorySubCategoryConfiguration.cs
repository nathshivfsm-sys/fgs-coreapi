using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsInventorySubCategoryConfiguration : IEntityTypeConfiguration<FgsInventorySubCategory>
{
    public void Configure(EntityTypeBuilder<FgsInventorySubCategory> entity)
    {
        entity.ToTable("FgsInventorySubCategory");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns();
        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.InventoryCategoryId, e.SubCategoryCode })
            .HasName("UQ_FgsInventorySubCategory_TenantId_CompanyId_InventoryCategoryId_SubCategoryCode");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.InventoryCategoryId })
            .HasDatabaseName("IX_FgsInventorySubCategory_TenantId_CompanyId_InventoryCategoryId");
        entity.Property(e => e.SubCategoryCode).HasMaxLength(50);
        entity.Property(e => e.Name).HasMaxLength(150);
        entity.Property(e => e.Description).HasColumnType("text");
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1);
        entity.Property(e => e.IsSystem).HasDefaultValue(false);
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        entity.HasOne<FgsInventoryCategory>()
            .WithMany()
            .HasForeignKey(e => e.InventoryCategoryId)
            .HasConstraintName("FK_FgsInventorySubCategory_FgsInventoryCategory")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
