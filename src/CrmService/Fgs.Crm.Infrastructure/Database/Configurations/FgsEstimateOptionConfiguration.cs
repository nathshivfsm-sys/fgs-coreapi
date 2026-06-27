using Fgs.Crm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Crm.Infrastructure.Database.Configurations;

internal sealed class FgsEstimateOptionConfiguration : IEntityTypeConfiguration<FgsEstimateOption>
{
    public void Configure(EntityTypeBuilder<FgsEstimateOption> entity)
    {
        entity.ToTable(
            "FgsEstimateOption",
            t =>
            {
                t.HasComment(
                    "Stores sellable estimate options/packages belonging to an estimate. Detailed pricing is stored in crm.FgsEstimateOptionLine.");
                t.HasCheckConstraint("CK_FgsEstimateOption_DisplayOrder", "\"DisplayOrder\" > 0");
                t.HasCheckConstraint("CK_FgsEstimateOption_SubtotalAmount", "\"SubtotalAmount\" >= 0");
                t.HasCheckConstraint("CK_FgsEstimateOption_DiscountAmount", "\"DiscountAmount\" >= 0");
                t.HasCheckConstraint("CK_FgsEstimateOption_TaxAmount", "\"TaxAmount\" >= 0");
                t.HasCheckConstraint("CK_FgsEstimateOption_TotalAmount", "\"TotalAmount\" >= 0");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityAlwaysColumn().HasComment("Primary key.");
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");
        entity.Property(e => e.EstimateId).HasComment("Parent estimate.");
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1)
            .HasComment("Display order within the estimate.");
        entity.Property(e => e.OptionName).HasMaxLength(255).IsRequired()
            .HasComment("Customer-facing option name.");
        entity.Property(e => e.OptionDescription).HasColumnType("text")
            .HasComment("Detailed customer-facing option description.");
        entity.Property(e => e.IsRecommended).HasDefaultValue(false)
            .HasComment("Indicates whether the option is highlighted as the recommended option.");
        entity.Property(e => e.IsSelected).HasDefaultValue(false)
            .HasComment("Indicates whether the customer selected this option.");
        entity.Property(e => e.SelectedOn).HasColumnType("timestamptz")
            .HasComment("Date and time the option was selected by the customer.");
        entity.Property(e => e.SubtotalAmount).HasColumnType("numeric(18,2)").HasDefaultValue(0m)
            .HasComment("Option subtotal amount.");
        entity.Property(e => e.DiscountAmount).HasColumnType("numeric(18,2)").HasDefaultValue(0m)
            .HasComment("Option discount amount.");
        entity.Property(e => e.TaxAmount).HasColumnType("numeric(18,2)").HasDefaultValue(0m)
            .HasComment("Option tax amount.");
        entity.Property(e => e.TotalAmount).HasColumnType("numeric(18,2)").HasDefaultValue(0m)
            .HasComment("Option total amount.");
        entity.Property(e => e.InternalNotes).HasColumnType("text")
            .HasComment("Internal notes not visible to customers.");

        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).HasComment("User or process that created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz")
            .HasComment("Date and time the record was last updated.");
        entity.Property(e => e.UpdatedBy).HasComment("User or process that last updated the record.");

        entity.HasOne<FgsEstimate>()
            .WithMany()
            .HasForeignKey(e => e.EstimateId)
            .HasConstraintName("FK_FgsEstimateOption_Estimate")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsEstimateOption_TenantId_CompanyId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.EstimateId })
            .HasDatabaseName("IX_FgsEstimateOption_TenantId_CompanyId_EstimateId");
    }
}
