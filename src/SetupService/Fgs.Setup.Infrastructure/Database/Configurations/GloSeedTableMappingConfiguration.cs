using Fgs.Persistence.Extensions;
using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class GloSeedTableMappingConfiguration : IEntityTypeConfiguration<GloSeedTableMapping>
{
    public void Configure(EntityTypeBuilder<GloSeedTableMapping> entity)
    {
        entity.ToTable("GloSeedTableMapping");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .HasColumnType("bigint")
            .UseIdentityByDefaultColumn();

        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.ConfigureGloEntityAuditColumns();

        entity.Property(e => e.SeedCode).HasMaxLength(100);
        entity.HasIndex(e => e.SeedCode)
            .HasDatabaseName("IX_GloSeedTableMapping_SeedCode");

        entity.HasIndex(e => new { e.SeedCode, e.TargetTableName })
            .IsUnique()
            .HasDatabaseName("UX_GloSeedTableMapping_SeedCode_TargetTableName");

        entity.Property(e => e.SourceDatabaseName).HasMaxLength(150);
        entity.Property(e => e.SourceSchemaName)
            .HasMaxLength(100)
            .HasDefaultValue("public");
        entity.Property(e => e.SourceTableName).HasMaxLength(150);
        entity.Property(e => e.TargetDatabaseName).HasMaxLength(150);
        entity.Property(e => e.TargetSchemaName)
            .HasMaxLength(100)
            .HasDefaultValue("public");
        entity.Property(e => e.TargetTableName).HasMaxLength(150);
        entity.Property(e => e.Description).HasColumnType("text");

        entity.HasIndex(e => e.SeedOrder)
            .HasDatabaseName("IX_GloSeedTableMapping_SeedOrder");
    }
}
