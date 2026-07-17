using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class GloUniversalPricingServiceConfiguration : IEntityTypeConfiguration<GloUniversalPricingService>
{
    public void Configure(EntityTypeBuilder<GloUniversalPricingService> entity)
    {
        entity.ToTable("GloUniversalPricingService", t =>
            t.HasComment("Global seeded list of services supported by the Universal Pricing Matrix."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .HasColumnType("smallint")
            .UseIdentityByDefaultColumn();

        entity.Property(e => e.ServiceCode)
            .HasMaxLength(50)
            .IsRequired()
            .HasComment("Stable system code used to identify the universal pricing service across domains.");

        entity.Property(e => e.Name)
            .HasMaxLength(100)
            .IsRequired()
            .HasComment("User-facing service name.");

        entity.Property(e => e.Description)
            .HasColumnType("text")
            .HasComment("Optional description of the universal pricing service.");

        entity.Property(e => e.DisplayOrder)
            .HasDefaultValue((short)1);

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");

        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");

        entity.HasIndex(e => e.ServiceCode)
            .IsUnique()
            .HasDatabaseName("IX_GloUniversalPricingService_ServiceCode");

        entity.HasIndex(e => e.Name)
            .HasDatabaseName("IX_GloUniversalPricingService_Name");
    }
}
