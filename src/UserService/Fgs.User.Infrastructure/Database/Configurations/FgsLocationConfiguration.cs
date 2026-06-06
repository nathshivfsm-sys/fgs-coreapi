using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal class FgsLocationConfiguration : IEntityTypeConfiguration<FgsLocation>
{
    public void Configure(EntityTypeBuilder<FgsLocation> entity)
    {
        entity.ToTable("FgsLocation");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasColumnOrder(0);
        entity.Property(e => e.TenantId).HasColumnOrder(1);
        entity.Property(e => e.CompanyId).HasColumnOrder(2);
        entity.Property(e => e.AddressLine1).HasMaxLength(200);
        entity.Property(e => e.AddressLine2).HasMaxLength(200);
        entity.Property(e => e.AddressLine3).HasMaxLength(200);
        entity.Property(e => e.AddressLine4).HasMaxLength(200);
        entity.Property(e => e.City).HasMaxLength(100);
        entity.Property(e => e.State).HasMaxLength(100);
        entity.Property(e => e.County).HasMaxLength(100);
        entity.Property(e => e.Country).HasMaxLength(100);
        entity.Property(e => e.PostalCode).HasMaxLength(20);
        entity.Property(e => e.FormattedAddress).HasMaxLength(1000);
        entity.Property(e => e.Latitude).HasPrecision(18, 10);
        entity.Property(e => e.Longitude).HasPrecision(18, 10);
        entity.Property(e => e.PlaceId).HasMaxLength(500);
        entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.MasterEntityTypeId, e.EntityNumber })
            .HasDatabaseName("IX_FgsLocation_Tenant_Company_Entity");
    }
}
