using Fgs.ServiceAgreement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.ServiceAgreement.Infrastructure.Database.Configurations;

internal sealed class FgsServiceAgreementBillingScheduleConfiguration : IEntityTypeConfiguration<FgsServiceAgreementBillingSchedule>
{
    public void Configure(EntityTypeBuilder<FgsServiceAgreementBillingSchedule> entity)
    {
        entity.ToTable(
            "FgsServiceAgreementBillingSchedule",
            t =>
            {
                t.HasComment(
                    "Stores future billing events generated from a service agreement. Billing schedules generate invoices but are not invoices themselves.");
                t.HasCheckConstraint("CK_FgsServiceAgreementBillingSchedule_Status", "\"BillingScheduleStatusId\" IN (1, 2, 3, 4, 5)");
                t.HasCheckConstraint("CK_FgsServiceAgreementBillingSchedule_BillingAmount", "\"BillingAmount\" >= 0");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.ServiceAgreementId).HasComment("Parent service agreement identifier.");
        entity.Property(e => e.BillingSequence).HasComment("Sequential billing number within the service agreement.");
        entity.Property(e => e.BillingDate).HasComment("Scheduled billing date used for invoice generation.");
        entity.Property(e => e.BillingAmount).HasColumnType("numeric(18,2)").HasComment("Amount expected to be billed for this billing event.");
        entity.Property(e => e.BillingScheduleStatusId)
            .HasComment("Billing schedule status. Values: 1=Pending, 2=InvoiceCreated, 3=Invoiced, 4=Skipped, 5=Cancelled.");
        entity.Property(e => e.InvoiceId).HasComment("Generated invoice identifier associated with the billing event.");
        entity.Property(e => e.ExternalInvoiceNumber).HasMaxLength(100)
            .HasComment("Invoice number from a legacy or external system used during data migration.");

        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Record creation timestamp.");
        entity.Property(e => e.CreatedBy).HasComment("User that created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz").HasComment("Record last update timestamp.");
        entity.Property(e => e.UpdatedBy).HasComment("User that last updated the record.");

        entity.HasOne(e => e.ServiceAgreement)
            .WithMany()
            .HasForeignKey(e => e.ServiceAgreementId)
            .HasConstraintName("FK_FgsServiceAgreementBillingSchedule_ServiceAgreement")
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ServiceAgreementId, e.BillingSequence })
            .IsUnique()
            .HasDatabaseName("UQ_FgsServiceAgreementBillingSchedule_Agreement_Sequence");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId }).HasDatabaseName("IX_FgsServiceAgreementBillingSchedule_TenantId_CompanyId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ServiceAgreementId })
            .HasDatabaseName("IX_FgsServiceAgreementBillingSchedule_ServiceAgreementId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.BillingScheduleStatusId })
            .HasDatabaseName("IX_FgsServiceAgreementBillingSchedule_Status");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.BillingDate })
            .HasDatabaseName("IX_FgsServiceAgreementBillingSchedule_BillingDate");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.InvoiceId })
            .HasDatabaseName("IX_FgsServiceAgreementBillingSchedule_InvoiceId");
    }
}
