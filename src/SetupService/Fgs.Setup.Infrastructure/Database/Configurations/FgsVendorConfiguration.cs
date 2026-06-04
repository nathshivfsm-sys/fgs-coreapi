using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsVendorConfiguration : IEntityTypeConfiguration<FgsVendor>
{
    public void Configure(EntityTypeBuilder<FgsVendor> entity)
    {
        entity.ToTable(
            "FgsVendor",
            t =>
            {
                t.HasComment(
                    "Stores vendor and subcontractor master information for purchasing, AP, and subcontractor management.");
                t.HasCheckConstraint(
                    "CK_FgsVendor_VendorType",
                    "\"VendorType\" IN ('VENDOR', 'SUBCONTRACTOR')");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns(tenantCompanyIndexName: "IX_FgsVendor_TenantId_CompanyId");

        entity.Property(e => e.VendorCode).HasMaxLength(50).IsRequired();
        entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
        entity.Property(e => e.LegalName).HasMaxLength(200);
        entity.Property(e => e.VendorType)
            .HasMaxLength(50)
            .IsRequired()
            .HasComment("Allowed values: VENDOR, SUBCONTRACTOR");
        entity.Property(e => e.Email).HasMaxLength(255);
        entity.Property(e => e.PhoneNumber).HasMaxLength(50);
        entity.Property(e => e.MobileNumber).HasMaxLength(50);
        entity.Property(e => e.Website).HasMaxLength(255);
        entity.Property(e => e.TaxIdentificationNumber).HasMaxLength(100);
        entity.Property(e => e.LicenseNumber).HasMaxLength(100);
        entity.Property(e => e.InsurancePolicyNumber).HasMaxLength(100);
        entity.Property(e => e.Notes).HasColumnType("text");
        entity.Property(e => e.Is1099Eligible)
            .HasDefaultValue(false)
            .HasComment("Indicates whether vendor should be included in 1099 reporting.");
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.PaymentTermId)
            .HasComment("References payment terms used for accounts payable due date calculation.");
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Name })
            .HasDatabaseName("IX_FgsVendor_TenantId_CompanyId_Name");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.VendorType })
            .HasDatabaseName("IX_FgsVendor_TenantId_CompanyId_VendorType");

        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.VendorCode })
            .HasName("UQ_FgsVendor_TenantId_CompanyId_VendorCode");

        entity.HasOne<FgsSetupPaymentTerm>()
            .WithMany()
            .HasForeignKey(e => e.PaymentTermId)
            .HasConstraintName("FK_FgsVendor_FgsSetupPaymentTerm")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
