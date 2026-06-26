using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class GloTagConfiguration : IEntityTypeConfiguration<GloTag>
{
    public void Configure(EntityTypeBuilder<GloTag> entity)
    {
        entity.ToTable("GloTag");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .UseIdentityAlwaysColumn();
        entity.HasIndex(e => e.TagCode)
            .IsUnique()
            .HasDatabaseName("UX_GloTag_TagCode");
        entity.HasIndex(e => e.NormalizedName)
            .IsUnique()
            .HasDatabaseName("UX_GloTag_NormalizedName");
        entity.HasIndex(e => e.Name)
            .HasDatabaseName("IX_GloTag_Name");
        entity.HasIndex(e => e.DisplayOrder)
            .HasDatabaseName("IX_GloTag_DisplayOrder");
        entity.HasIndex(e => e.IsActive)
            .HasDatabaseName("IX_GloTag_IsActive");
        entity.HasIndex(e => e.IconFileId)
            .HasDatabaseName("IX_GloTag_IconFileId");
        entity.Property(e => e.TagCode).HasMaxLength(50);
        entity.Property(e => e.Name).HasMaxLength(100);
        entity.Property(e => e.NormalizedName).HasMaxLength(100);
        entity.Property(e => e.BackgroundColor).HasMaxLength(20);
        entity.Property(e => e.TextColor).HasMaxLength(20);
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1);
        entity.Property(e => e.IsSystemGenerated).HasDefaultValue(true);
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        entity.HasIndex(e => e.IconFileId).HasDatabaseName("IX_GloTag_IconFileId");
    }
}
