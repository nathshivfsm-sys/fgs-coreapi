using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsSourceConfiguration : IEntityTypeConfiguration<FgsSource>
{
    public void Configure(EntityTypeBuilder<FgsSource> entity)
    {
        entity.ToTable("FgsSource");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.SourceCode })
            .IsUnique()
            .HasDatabaseName("UX_FgsSource_TenantId_CompanyId_SourceCode");
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
