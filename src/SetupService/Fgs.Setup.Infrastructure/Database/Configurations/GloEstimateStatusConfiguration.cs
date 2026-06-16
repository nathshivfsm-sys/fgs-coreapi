using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal sealed class GloEstimateStatusConfiguration : IEntityTypeConfiguration<GloEstimateStatus>
{
    public void Configure(EntityTypeBuilder<GloEstimateStatus> entity)
    {
        entity.ToTable(
            "GloEstimateStatus",
            t => t.HasComment(
                "Stores system-defined estimate statuses used to seed tenant/company estimate statuses during provisioning."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .HasColumnType("smallint")
            .UseIdentityByDefaultColumn();

        entity.Property(e => e.StatusCode).HasMaxLength(50).IsRequired();
        entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1);

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamp")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamp");

        entity.HasIndex(e => e.StatusCode)
            .IsUnique()
            .HasDatabaseName("UX_GloEstimateStatus_StatusCode");
    }
}
