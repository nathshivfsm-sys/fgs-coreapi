using Fgs.File.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.File.Infrastructure.Database.Configurations;

internal class FgsFileConfiguration : IEntityTypeConfiguration<FgsFile>
{
    public void Configure(EntityTypeBuilder<FgsFile> entity)
    {
        entity.ToTable("FgsFile");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        entity.ConfigureTenantCompanySetupColumns(tenantCompanyIndexName: "IX_FgsFile_TenantId_CompanyId");
        entity.Ignore(e => e.IsActive);
        entity.Property(e => e.CreatedOn).HasDefaultValueSql("now()");
        entity.Property(e => e.EntityType).HasMaxLength(50);
        entity.Property(e => e.BucketName).HasMaxLength(255);
        entity.Property(e => e.ObjectKey).HasMaxLength(2000);
        entity.Property(e => e.ThumbnailObjectKey).HasMaxLength(2000);
        entity.Property(e => e.OriginalFileName).HasMaxLength(500);
        entity.Property(e => e.StoredFileName).HasMaxLength(500);
        entity.Property(e => e.ContentType).HasMaxLength(255);
        entity.Property(e => e.FileExtension).HasMaxLength(20);
        entity.Property(e => e.Description).HasColumnType("text");
        entity.Property(e => e.Tags).HasColumnType("text[]");
        entity.Property(e => e.UploadedByName).HasMaxLength(255);
        entity.Property(e => e.UploadedByType).HasMaxLength(50);
        entity.Property(e => e.IsVisibleToCustomer).HasDefaultValue(true);
        entity.Property(e => e.IsVisibleToFieldTechnician).HasDefaultValue(true);

        entity.HasIndex(e => new { e.BucketName, e.ObjectKey })
            .IsUnique()
            .HasDatabaseName("UX_FgsFile_Bucket_ObjectKey");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.EntityType, e.EntityId })
            .HasDatabaseName("IX_FgsFile_Entity");

        entity.HasIndex(e => e.Tags)
            .HasDatabaseName("IX_FgsFile_Tags")
            .HasMethod("gin");
    }
}
