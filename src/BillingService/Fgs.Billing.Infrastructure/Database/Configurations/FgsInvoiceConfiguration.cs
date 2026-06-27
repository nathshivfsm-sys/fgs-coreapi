using Fgs.Billing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Billing.Infrastructure.Database.Configurations;

internal sealed class FgsInvoiceConfiguration : IEntityTypeConfiguration<FgsInvoice>
{
    public void Configure(EntityTypeBuilder<FgsInvoice> entity)
    {
        entity.ToTable("FgsInvoice");

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityAlwaysColumn();

        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.InvoiceNumber).HasMaxLength(50).IsRequired();
        entity.Property(e => e.ServiceJobNum).HasMaxLength(100);
        entity.Property(e => e.WorkOrderNumber).HasMaxLength(50);
        entity.Property(e => e.CustomerPONumber).HasMaxLength(100);
        entity.Property(e => e.TaxingAuthorityJson).HasColumnType("jsonb");
        entity.Property(e => e.BillToAddressJson).HasColumnType("jsonb");
        entity.Property(e => e.ServiceLocationAddressJson).HasColumnType("jsonb");
        entity.Property(e => e.CompanyAddressJson).HasColumnType("jsonb");
        entity.Property(e => e.ExternalAccountingId).HasMaxLength(100);
        entity.Property(e => e.ExternalAccountingSyncToken).HasMaxLength(100);

        entity.Property(e => e.IsAgreementBilling).HasDefaultValue(false);
        entity.Property(e => e.IsRecurringInvoice).HasDefaultValue(false);
        entity.Property(e => e.IsSigned).HasDefaultValue(false);
        entity.Property(e => e.SignedOn).HasColumnType("timestamp");
        entity.Property(e => e.InvoiceSubtotal).HasColumnType("numeric(18,2)").HasDefaultValue(0m);
        entity.Property(e => e.TotalDiscount).HasColumnType("numeric(18,2)").HasDefaultValue(0m);
        entity.Property(e => e.TaxableAmount).HasColumnType("numeric(18,2)").HasDefaultValue(0m);
        entity.Property(e => e.TotalTax).HasColumnType("numeric(18,2)").HasDefaultValue(0m);
        entity.Property(e => e.InvoiceTotal).HasColumnType("numeric(18,2)").HasDefaultValue(0m);
        entity.Property(e => e.AppliedAmount).HasColumnType("numeric(18,2)").HasDefaultValue(0m);
        entity.Property(e => e.BalanceDue).HasColumnType("numeric(18,2)").HasDefaultValue(0m);
        entity.Property(e => e.IsApproved).HasDefaultValue(false);
        entity.Property(e => e.ApprovedOn).HasColumnType("timestamp");
        entity.Property(e => e.IsPosted).HasDefaultValue(false);
        entity.Property(e => e.PostedOn).HasColumnType("timestamp");
        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamp").HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamp");
        entity.Property(e => e.RowVersion).HasDefaultValue(1L);

        entity.HasOne<FgsInvoiceBatch>()
            .WithMany()
            .HasForeignKey(e => e.InvoiceBatchId)
            .HasConstraintName("FK_FgsInvoice_InvoiceBatch")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.InvoiceNumber })
            .IsUnique()
            .HasDatabaseName("UX_FgsInvoice_TenantCompany_InvoiceNumber");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CustomerId })
            .HasDatabaseName("IX_FgsInvoice_CustomerId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ServiceLocationId })
            .HasDatabaseName("IX_FgsInvoice_ServiceLocationId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.WorkOrderId })
            .HasDatabaseName("IX_FgsInvoice_WorkOrderId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ServiceAgreementId })
            .HasDatabaseName("IX_FgsInvoice_ServiceAgreementId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.InvoiceDate })
            .HasDatabaseName("IX_FgsInvoice_InvoiceDate");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.AccountingDate })
            .HasDatabaseName("IX_FgsInvoice_AccountingDate");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.InvoiceBatchId })
            .HasDatabaseName("IX_FgsInvoice_InvoiceBatchId");
    }
}
