using Fgs.Crm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Crm.Infrastructure.Database.Configurations;

internal sealed class FgsEstimateConfiguration : IEntityTypeConfiguration<FgsEstimate>
{
    public void Configure(EntityTypeBuilder<FgsEstimate> entity)
    {
        entity.ToTable(
            "FgsEstimate",
            t => t.HasComment(
                "Stores estimate header information and pricing totals for the selected/sold estimate option."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityAlwaysColumn().HasComment("Primary key.");
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");
        entity.Property(e => e.EstimateNumber).HasMaxLength(50).IsRequired()
            .HasComment("User-facing estimate number.");
        entity.Property(e => e.EstimateStatusId).HasComment("Current estimate status.");
        entity.Property(e => e.EstimateTypeId).HasComment(
            "Estimate presentation style such as Single Option or Good Better Best.");
        entity.Property(e => e.EstimateSourceId).HasComment("Source that originated the estimate.");
        entity.Property(e => e.OpportunityId).HasComment("Associated opportunity.");
        entity.Property(e => e.CustomerId).HasComment("Associated customer.");
        entity.Property(e => e.ServiceLocationId).HasComment("Service location where work will be performed.");
        entity.Property(e => e.WorkOrderId).HasComment("Work order generated from the estimate.");
        entity.Property(e => e.JobTypeId).HasComment("Job type associated with the estimate.");
        entity.Property(e => e.PaymentTermId).HasComment("Payment terms applicable to the estimate.");
        entity.Property(e => e.PaymentMethodId).HasComment("Preferred payment method for the estimate.");
        entity.Property(e => e.Break1Id).HasComment("First accounting segment used for GL exports and reporting.");
        entity.Property(e => e.Break2Id).HasComment("Second accounting segment used for GL exports and reporting.");
        entity.Property(e => e.EstimateName).HasMaxLength(255).IsRequired()
            .HasComment("User-facing estimate name.");
        entity.Property(e => e.EstimateDescription).HasColumnType("text")
            .HasComment("Detailed estimate description, scope summary, or explanatory information presented to the customer.");
        entity.Property(e => e.InternalNotes).HasColumnType("text")
            .HasComment("Stores internal notes related to the estimate. This information is intended for internal company use and should not be displayed to the customer.");
        entity.Property(e => e.InstallationDescription).HasColumnType("text")
            .HasComment("Stores installation instructions, installation scope details, or other information intended for the installation or field service team.");
        entity.Property(e => e.EstimateDate).HasColumnType("date")
            .HasComment("Date estimate was created.");
        entity.Property(e => e.ExpirationDate).HasColumnType("date")
            .HasComment("Date estimate expires.");
        entity.Property(e => e.QuotedByEmployeeId).HasComment("Employee who prepared or presented the estimate.");
        entity.Property(e => e.SoldByEmployeeId).HasComment("Employee credited with the sale.");
        entity.Property(e => e.RecommendedByEmployeeId).HasComment(
            "Stores the employee ID of the employee who recommended the estimate or proposed work to the customer.");
        entity.Property(e => e.VerificationRequired).HasDefaultValue(false)
            .HasComment("Indicates whether the estimate requires internal verification before it can proceed through the estimate workflow.");
        entity.Property(e => e.VerifiedByEmployeeId).HasComment(
            "Stores the employee ID of the employee who reviewed and verified the estimate.");
        entity.Property(e => e.VerifiedOn).HasColumnType("timestamptz")
            .HasComment("Stores the date and time when the estimate was verified.");
        entity.Property(e => e.SelectedEstimateOptionId).HasComment("Estimate option selected by the customer.");
        entity.Property(e => e.SignedBy).HasMaxLength(255)
            .HasComment("Name entered by the person signing the estimate.");
        entity.Property(e => e.SignedOn).HasColumnType("timestamptz")
            .HasComment("Date and time the estimate was signed.");
        entity.Property(e => e.SignatureFileId).HasComment(
            "File identifier pointing to the signature image stored in file.FgsFile.");
        entity.Property(e => e.TaxAuthoritySnapshotJson).HasColumnType("jsonb")
            .HasComment("Historical snapshot of tax authority codes, names, and rates used for tax calculations.");
        entity.Property(e => e.MaterialPricingMatrixId).HasComment("Material pricing matrix used for pricing calculations.");
        entity.Property(e => e.LaborPricingMatrixId).HasComment("Labor pricing matrix used for pricing calculations.");
        entity.Property(e => e.OtherPricingMatrixId).HasComment("Other pricing matrix used for pricing calculations.");
        entity.Property(e => e.TermsConditionVersionId)
            .HasComment("Reference to the specific terms and conditions version used by the estimate.");
        entity.Property(e => e.SubtotalAmount).HasColumnType("numeric(18,2)").HasDefaultValue(0m)
            .HasComment("Subtotal before discounts and taxes.");
        entity.Property(e => e.DiscountAmount).HasColumnType("numeric(18,2)").HasDefaultValue(0m)
            .HasComment("Total discount amount.");
        entity.Property(e => e.TaxAmount).HasColumnType("numeric(18,2)").HasDefaultValue(0m)
            .HasComment("Total tax amount.");
        entity.Property(e => e.TotalAmount).HasColumnType("numeric(18,2)").HasDefaultValue(0m)
            .HasComment("Final estimate amount.");
        entity.Property(e => e.GrossProfitAmount).HasColumnType("numeric(18,2)").HasDefaultValue(0m)
            .HasComment("Gross profit amount.");
        entity.Property(e => e.GrossProfitPercent).HasColumnType("numeric(9,4)").HasDefaultValue(0m)
            .HasComment("Gross profit percentage.");

        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).HasComment("User or process that created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz")
            .HasComment("Date and time the record was last updated.");
        entity.Property(e => e.UpdatedBy).HasComment("User or process that last updated the record.");

        entity.HasOne<FgsEstimateStatus>()
            .WithMany()
            .HasForeignKey(e => e.EstimateStatusId)
            .HasConstraintName("FK_FgsEstimate_EstimateStatus")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.EstimateNumber })
            .IsUnique()
            .HasDatabaseName("UX_FgsEstimate_TenantId_CompanyId_EstimateNumber");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.WorkOrderId })
            .IsUnique()
            .HasFilter("\"WorkOrderId\" IS NOT NULL")
            .HasDatabaseName("UX_FgsEstimate_TenantId_CompanyId_WorkOrderId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.OpportunityId })
            .IsUnique()
            .HasFilter("\"OpportunityId\" IS NOT NULL")
            .HasDatabaseName("UX_FgsEstimate_TenantId_CompanyId_OpportunityId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsEstimate_TenantId_CompanyId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.EstimateStatusId })
            .HasDatabaseName("IX_FgsEstimate_TenantId_CompanyId_EstimateStatusId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CustomerId })
            .HasDatabaseName("IX_FgsEstimate_TenantId_CompanyId_CustomerId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ServiceLocationId })
            .HasDatabaseName("IX_FgsEstimate_TenantId_CompanyId_ServiceLocationId");
    }
}
