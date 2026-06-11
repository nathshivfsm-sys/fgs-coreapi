using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsSetupServiceAgreementTemplateConfiguration : IEntityTypeConfiguration<FgsSetupServiceAgreementTemplate>
{
    public void Configure(EntityTypeBuilder<FgsSetupServiceAgreementTemplate> entity)
    {
        entity.ToTable(
            "FgsSetupServiceAgreementTemplate",
            t =>
            {
                t.HasComment(
                    "Stores service agreement templates defining billing frequency, maintenance frequency, repair discounts, and default terms for a tenant-company.");
                t.HasCheckConstraint(
                    "CK_FgsSetupServiceAgreementTemplate_BillingFrequencyMonths",
                    "\"BillingFrequencyMonths\" IN (1,3,6,12)");
                t.HasCheckConstraint(
                    "CK_FgsSetupServiceAgreementTemplate_MaintenanceFrequencyMonths",
                    "\"MaintenanceFrequencyMonths\" IN (1,3,6,12)");
                t.HasCheckConstraint(
                    "CK_FgsSetupServiceAgreementTemplate_RepairDiscountPercent",
                    "\"RepairDiscountPercent\" >= 0 AND \"RepairDiscountPercent\" <= 100");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns(
            tenantCompanyIndexName: "IX_FgsSetupServiceAgreementTemplate_TenantId_CompanyId");

        entity.Property(e => e.TemplateCode).HasMaxLength(50).IsRequired();
        entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
        entity.Property(e => e.Description).HasColumnType("text");

        entity.Property(e => e.BillingFrequencyMonths)
            .IsRequired()
            .HasComment(
                "Billing frequency in months. 1 = Monthly, 3 = Quarterly, 6 = Semi-Annual, 12 = Annual.");

        entity.Property(e => e.MaintenanceFrequencyMonths)
            .IsRequired()
            .HasComment(
                "Maintenance frequency in months. 1 = Monthly, 3 = Quarterly, 6 = Semi-Annual, 12 = Annual.");

        entity.Property(e => e.RepairDiscountPercent)
            .HasPrecision(5, 2)
            .HasDefaultValue(0m)
            .HasComment("Discount given to service agreement customers on additional repairs.");

        entity.Property(e => e.IsAutoRenew).HasDefaultValue(true);
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1);
        entity.Property(e => e.IsActive).HasDefaultValue(true);

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Name })
            .HasDatabaseName("IX_FgsSetupServiceAgreementTemplate_TenantId_CompanyId_Name");

        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.TemplateCode })
            .HasName("UQ_FgsSetupServiceAgreementTemplate_TenantId_CompanyId_TemplateCode");
    }
}
