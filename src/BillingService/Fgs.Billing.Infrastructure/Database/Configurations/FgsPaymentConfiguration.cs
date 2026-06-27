using Fgs.Billing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Billing.Infrastructure.Database.Configurations;

internal sealed class FgsPaymentConfiguration : IEntityTypeConfiguration<FgsPayment>
{
    public void Configure(EntityTypeBuilder<FgsPayment> entity)
    {
        entity.ToTable(
            "FgsPayment",
            t => t.HasComment(
                "Stores customer payment transactions received for invoices, estimates, service agreements, deposits, refunds, and other billing-related activities."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityAlwaysColumn();

        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.PaymentNumber).HasMaxLength(50).IsRequired();
        entity.Property(e => e.SourceType).HasMaxLength(50);
        entity.Property(e => e.ReferenceNumber).HasMaxLength(100);
        entity.Property(e => e.PaymentAmount).HasColumnType("numeric(18,2)").IsRequired();
        entity.Property(e => e.AppliedAmount).HasColumnType("numeric(18,2)").HasDefaultValue(0m);
        entity.Property(e => e.PaymentNote).HasColumnType("text");
        entity.Property(e => e.ExternalAccountingId).HasMaxLength(100);
        entity.Property(e => e.ExternalAccountingSyncToken).HasMaxLength(100);
        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamp").HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamp");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsPayment_TenantCompany");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.PaymentNumber })
            .IsUnique()
            .HasDatabaseName("UX_FgsPayment_TenantCompany_PaymentNumber");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CustomerId })
            .HasDatabaseName("IX_FgsPayment_Customer");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ServiceLocationId })
            .HasDatabaseName("IX_FgsPayment_ServiceLocation");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.PaymentDate })
            .HasDatabaseName("IX_FgsPayment_PaymentDate");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.AccountingDate })
            .HasDatabaseName("IX_FgsPayment_AccountingDate");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.BankAccountId })
            .HasDatabaseName("IX_FgsPayment_BankAccount");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.SourceType, e.SourceId })
            .HasDatabaseName("IX_FgsPayment_Source");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.PaymentStatusId })
            .HasDatabaseName("IX_FgsPayment_Status");
    }
}
