using Fgs.Crm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Crm.Infrastructure.Database.Configurations;

internal sealed class FgsEstimateOptionLineConfiguration : IEntityTypeConfiguration<FgsEstimateOptionLine>
{
    public void Configure(EntityTypeBuilder<FgsEstimateOptionLine> entity)
    {
        entity.ToTable(
            "FgsEstimateOptionLine",
            t =>
            {
                t.HasComment(
                    "Stores detailed pricing lines belonging to an estimate option. Supports materials, labor, services, discounts, taxes, fees, and hierarchical pricing structures.");
                t.HasCheckConstraint("CK_FgsEstimateOptionLine_Quantity", "\"Quantity\" >= 0");
                t.HasCheckConstraint("CK_FgsEstimateOptionLine_UnitCost", "\"UnitCost\" >= 0");
                t.HasCheckConstraint("CK_FgsEstimateOptionLine_UnitPrice", "\"UnitPrice\" >= 0");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityAlwaysColumn().HasComment("Primary key.");
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");
        entity.Property(e => e.EstimateOptionId).HasComment("Parent estimate option.");
        entity.Property(e => e.ParentLineId).HasComment(
            "Parent estimate option line used for service breakdowns, discounts, taxes, bundles, rebates, and other hierarchical structures.");
        entity.Property(e => e.TemplateId).HasComment("Source estimate template.");
        entity.Property(e => e.TemplateLineId).HasComment("Source estimate template option line.");
        entity.Property(e => e.DisplayOrder).HasDefaultValue(1)
            .HasComment("Display sequence within the estimate option.");
        entity.Property(e => e.BillingCategoryId).HasComment(
            "Billing category such as Material, Labor, Service, Equipment, Discount, Tax, Fee, or Other.");
        entity.Property(e => e.ItemCode).HasMaxLength(100)
            .HasComment("Associated item identifier. May represent inventory, non-inventory, service, labor, fee, or miscellaneous items.");
        entity.Property(e => e.RateOfDayId).HasComment(
            "Rate of day applied to labor pricing such as Regular, Overtime, Double Time, Weekend, Holiday, or Emergency.");
        entity.Property(e => e.Description).HasColumnType("text").IsRequired()
            .HasComment("Customer-facing description, service description, tax authority name, or other detail text.");
        entity.Property(e => e.ShowOnProposal).HasDefaultValue(true)
            .HasComment("Indicates whether the line should be displayed on customer-facing proposal documents.");
        entity.Property(e => e.ShowPriceOnProposal).HasDefaultValue(true)
            .HasComment("Indicates whether price and amount should be displayed on customer-facing proposal documents.");
        entity.Property(e => e.ShowToFieldTechnician).HasDefaultValue(true)
            .HasComment("Indicates whether the line should be visible to field technicians.");
        entity.Property(e => e.Source).HasMaxLength(100)
            .HasComment("Indicates where the line originated such as Manual, Template, ServiceItem, PricingMatrix, Bundle, Import, Clone, or System.");
        entity.Property(e => e.Quantity).HasColumnType("numeric(18,4)").HasDefaultValue(1m)
            .HasComment("Quantity associated with the line.");
        entity.Property(e => e.UnitCost).HasColumnType("numeric(18,4)").HasDefaultValue(0m)
            .HasComment("Cost per unit.");
        entity.Property(e => e.ExtendedCost).HasColumnType("numeric(18,2)").HasDefaultValue(0m)
            .HasComment("Quantity multiplied by UnitCost.");
        entity.Property(e => e.UnitPrice).HasColumnType("numeric(18,4)").HasDefaultValue(0m)
            .HasComment("Selling price per unit.");
        entity.Property(e => e.ExtendedPrice).HasColumnType("numeric(18,2)").HasDefaultValue(0m)
            .HasComment("Quantity multiplied by UnitPrice.");

        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).HasComment("User or process that created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz")
            .HasComment("Date and time the record was last updated.");
        entity.Property(e => e.UpdatedBy).HasComment("User or process that last updated the record.");

        entity.HasOne<FgsEstimateOption>()
            .WithMany()
            .HasForeignKey(e => e.EstimateOptionId)
            .HasConstraintName("FK_FgsEstimateOptionLine_EstimateOption")
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne<FgsEstimateOptionLine>()
            .WithMany()
            .HasForeignKey(e => e.ParentLineId)
            .HasConstraintName("FK_FgsEstimateOptionLine_ParentLine")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsEstimateOptionLine_TenantId_CompanyId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.EstimateOptionId })
            .HasDatabaseName("IX_FgsEstimateOptionLine_TenantId_CompanyId_EstimateOptionId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ParentLineId })
            .HasDatabaseName("IX_FgsEstimateOptionLine_TenantId_CompanyId_ParentLineId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.EstimateOptionId, e.DisplayOrder })
            .HasDatabaseName("IX_FgsEstimateOptionLine_TenantId_CompanyId_DisplayOrder");
    }
}
