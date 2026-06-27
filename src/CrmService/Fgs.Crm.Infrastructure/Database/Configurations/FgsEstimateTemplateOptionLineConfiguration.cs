using Fgs.Crm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Crm.Infrastructure.Database.Configurations;

internal sealed class FgsEstimateTemplateOptionLineConfiguration : IEntityTypeConfiguration<FgsEstimateTemplateOptionLine>
{
    public void Configure(EntityTypeBuilder<FgsEstimateTemplateOptionLine> entity)
    {
        entity.ToTable(
            "FgsEstimateTemplateOptionLine",
            t =>
            {
                t.HasComment(
                    "Stores detailed pricing lines belonging to an estimate template option and are copied into estimate option lines when a template is applied.");
                t.HasCheckConstraint("CK_FgsEstimateTemplateOptionLine_DisplayOrder", "\"DisplayOrder\" > 0");
                t.HasCheckConstraint("CK_FgsEstimateTemplateOptionLine_Quantity", "\"Quantity\" >= 0");
                t.HasCheckConstraint("CK_FgsEstimateTemplateOptionLine_UnitCost", "\"UnitCost\" >= 0");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityAlwaysColumn().HasComment("Primary key.");
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");
        entity.Property(e => e.EstimateTemplateOptionId).HasComment("Parent estimate template option.");
        entity.Property(e => e.ParentLineId).HasComment(
            "Parent template option line used for service breakdowns, bundles, discounts, rebates, credits, and other hierarchical pricing structures.");
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1)
            .HasComment("Display sequence within the template option.");
        entity.Property(e => e.BillingCategoryId).HasComment(
            "Billing category such as Material, Labor, Service, Equipment, Discount, Tax, or Other.");
        entity.Property(e => e.ItemId).HasComment("Item associated with the line.");
        entity.Property(e => e.RateOfDayId).HasComment(
            "Rate of day applied to labor pricing such as Regular, Overtime, Double Time, Weekend, Holiday, or Emergency.");
        entity.Property(e => e.Description).HasMaxLength(500).IsRequired()
            .HasComment("Customer-facing description or tax authority name.");
        entity.Property(e => e.ShowOnProposal).HasDefaultValue(true)
            .HasComment("Indicates whether the line should be displayed on customer-facing proposals.");
        entity.Property(e => e.ShowPriceOnProposal).HasDefaultValue(true)
            .HasComment("Indicates whether pricing amounts should be displayed on customer-facing proposals.");
        entity.Property(e => e.AllowQuantityChange).HasDefaultValue(true)
            .HasComment("Indicates whether quantity may be modified after template application.");
        entity.Property(e => e.AllowPriceChange).HasDefaultValue(true)
            .HasComment("Indicates whether pricing may be modified after template application.");
        entity.Property(e => e.Source).HasMaxLength(50)
            .HasComment("Identifies where the line originated such as Manual, ServiceItem, PricingMatrix, Bundle, Import, or Clone.");
        entity.Property(e => e.Quantity).HasColumnType("numeric(18,4)").HasDefaultValue(1m)
            .HasComment("Default quantity applied when template is used.");
        entity.Property(e => e.UnitCost).HasColumnType("numeric(18,2)").HasDefaultValue(0m)
            .HasComment("Default cost per unit.");
        entity.Property(e => e.ExtendedCost).HasColumnType("numeric(18,2)").HasDefaultValue(0m)
            .HasComment("Quantity multiplied by UnitCost.");
        entity.Property(e => e.UnitPrice).HasColumnType("numeric(18,2)").HasDefaultValue(0m)
            .HasComment("Default selling price per unit.");
        entity.Property(e => e.ExtendedPrice).HasColumnType("numeric(18,2)").HasDefaultValue(0m)
            .HasComment("Quantity multiplied by UnitPrice.");

        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).HasComment("User or process that created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz")
            .HasComment("Date and time the record was last updated.");
        entity.Property(e => e.UpdatedBy).HasComment("User or process that last updated the record.");

        entity.HasOne<FgsEstimateTemplateOption>()
            .WithMany()
            .HasForeignKey(e => e.EstimateTemplateOptionId)
            .HasConstraintName("FK_FgsEstimateTemplateOptionLine_EstimateTemplateOption")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne<FgsEstimateTemplateOptionLine>()
            .WithMany()
            .HasForeignKey(e => e.ParentLineId)
            .HasConstraintName("FK_FgsEstimateTemplateOptionLine_ParentLine")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsEstimateTemplateOptionLine_TenantId_CompanyId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.EstimateTemplateOptionId })
            .HasDatabaseName("IX_FgsEstimateTemplateOptionLine_TenantId_CompanyId_EstimateTemplateOptionId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ParentLineId })
            .HasDatabaseName("IX_FgsEstimateTemplateOptionLine_TenantId_CompanyId_ParentLineId");
    }
}
