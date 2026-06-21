using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class GloInventoryCategoryConfiguration : IEntityTypeConfiguration<GloInventoryCategory>
{
    public void Configure(EntityTypeBuilder<GloInventoryCategory> entity)
    {
        entity.ToTable("GloInventoryCategory", t =>
            t.HasComment("Global inventory category catalog scoped to a business type."));
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn()
            .HasComment("Primary key.");
        entity.HasIndex(e => new { e.BusinessTypeId, e.CategoryCode })
            .IsUnique()
            .HasDatabaseName("UQ_GloInventoryCategory_BusinessTypeId_CategoryCode");
        entity.HasIndex(e => e.BusinessTypeId)
            .HasDatabaseName("IX_GloInventoryCategory_BusinessTypeId");
        entity.Property(e => e.BusinessTypeId)
            .HasComment("Business type that owns this category.");
        entity.Property(e => e.CategoryCode).HasMaxLength(50)
            .HasComment("Unique category code within the business type.");
        entity.Property(e => e.Name).HasMaxLength(150)
            .HasComment("Display name of the category.");
        entity.Property(e => e.Description).HasColumnType("text")
            .HasComment("Description of the category.");
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1)
            .HasComment("Display order.");
        entity.Property(e => e.IsActive).HasDefaultValue(true)
            .HasComment("Indicates whether the category is active.");
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        entity.HasOne<GloBusinessType>()
            .WithMany()
            .HasForeignKey(e => e.BusinessTypeId)
            .HasConstraintName("FK_GloInventoryCategory_GloBusinessType")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
