using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsTagConfiguration : IEntityTypeConfiguration<FgsTag>
{
    public void Configure(EntityTypeBuilder<FgsTag> entity)
    {
        entity.ToTable("FgsTag");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        entity.Property(e => e.TagCode).HasMaxLength(50);
        entity.Property(e => e.Name).HasMaxLength(100);
        entity.Property(e => e.NormalizedName).HasMaxLength(100);
        entity.Property(e => e.BackgroundColor).HasMaxLength(20);
        entity.Property(e => e.TextColor).HasMaxLength(20);
        entity.Property(e => e.UsageCount).HasDefaultValue(0);
        entity.Property(e => e.IsSystemGenerated).HasDefaultValue(false);
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.CreatedBy).HasMaxLength(100);
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.UpdatedBy).HasMaxLength(100);
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.NormalizedName })
            .IsUnique()
            .HasDatabaseName("UX_FgsTag_TenantId_CompanyId_NormalizedName");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.TagCode })
            .IsUnique()
            .HasDatabaseName("UX_FgsTag_TenantId_CompanyId_TagCode")
            .HasFilter("\"TagCode\" IS NOT NULL");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Name })
            .HasDatabaseName("IX_FgsTag_Name");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsActive })
            .HasDatabaseName("IX_FgsTag_IsActive");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.UsageCount })
            .IsDescending(false, false, true)
            .HasDatabaseName("IX_FgsTag_UsageCount");
        entity.HasIndex(e => e.IconFileId)
            .HasDatabaseName("IX_FgsTag_IconFileId");
    }
}
