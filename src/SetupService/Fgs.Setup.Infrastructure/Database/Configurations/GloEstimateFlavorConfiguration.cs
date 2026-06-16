using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal sealed class GloEstimateFlavorConfiguration : IEntityTypeConfiguration<GloEstimateFlavor>
{
    public void Configure(EntityTypeBuilder<GloEstimateFlavor> entity)
    {
        entity.ToTable("GloEstimateFlavor");

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .HasColumnType("smallint")
            .UseIdentityByDefaultColumn();

        entity.Property(e => e.FlavorCode).HasMaxLength(50).IsRequired();
        entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
        entity.Property(e => e.BackgroundColor).HasMaxLength(20).IsRequired();
        entity.Property(e => e.TextColor).HasMaxLength(20).IsRequired();
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1);

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamp")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamp");

        entity.HasIndex(e => e.FlavorCode)
            .IsUnique()
            .HasDatabaseName("UX_GloEstimateFlavor_FlavorCode");
    }
}
