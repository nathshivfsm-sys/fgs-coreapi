using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Persistence.Database.Configurations;

internal class GloInventoryItemTypeConfiguration : IEntityTypeConfiguration<GloInventoryItemType>
{
    public void Configure(EntityTypeBuilder<GloInventoryItemType> entity)
    {
        entity.ToTable("GloInventoryItemType");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .HasColumnType("smallint")
            .UseIdentityByDefaultColumn();
        entity.HasIndex(e => e.ItemTypeCode)
            .IsUnique()
            .HasDatabaseName("UQ_GloInventoryItemType_ItemTypeCode");
        entity.Property(e => e.ItemTypeCode).HasMaxLength(30);
        entity.Property(e => e.Name).HasMaxLength(50);
        entity.Property(e => e.Description).HasColumnType("text");
        entity.Property(e => e.TracksQuantity).HasDefaultValue(false);
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1);
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
    }
}
