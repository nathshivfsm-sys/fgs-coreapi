using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal sealed class GloResolutionTypeCacheConfiguration : IEntityTypeConfiguration<GloResolutionTypeCache>
{
    public void Configure(EntityTypeBuilder<GloResolutionTypeCache> entity)
    {
        entity.ToTable(
            "GloResolutionTypeCache",
            t => t.HasComment(
                "Local cache of globally defined resolution types used to eliminate cross-schema dependencies."));

        entity.HasKey(e => e.ResolutionTypeId)
            .HasName("PK_GloResolutionTypeCache");

        entity.Property(e => e.ResolutionTypeId)
            .ValueGeneratedNever()
            .HasComment("Identifier from glo.GloResolutionType.Id.");
        entity.Property(e => e.ResolutionTypeCode).HasMaxLength(50)
            .HasComment("System unique resolution type code.");
        entity.Property(e => e.ResolutionTypeName).HasMaxLength(200)
            .HasComment("User friendly resolution type name displayed in setup screens.");
        entity.Property(e => e.IsActive)
            .HasComment("Indicates whether the resolution type can be used for new configurations.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz")
            .HasComment("Timestamp of the most recent synchronization from glo.GloResolutionType.");

        entity.HasIndex(e => e.ResolutionTypeCode)
            .IsUnique()
            .HasDatabaseName("UQ_GloResolutionTypeCache_ResolutionTypeCode");

        entity.HasIndex(e => e.ResolutionTypeName)
            .HasDatabaseName("IX_GloResolutionTypeCache_ResolutionTypeName");
    }
}
