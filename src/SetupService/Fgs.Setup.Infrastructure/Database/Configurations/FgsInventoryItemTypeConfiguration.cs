using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsInventoryItemTypeConfiguration : IEntityTypeConfiguration<FgsInventoryItemType>
{
    public void Configure(EntityTypeBuilder<FgsInventoryItemType> entity)
    {
        entity.ToTable("FgsInventoryItemType");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns();
        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.ItemTypeCode })
            .HasName("UQ_FgsInventoryItemType_TenantId_CompanyId_ItemTypeCode");
        entity.Property(e => e.ItemTypeCode).HasMaxLength(30);
        entity.Property(e => e.Name).HasMaxLength(50);
        entity.Property(e => e.Description).HasColumnType("text");
        entity.Property(e => e.TracksQuantity).HasDefaultValue(false);
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1);
        entity.Property(e => e.IsSystem).HasDefaultValue(false);
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
    }
}
