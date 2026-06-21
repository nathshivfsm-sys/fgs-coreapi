using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class GloInventoryItemTypeConfiguration : IEntityTypeConfiguration<GloInventoryItemType>
{
    public void Configure(EntityTypeBuilder<GloInventoryItemType> entity)
    {
        entity.ToTable("GloInventoryItemType", t =>
            t.HasComment("Global inventory item type catalog (inventory, non-inventory, service, kit, tool)."));
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .HasColumnType("smallint")
            .UseIdentityByDefaultColumn()
            .HasComment("Primary key.");
        entity.HasIndex(e => e.ItemTypeCode)
            .IsUnique()
            .HasDatabaseName("UQ_GloInventoryItemType_ItemTypeCode");
        entity.Property(e => e.ItemTypeCode).HasMaxLength(30)
            .HasComment("Unique item type code.");
        entity.Property(e => e.Name).HasMaxLength(50)
            .HasComment("Display name of the item type.");
        entity.Property(e => e.Description).HasColumnType("text")
            .HasComment("Description of the item type.");
        entity.Property(e => e.TracksQuantity).HasDefaultValue(false)
            .HasComment("Indicates whether items of this type track quantity on hand.");
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1)
            .HasComment("Display order.");
        entity.Property(e => e.IsActive).HasDefaultValue(true)
            .HasComment("Indicates whether the item type is active.");
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
    }
}
