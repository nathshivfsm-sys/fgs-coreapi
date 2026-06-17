using Fgs.Billing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Billing.Infrastructure.Database.Configurations;

internal sealed class FgsInvoiceBatchConfiguration : IEntityTypeConfiguration<FgsInvoiceBatch>
{
    public void Configure(EntityTypeBuilder<FgsInvoiceBatch> entity)
    {
        entity.ToTable("FgsInvoiceBatch");

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityAlwaysColumn();

        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.BatchNumber).HasMaxLength(50).IsRequired();
        entity.Property(e => e.InvoiceCount).HasDefaultValue(0);
        entity.Property(e => e.InvoiceSubtotal).HasColumnType("numeric(18,2)").HasDefaultValue(0m);
        entity.Property(e => e.TotalTax).HasColumnType("numeric(18,2)").HasDefaultValue(0m);
        entity.Property(e => e.InvoiceTotal).HasColumnType("numeric(18,2)").HasDefaultValue(0m);
        entity.Property(e => e.IsClosed).HasDefaultValue(false);
        entity.Property(e => e.ClosedOn).HasColumnType("timestamp");
        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamp").HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamp");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsInvoiceBatch_TenantCompany");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.BatchNumber })
            .IsUnique()
            .HasDatabaseName("UX_FgsInvoiceBatch_TenantCompany_BatchNumber");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.BatchDate })
            .HasDatabaseName("IX_FgsInvoiceBatch_BatchDate");
    }
}
