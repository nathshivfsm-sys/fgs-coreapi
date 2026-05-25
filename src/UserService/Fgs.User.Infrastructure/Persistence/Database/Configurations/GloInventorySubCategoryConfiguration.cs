using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Persistence.Database.Configurations;

internal class GloInventorySubCategoryConfiguration : IEntityTypeConfiguration<GloInventorySubCategory>
{
    public void Configure(EntityTypeBuilder<GloInventorySubCategory> entity)
    {
        entity.ToTable("GloInventorySubCategory");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.HasIndex(e => new { e.InventoryCategoryId, e.SubCategoryCode })
            .IsUnique()
            .HasDatabaseName("UQ_GloInventorySubCategory_InventoryCategoryId_SubCategoryCode");
        entity.HasIndex(e => e.InventoryCategoryId)
            .HasDatabaseName("IX_GloInventorySubCategory_InventoryCategoryId");
        entity.Property(e => e.SubCategoryCode).HasMaxLength(50);
        entity.Property(e => e.Name).HasMaxLength(150);
        entity.Property(e => e.Description).HasColumnType("text");
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1);
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.HasOne<GloInventoryCategory>()
            .WithMany()
            .HasForeignKey(e => e.InventoryCategoryId)
            .HasConstraintName("FK_GloInventorySubCategory_GloInventoryCategory")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
