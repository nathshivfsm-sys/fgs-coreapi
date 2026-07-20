using Fgs.Audit.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Audit.Infrastructure.Database.Configurations;

internal sealed class FgsArchiveCatalogConfiguration : IEntityTypeConfiguration<FgsArchiveCatalog>
{
    public void Configure(EntityTypeBuilder<FgsArchiveCatalog> entity)
    {
        entity.ToTable(
            "FgsArchiveCatalog",
            t => t.HasComment("Maintains an inventory of archived event partitions."));

        entity.HasKey(e => e.Id).HasName("PK_FgsArchiveCatalog");
        entity.Property(e => e.Id)
            .UseIdentityByDefaultColumn()
            .HasIdentityOptions(startValue: 1, incrementBy: 1)
            .HasComment("Unique identifier of the archive record.");

        entity.Property(e => e.ArchiveMonth)
            .HasColumnType("date")
            .IsRequired()
            .HasComment("First day of the month represented by the archived partition (for example, 2026-07-01).");
        entity.Property(e => e.StoragePath).HasMaxLength(1000).IsRequired()
            .HasComment("Object key or path where the archive file is stored.");
        entity.Property(e => e.FileSize).IsRequired()
            .HasComment("Size of the archive file in bytes.");
        entity.Property(e => e.CreatedOn)
            .HasColumnType("timestamp")
            .HasDefaultValueSql("now()")
            .IsRequired()
            .HasComment("Date and time the archive record was created.");

        entity.HasIndex(e => e.ArchiveMonth)
            .IsUnique()
            .HasDatabaseName("IX_FgsArchiveCatalog_ArchiveMonth");
    }
}
