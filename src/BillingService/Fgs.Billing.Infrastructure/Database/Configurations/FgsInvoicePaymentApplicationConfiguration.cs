using Fgs.Billing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Billing.Infrastructure.Database.Configurations;

internal sealed class FgsInvoicePaymentApplicationConfiguration : IEntityTypeConfiguration<FgsInvoicePaymentApplication>
{
    public void Configure(EntityTypeBuilder<FgsInvoicePaymentApplication> entity)
    {
        entity.ToTable(
            "FgsInvoicePaymentApplication",
            t => t.HasComment(
                "Stores payment allocation records between payments and invoices."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityAlwaysColumn();

        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1);
        entity.Property(e => e.AppliedAmount).HasColumnType("numeric(18,2)").IsRequired();
        entity.Property(e => e.AppliedOn).IsRequired().HasColumnType("timestamp").HasDefaultValueSql("now()");
        entity.Property(e => e.ApplicationNote).HasColumnType("text");
        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamp").HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamp");

        entity.HasOne<FgsPayment>()
            .WithMany()
            .HasForeignKey(e => e.PaymentId)
            .HasConstraintName("FK_FgsInvoicePaymentApplication_Payment")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne<FgsInvoice>()
            .WithMany()
            .HasForeignKey(e => e.InvoiceId)
            .HasConstraintName("FK_FgsInvoicePaymentApplication_Invoice")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsInvoicePaymentApplication_TenantCompany");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.PaymentId })
            .HasDatabaseName("IX_FgsInvoicePaymentApplication_Payment");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.InvoiceId })
            .HasDatabaseName("IX_FgsInvoicePaymentApplication_Invoice");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.AppliedOn })
            .HasDatabaseName("IX_FgsInvoicePaymentApplication_AppliedOn");
        entity.HasIndex(e => new { e.PaymentId, e.InvoiceId })
            .IsUnique()
            .HasDatabaseName("UX_FgsInvoicePaymentApplication_PaymentInvoice");
    }
}
