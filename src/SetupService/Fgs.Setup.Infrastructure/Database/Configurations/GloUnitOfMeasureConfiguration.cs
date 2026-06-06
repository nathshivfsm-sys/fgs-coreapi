using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class GloUnitOfMeasureConfiguration : IEntityTypeConfiguration<GloUnitOfMeasure>
{
    public void Configure(EntityTypeBuilder<GloUnitOfMeasure> entity)
    {
        entity.ToTable("GloUnitOfMeasure");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .UseIdentityByDefaultColumn();
        entity.HasIndex(e => e.UnitCode)
            .IsUnique()
            .HasDatabaseName("UQ_GloUnitOfMeasure_UnitCode");
        entity.HasIndex(e => e.UnitType)
            .HasDatabaseName("IX_GloUnitOfMeasure_UnitType");
        entity.Property(e => e.UnitCode).HasMaxLength(30);
        entity.Property(e => e.Name).HasMaxLength(100);
        entity.Property(e => e.Abbreviation).HasMaxLength(20);
        entity.Property(e => e.UnitType).HasMaxLength(50);
        entity.Property(e => e.DecimalPlaces).HasDefaultValue((short)2);
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1);
        entity.Property(e => e.IsSystem).HasDefaultValue(true);
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
    }
}
