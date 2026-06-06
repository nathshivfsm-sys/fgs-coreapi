using Fgs.Persistence.Extensions;
using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class GloSeedTableColumnMappingConfiguration : IEntityTypeConfiguration<GloSeedTableColumnMapping>
{
    public void Configure(EntityTypeBuilder<GloSeedTableColumnMapping> entity)
    {
        entity.ToTable("GloSeedTableColumnMapping");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .HasColumnType("bigint")
            .UseIdentityByDefaultColumn();

        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.ConfigureGloEntityAuditColumns();

        entity.Property(e => e.SeedTableMappingId).HasColumnType("bigint");
        entity.Property(e => e.SourceColumnName).HasMaxLength(150);
        entity.Property(e => e.TargetColumnName).HasMaxLength(150);
        entity.Property(e => e.TransformationType).HasMaxLength(50);
        entity.Property(e => e.StaticValue).HasColumnType("text");
        entity.Property(e => e.ColumnOrder).HasDefaultValue(0);
        entity.Property(e => e.IsRequired).HasDefaultValue(false);
        entity.Property(e => e.Description).HasColumnType("text");

        entity.HasIndex(e => e.SeedTableMappingId)
            .HasDatabaseName("IX_GloSeedTableColumnMapping_SeedTableMappingId");

        entity.HasOne(e => e.SeedTableMapping)
            .WithMany(m => m.ColumnMappings)
            .HasForeignKey(e => e.SeedTableMappingId)
            .HasConstraintName("FK_GloSeedTableColumnMapping_GloSeedTableMapping")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
