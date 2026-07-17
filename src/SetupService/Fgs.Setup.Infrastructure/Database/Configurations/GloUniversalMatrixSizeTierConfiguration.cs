using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class GloUniversalMatrixSizeTierConfiguration : IEntityTypeConfiguration<GloUniversalMatrixSizeTier>
{
    public void Configure(EntityTypeBuilder<GloUniversalMatrixSizeTier> entity)
    {
        entity.ToTable("GloUniversalMatrixSizeTier", t =>
            t.HasComment("Global service size tiers and their default pricing multipliers."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();

        entity.Property(e => e.UniversalPricingServiceId)
            .HasColumnType("smallint")
            .HasComment("Reference to the global universal pricing service.");

        entity.Property(e => e.Name)
            .HasMaxLength(100)
            .IsRequired();

        entity.Property(e => e.Multiplier)
            .HasPrecision(8, 4)
            .HasDefaultValue(1.0000m)
            .HasComment("Multiplier applied to calculated service pricing for this size tier.");

        entity.Property(e => e.DisplayOrder)
            .HasDefaultValue((short)1);

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");

        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");

        entity.HasIndex(e => e.UniversalPricingServiceId)
            .HasDatabaseName("IX_GloUniversalMatrixSizeTier_UniversalPricingServiceId");

        entity.HasIndex(e => new { e.UniversalPricingServiceId, e.Name })
            .IsUnique()
            .HasDatabaseName("UX_GloUniversalMatrixSizeTier_ServiceId_Name");

        entity.HasOne<GloUniversalPricingService>()
            .WithMany()
            .HasForeignKey(e => e.UniversalPricingServiceId)
            .HasConstraintName("FK_GloUniversalMatrixSizeTier_GloUniversalPricingService_UniversalPricingServiceId")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
