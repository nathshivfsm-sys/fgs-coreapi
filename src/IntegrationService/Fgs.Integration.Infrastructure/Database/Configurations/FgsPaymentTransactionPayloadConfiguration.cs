using Fgs.Integration.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Integration.Infrastructure.Database.Configurations;

internal sealed class FgsPaymentTransactionPayloadConfiguration : IEntityTypeConfiguration<FgsPaymentTransactionPayload>
{
    public void Configure(EntityTypeBuilder<FgsPaymentTransactionPayload> entity)
    {
        entity.ToTable(
            "FgsPaymentTransactionPayload",
            t => t.HasComment(
                "Stores optional payment processor request and response payloads for troubleshooting, support, auditing, and integration diagnostics."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityAlwaysColumn();

        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.PaymentTransactionId).IsRequired();
        entity.Property(e => e.RequestJson).HasColumnType("jsonb");
        entity.Property(e => e.ResponseJson).HasColumnType("jsonb");
        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamp").HasDefaultValueSql("now()");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsPaymentTransactionPayload_TenantCompany");
        entity.HasIndex(e => e.PaymentTransactionId)
            .IsUnique()
            .HasDatabaseName("UX_FgsPaymentTransactionPayload_PaymentTransaction");
    }
}
