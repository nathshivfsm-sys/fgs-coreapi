using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Persistence.Database.Configurations;

internal class GloStateProvinceConfiguration : IEntityTypeConfiguration<GloStateProvince>
{
    public void Configure(EntityTypeBuilder<GloStateProvince> entity)
    {
        entity.ToTable("GloStateProvince");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.Property(e => e.CountryCode).HasMaxLength(2);
        entity.Property(e => e.StateProvinceCode).HasMaxLength(10);
        entity.Property(e => e.StateProvinceName).HasMaxLength(100);
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.HasIndex(e => new { e.CountryCode, e.StateProvinceCode })
            .IsUnique()
            .HasDatabaseName("UQ_GloStateProvince");
        entity.HasOne(e => e.Country)
            .WithMany()
            .HasForeignKey(e => e.CountryCode)
            .HasConstraintName("FK_GloStateProvince_Country")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
