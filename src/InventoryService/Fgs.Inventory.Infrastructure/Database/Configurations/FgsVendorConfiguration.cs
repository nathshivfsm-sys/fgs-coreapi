using Fgs.Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Inventory.Infrastructure.Database.Configurations;

internal sealed class FgsVendorConfiguration : IEntityTypeConfiguration<FgsVendor>
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
                t.HasCheckConstraint(
                    "CK_FgsVendor_VendorStatus",
                    "\"VendorStatus\" IN ('ACTIVE', 'INACTIVE', 'ON_HOLD')");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureCatalogColumns();

        entity.Property(e => e.VendorCode).HasMaxLength(50).IsRequired();
        entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
        entity.Property(e => e.LegalName).HasMaxLength(200);
        entity.Property(e => e.VendorType).HasMaxLength(50).IsRequired();
        entity.Property(e => e.VendorStatus).HasMaxLength(30).IsRequired().HasDefaultValue(VendorStatuses.Active);
        entity.Property(e => e.VendorAccountNumber).HasMaxLength(100);
        entity.Property(e => e.PaymentTermId)
            .HasComment("References setup payment terms; scalar only — no cross-schema FK.");
        entity.Property(e => e.ContactName).HasMaxLength(150);
        entity.Property(e => e.ContactTitle).HasMaxLength(100);
        entity.Property(e => e.Email).HasMaxLength(255);
        entity.Property(e => e.PurchaseOrderEmail).HasMaxLength(255);
        entity.Property(e => e.PhoneNumber).HasMaxLength(50);
        entity.Property(e => e.MobileNumber).HasMaxLength(50);
        entity.Property(e => e.FaxNumber).HasMaxLength(50);
        entity.Property(e => e.Website).HasMaxLength(255);
        entity.Property(e => e.Address1).HasMaxLength(200);
        entity.Property(e => e.Address2).HasMaxLength(200);
        entity.Property(e => e.City).HasMaxLength(100);
        entity.Property(e => e.StateProvince).HasMaxLength(100);
        entity.Property(e => e.PostalCode).HasMaxLength(20);
        entity.Property(e => e.Country).HasMaxLength(100);
        entity.Property(e => e.TaxIdNumber).HasMaxLength(100);
        entity.Property(e => e.LicenseNumber).HasMaxLength(100);
        entity.Property(e => e.InsurancePolicyNumber).HasMaxLength(100);
        entity.Property(e => e.Notes).HasColumnType("text");
        entity.Property(e => e.Is1099Eligible).HasDefaultValue(false);

        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.VendorCode })
            .HasName("UQ_FgsVendor_TenantId_CompanyId_VendorCode");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsVendor_TenantId_CompanyId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Name })
            .HasDatabaseName("IX_FgsVendor_TenantId_CompanyId_Name");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ContactName })
            .HasDatabaseName("IX_FgsVendor_TenantId_CompanyId_VendorContactName");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.PhoneNumber })
            .HasDatabaseName("IX_FgsVendor_TenantId_CompanyId_PhoneNumber");
    }
}
