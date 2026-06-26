using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class GloTitleOfCourtesyConfiguration : IEntityTypeConfiguration<GloTitleOfCourtesy>
{
    public void Configure(EntityTypeBuilder<GloTitleOfCourtesy> entity)
    {
        entity.ToTable("GloTitleOfCourtesy");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.HasIndex(e => e.Code)
            .IsUnique()
            .HasDatabaseName("UQ_GloTitleOfCourtesy_Code");
        entity.HasIndex(e => e.SortOrder)
            .HasDatabaseName("IX_GloTitleOfCourtesy_SortOrder");
        entity.Property(e => e.Code).HasMaxLength(25);
        entity.Property(e => e.DisplayName).HasMaxLength(100);
        entity.Property(e => e.SortOrder).HasDefaultValue(0);
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.CreatedBy).HasMaxLength(100);
        entity.Property(e => e.UpdatedBy).HasMaxLength(100);
        entity.ToTable(t =>
        {
            t.HasCheckConstraint("CK_GloTitleOfCourtesy_Code_Upper", "\"Code\" = upper(\"Code\")");
            t.HasCheckConstraint("CK_GloTitleOfCourtesy_SortOrder", "\"SortOrder\" >= 0");
        });
    }
}
