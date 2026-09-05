using Fgs.Billing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Billing.Infrastructure.Database.Configurations;

internal sealed class FgsInvoiceBatchConfiguration : IEntityTypeConfiguration<FgsInvoiceBatch>
{
    public void Configure(EntityTypeBuilder<FgsInvoiceBatch> entity)
    {
        entity.ToTable(
            "FgsInvoiceBatch",
            t => t.HasComment(
                "Stores invoice batch records used to group and summarize invoices for a tenant and company."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .UseIdentityAlwaysColumn()
            .HasComment("Unique identifier for the invoice batch.");

        entity.ConfigureTenantCompanyColumns();
        entity.Property(e => e.TenantId)
            .HasComment("Identifies the tenant that owns the invoice batch.");
        entity.Property(e => e.CompanyId)
            .HasComment("Identifies the company within the tenant that owns the invoice batch.");

        entity.Property(e => e.BatchNumber)
            .HasMaxLength(50)
            .IsRequired()
            .HasComment(
                "Unique batch number used to identify the invoice batch within the tenant and company.");
        entity.Property(e => e.BatchDate)
            .HasComment("Date assigned to the invoice batch.");
        entity.Property(e => e.InvoiceCount)
            .HasDefaultValue(0)
            .HasComment("Number of invoices included in the batch.");
        entity.Property(e => e.InvoiceSubtotal)
            .HasColumnType("numeric(18,2)")
            .HasDefaultValue(0m)
            .HasComment("Sum of the subtotals for all invoices included in the batch before tax.");
        entity.Property(e => e.TotalTax)
            .HasColumnType("numeric(18,2)")
            .HasDefaultValue(0m)
            .HasComment("Total tax amount across all invoices included in the batch.");
        entity.Property(e => e.InvoiceTotal)
            .HasColumnType("numeric(18,2)")
            .HasDefaultValue(0m)
            .HasComment(
                "Total invoice amount across all invoices included in the batch, including applicable taxes.");
        entity.Property(e => e.IsClosed)
            .HasDefaultValue(false)
            .HasComment(
                "Indicates whether the invoice batch has been closed and is no longer available for further batch processing.");
        entity.Property(e => e.ClosedOn)
            .HasColumnType("timestamp")
            .HasComment("Date and time when the invoice batch was closed.");
        entity.Property(e => e.ClosedBy)
            .HasComment("Identifies the user who closed the invoice batch.");
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamp")
            .HasDefaultValueSql("now()")
            .HasComment("Date and time when the invoice batch was created.");
        entity.Property(e => e.CreatedBy)
            .HasComment("Identifies the user who created the invoice batch.");
        entity.Property(e => e.UpdatedOn)
            .HasColumnType("timestamp")
            .HasComment("Date and time when the invoice batch was last updated.");
        entity.Property(e => e.UpdatedBy)
            .HasComment("Identifies the user who last updated the invoice batch.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsInvoiceBatch_TenantCompany");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.BatchNumber })
            .IsUnique()
            .HasDatabaseName("UX_FgsInvoiceBatch_TenantCompany_BatchNumber");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.BatchDate })
            .HasDatabaseName("IX_FgsInvoiceBatch_BatchDate");
    }
}
