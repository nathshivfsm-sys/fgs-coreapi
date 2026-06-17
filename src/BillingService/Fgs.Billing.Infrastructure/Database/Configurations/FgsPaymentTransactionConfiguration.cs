using Fgs.Billing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Billing.Infrastructure.Database.Configurations;

internal sealed class FgsPaymentTransactionConfiguration : IEntityTypeConfiguration<FgsPaymentTransaction>
{
    public void Configure(EntityTypeBuilder<FgsPaymentTransaction> entity)
    {
        entity.ToTable(
            "FgsPaymentTransaction",
            t => t.HasComment(
                "Stores payment processor transaction records associated with customer payments."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityAlwaysColumn();

        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.TransactionId).HasMaxLength(150).IsRequired();
        entity.Property(e => e.OriginalTransactionId).HasMaxLength(150);
        entity.Property(e => e.AuthorizationCode).HasMaxLength(100);
        entity.Property(e => e.ProcessorStatus).HasMaxLength(50);
        entity.Property(e => e.CardHolderName).HasMaxLength(200);
        entity.Property(e => e.CardLast4).HasMaxLength(4);
        entity.Property(e => e.BankAccountLast4).HasMaxLength(4);
        entity.Property(e => e.TransactionAmount).HasColumnType("numeric(18,2)").IsRequired();
        entity.Property(e => e.TransactionDate).HasColumnType("timestamp").IsRequired();
        entity.Property(e => e.UserName).HasMaxLength(200);
        entity.Property(e => e.Source).HasMaxLength(50);
        entity.Property(e => e.TransactionDataJson).HasColumnType("jsonb");
        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamp").HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamp");

        entity.HasOne<FgsPayment>()
            .WithMany()
            .HasForeignKey(e => e.PaymentId)
            .HasConstraintName("FK_FgsPaymentTransaction_Payment")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsPaymentTransaction_TenantCompany");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.PaymentId })
            .HasDatabaseName("IX_FgsPaymentTransaction_Payment");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.TransactionDate })
            .HasDatabaseName("IX_FgsPaymentTransaction_TransactionDate");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.PaymentProcessorId })
            .HasDatabaseName("IX_FgsPaymentTransaction_Processor");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.OriginalTransactionId })
            .HasDatabaseName("IX_FgsPaymentTransaction_OriginalTransactionId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ProcessorStatus })
            .HasDatabaseName("IX_FgsPaymentTransaction_ProcessorStatus");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.TransactionId })
            .IsUnique()
            .HasDatabaseName("UX_FgsPaymentTransaction_TransactionId");
    }
}
