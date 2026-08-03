using Fgs.Asset.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Asset.Infrastructure.Database.Configurations;

internal sealed class FgsAssetAttributeValueConfiguration : IEntityTypeConfiguration<FgsAssetAttributeValue>
{
    public void Configure(EntityTypeBuilder<FgsAssetAttributeValue> entity)
    {
        entity.ToTable(
            "FgsAssetAttributeValue",
            t =>
            {
                t.HasComment(
                    "Stores the values of custom attributes for individual assets. Each record contains the value of one attribute assigned to one asset.");
            });

        entity.HasKey(e => e.Id).HasName("PK_FgsAssetAttributeValue");
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.Id).HasComment("Unique asset attribute value identifier.");
        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");
        entity.Property(e => e.AssetId).HasComment("Asset that owns the attribute value.");
        entity.Property(e => e.AssetAttributeId).HasComment("Reference to the asset attribute definition.");
        entity.Property(e => e.OptionId)
            .HasComment("Selected option identifier when the attribute input type is DROPDOWN.");
        entity.Property(e => e.ValueText).HasMaxLength(500)
            .HasComment("Text value for TEXT or TEXTAREA attributes.");
        entity.Property(e => e.ValueInteger)
            .HasComment("Integer value for INTEGER attributes.");
        entity.Property(e => e.ValueDecimal).HasColumnType("numeric(18,4)")
            .HasComment("Decimal value for DECIMAL attributes.");
        entity.Property(e => e.ValueDate).HasColumnType("date")
            .HasComment("Date value for DATE attributes.");
        entity.Property(e => e.ValueBoolean)
            .HasComment("Boolean value for BOOLEAN attributes.");

        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).HasComment("User who created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz")
            .HasComment("Date and time the record was last updated.");
        entity.Property(e => e.UpdatedBy).HasComment("User who last updated the record.");

        entity.HasOne<Domain.Entities.FgsAsset>()
            .WithMany()
            .HasForeignKey(e => e.AssetId)
            .HasConstraintName("FK_FgsAssetAttributeValue_Asset")
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne<FgsAssetAttribute>()
            .WithMany()
            .HasForeignKey(e => e.AssetAttributeId)
            .HasConstraintName("FK_FgsAssetAttributeValue_Attribute")
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasIndex(e => e.AssetId)
            .HasDatabaseName("IX_FgsAssetAttributeValue_AssetId");

        entity.HasIndex(e => e.AssetAttributeId)
            .HasDatabaseName("IX_FgsAssetAttributeValue_AssetAttributeId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsAssetAttributeValue_TenantCompany");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.AssetId, e.AssetAttributeId })
            .IsUnique()
            .HasDatabaseName("UQ_FgsAssetAttributeValue_AssetAttribute");
    }
}
