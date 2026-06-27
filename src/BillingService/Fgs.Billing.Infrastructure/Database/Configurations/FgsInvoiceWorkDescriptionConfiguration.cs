using Fgs.Billing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Billing.Infrastructure.Database.Configurations;

internal sealed class FgsInvoiceWorkDescriptionConfiguration : IEntityTypeConfiguration<FgsInvoiceWorkDescription>
{
    public void Configure(EntityTypeBuilder<FgsInvoiceWorkDescription> entity)
    {
        entity.ToTable(
            "FgsInvoiceWorkDescription",
            t => t.HasComment(
                "Stores technician and office-entered work descriptions associated with an invoice. Supports multiple work performed entries for an invoice."));

        entity.HasKey(e => e.Id).HasName("PK_FgsInvoiceWorkDescription");
        entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.InvoiceId).HasComment("Parent invoice identifier.");
        entity.Property(e => e.ServiceDate).HasColumnType("date").IsRequired()
            .HasComment("Service date for the work performed entry.");
        entity.Property(e => e.TechCode).HasMaxLength(50)
            .HasComment("Technician code associated with the work performed.");
        entity.Property(e => e.UserName).HasMaxLength(200).IsRequired()
            .HasComment("User who entered the work description.");
        entity.Property(e => e.WorkDescription).HasColumnType("text").IsRequired()
            .HasComment("Work performed description.");
        entity.Property(e => e.IsCustomerVisible).HasDefaultValue(true)
            .HasComment("Indicates whether the work description is visible to the customer.");
        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamp").HasDefaultValueSql("now()");
        entity.Property(e => e.CreatedBy).IsRequired();
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamp");
        entity.Property(e => e.UpdatedBy);

        entity.HasOne<FgsInvoice>()
            .WithMany()
            .HasForeignKey(e => e.InvoiceId)
            .HasConstraintName("FK_FgsInvoiceWorkDescription_FgsInvoice")
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsInvoiceWorkDescription_TenantCompany");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.InvoiceId })
            .HasDatabaseName("IX_FgsInvoiceWorkDescription_InvoiceId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ServiceDate })
            .HasDatabaseName("IX_FgsInvoiceWorkDescription_ServiceDate");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.TechCode })
            .HasDatabaseName("IX_FgsInvoiceWorkDescription_TechCode");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsCustomerVisible })
            .HasDatabaseName("IX_FgsInvoiceWorkDescription_IsCustomerVisible");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.InvoiceId, e.TechCode, e.ServiceDate })
            .IsUnique()
            .HasFilter("\"TechCode\" IS NOT NULL")
            .HasDatabaseName("UX_FgsInvoiceWorkDescription_Invoice_TechCode_ServiceDate");
    }
}
