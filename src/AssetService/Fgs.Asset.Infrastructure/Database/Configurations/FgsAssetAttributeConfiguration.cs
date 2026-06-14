using Fgs.Asset.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Asset.Infrastructure.Database.Configurations;

internal sealed class FgsAssetAttributeConfiguration : IEntityTypeConfiguration<FgsAssetAttribute>
{
    public void Configure(EntityTypeBuilder<FgsAssetAttribute> entity)
    {
        entity.ToTable(
            "FgsAssetAttribute",
            t =>
            {
                t.HasComment(
                    "Defines custom asset attributes that can be assigned to specific asset types.");
                t.HasCheckConstraint(
                    "CK_FgsAssetAttribute_AttributeCode_Upper",
                    "\"AttributeCode\" = upper(\"AttributeCode\")");
                t.HasCheckConstraint(
                    "CK_FgsAssetAttribute_InputType_Upper",
                    "\"InputType\" = upper(\"InputType\")");
            });

        entity.HasKey(e => e.Id).HasName("PK_FgsAssetAttribute");
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.Id).HasComment("Unique asset attribute identifier.");
        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");
        entity.Property(e => e.AssetTypeId).HasComment("Asset type that owns this attribute definition.");
        entity.Property(e => e.AttributeCode).HasMaxLength(75).IsRequired()
            .HasComment("Unique attribute code within the asset type. Stored in uppercase.");
        entity.Property(e => e.AttributeName).HasMaxLength(200).IsRequired()
            .HasComment("Display name shown to users.");
        entity.Property(e => e.InputType).HasMaxLength(25).IsRequired()
            .HasComment("Input type. Valid values: TEXT, TEXTAREA, INTEGER, DECIMAL, DATE, BOOLEAN, DROPDOWN.");
        entity.Property(e => e.DefaultOptionId)
            .HasComment("Default dropdown option when InputType is DROPDOWN.");
        entity.Property(e => e.DefaultValueText).HasMaxLength(500)
            .HasComment("Default text value.");
        entity.Property(e => e.DefaultValueInteger)
            .HasComment("Default integer value.");
        entity.Property(e => e.DefaultValueDecimal).HasColumnType("numeric(18,4)")
            .HasComment("Default decimal value.");
        entity.Property(e => e.DefaultValueDate).HasColumnType("date")
            .HasComment("Default date value.");
        entity.Property(e => e.DefaultValueBoolean)
            .HasComment("Default boolean value.");
        entity.Property(e => e.IsRequired).HasDefaultValue(false)
            .HasComment("Indicates whether a value must be supplied when creating or updating an asset.");
        entity.Property(e => e.IsSearchable).HasDefaultValue(true)
            .HasComment("Indicates whether the attribute should be available in asset search and filtering screens.");
        entity.Property(e => e.DisplayOrder).HasDefaultValue(0)
            .HasComment("Controls the display order of attributes within the asset type.");
        entity.Property(e => e.IsActive).HasDefaultValue(true)
            .HasComment("Indicates whether the attribute definition is active.");

        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).HasComment("User who created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz")
            .HasComment("Date and time the record was last updated.");
        entity.Property(e => e.UpdatedBy).HasComment("User who last updated the record.");

        entity.HasOne<FgsAssetType>()
            .WithMany()
            .HasForeignKey(e => e.AssetTypeId)
            .HasConstraintName("FK_FgsAssetAttribute_AssetType")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.AssetTypeId, e.AttributeCode })
            .IsUnique()
            .HasDatabaseName("UQ_FgsAssetAttribute_TenantCompanyAssetTypeCode");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsAssetAttribute_TenantId_CompanyId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.AssetTypeId })
            .HasDatabaseName("IX_FgsAssetAttribute_TenantId_CompanyId_AssetTypeId");
    }
}
