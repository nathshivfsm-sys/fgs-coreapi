using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class GloLeadStatusConfiguration : IEntityTypeConfiguration<GloLeadStatus>
{
    public void Configure(EntityTypeBuilder<GloLeadStatus> entity)
    {
        entity.ToTable("GloLeadStatus");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .HasColumnType("smallint")
            .UseIdentityByDefaultColumn();

        entity.Property(e => e.StatusCode).HasMaxLength(50).IsRequired();
        entity.Property(e => e.StatusName).HasMaxLength(100).IsRequired();
        entity.Property(e => e.Description).HasMaxLength(255);
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1);
        entity.Property(e => e.IsActive).HasDefaultValue(true);

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");

        entity.HasIndex(e => e.StatusCode)
            .IsUnique()
            .HasDatabaseName("UX_GloLeadStatus_StatusCode");

        entity.HasIndex(e => e.DisplayOrder)
            .HasDatabaseName("IX_GloLeadStatus_DisplayOrder");
    }
}
