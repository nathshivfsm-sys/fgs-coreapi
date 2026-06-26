using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class GloZoneConfiguration : IEntityTypeConfiguration<GloZone>
{
    public void Configure(EntityTypeBuilder<GloZone> entity)
    {
        entity.ToTable("GloZone");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .HasColumnType("smallint")
            .UseIdentityAlwaysColumn();
        entity.HasIndex(e => e.Code)
            .IsUnique()
            .HasDatabaseName("UQ_GloZone_Code");
        entity.Property(e => e.Code).HasMaxLength(50);
        entity.Property(e => e.Name).HasMaxLength(100);
        entity.Property(e => e.Description).HasMaxLength(255);
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        entity.ToTable(t => t.HasCheckConstraint("CK_GloZone_Code_Upper", "\"Code\" = upper(\"Code\")"));
    }
}
