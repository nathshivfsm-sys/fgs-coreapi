using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Persistence.Database.Configurations;

internal class GloLeadSourceConfiguration : IEntityTypeConfiguration<GloLeadSource>
{
    public void Configure(EntityTypeBuilder<GloLeadSource> entity)
    {
        entity.ToTable("GloLeadSource");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .HasColumnType("smallint")
            .UseIdentityByDefaultColumn();
        entity.HasIndex(e => e.SourceCode)
            .IsUnique()
            .HasDatabaseName("UX_GloLeadSource_SourceCode");
        entity.Property(e => e.SourceCode).HasMaxLength(50);
        entity.Property(e => e.SourceName).HasMaxLength(100);
        entity.Property(e => e.Description).HasMaxLength(255);
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
    }
}
