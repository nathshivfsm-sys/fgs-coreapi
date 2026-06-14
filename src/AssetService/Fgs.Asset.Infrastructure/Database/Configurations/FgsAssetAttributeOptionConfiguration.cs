using Fgs.Asset.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Asset.Infrastructure.Database.Configurations;

internal sealed class FgsAssetAttributeOptionConfiguration : IEntityTypeConfiguration<FgsAssetAttributeOption>
{
    public void Configure(EntityTypeBuilder<FgsAssetAttributeOption> entity)
    {
        entity.ToTable(
            "FgsAssetAttributeOption",
            t =>
            {
                t.HasComment("Stores selectable dropdown values for asset attributes.");
                t.HasCheckConstraint(
                    "CK_FgsAssetAttributeOption_OptionCode_Upper",
                    "\"OptionCode\" = upper(\"OptionCode\")");
            });

        entity.HasKey(e => e.Id).HasName("PK_FgsAssetAttributeOption");
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.Id).HasComment("Unique asset attribute option identifier.");
        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");
        entity.Property(e => e.AssetAttributeId).HasComment("Asset attribute definition that owns this option.");
        entity.Property(e => e.OptionCode).HasMaxLength(75).IsRequired()
            .HasComment("Unique option code within the asset attribute. Stored in uppercase.");
        entity.Property(e => e.OptionName).HasMaxLength(200).IsRequired()
            .HasComment("Display name shown to users.");
        entity.Property(e => e.DisplayOrder).HasDefaultValue(0)
            .HasComment("Controls the order in which options are displayed to users.");
        entity.Property(e => e.IsActive).HasDefaultValue(true)
            .HasComment("Indicates whether the option is available for selection on new or updated assets.");

        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).HasComment("User who created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz")
            .HasComment("Date and time the record was last updated.");
        entity.Property(e => e.UpdatedBy).HasComment("User who last updated the record.");

        entity.HasOne<FgsAssetAttribute>()
            .WithMany()
            .HasForeignKey(e => e.AssetAttributeId)
            .HasConstraintName("FK_FgsAssetAttributeOption_AssetAttribute")
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.AssetAttributeId, e.OptionCode })
            .IsUnique()
            .HasDatabaseName("UQ_FgsAssetAttributeOption_Code");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.AssetAttributeId, e.OptionName })
            .IsUnique()
            .HasDatabaseName("UQ_FgsAssetAttributeOption_Name");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsAssetAttributeOption_TenantId_CompanyId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.AssetAttributeId })
            .HasDatabaseName("IX_FgsAssetAttributeOption_TenantId_CompanyId_AssetAttributeId");
    }
}
