using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class GloInventorySubCategoryConfiguration : IEntityTypeConfiguration<GloInventorySubCategory>
{
    public void Configure(EntityTypeBuilder<GloInventorySubCategory> entity)
    {
        entity.ToTable("GloInventorySubCategory", t =>
            t.HasComment("Global inventory sub-category catalog scoped to an inventory category."));
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn()
            .HasComment("Primary key.");
        entity.HasIndex(e => new { e.InventoryCategoryId, e.SubCategoryCode })
            .IsUnique()
            .HasDatabaseName("UQ_GloInventorySubCategory_InventoryCategoryId_SubCategoryCode");
        entity.HasIndex(e => e.InventoryCategoryId)
            .HasDatabaseName("IX_GloInventorySubCategory_InventoryCategoryId");
        entity.Property(e => e.InventoryCategoryId)
            .HasComment("Parent inventory category.");
        entity.Property(e => e.SubCategoryCode).HasMaxLength(50)
            .HasComment("Unique sub-category code within the category.");
        entity.Property(e => e.Name).HasMaxLength(150)
            .HasComment("Display name of the sub-category.");
        entity.Property(e => e.Description).HasColumnType("text")
            .HasComment("Description of the sub-category.");
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1)
            .HasComment("Display order.");
        entity.Property(e => e.IsActive).HasDefaultValue(true)
            .HasComment("Indicates whether the sub-category is active.");
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.HasOne<GloInventoryCategory>()
            .WithMany()
            .HasForeignKey(e => e.InventoryCategoryId)
            .HasConstraintName("FK_GloInventorySubCategory_GloInventoryCategory")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
