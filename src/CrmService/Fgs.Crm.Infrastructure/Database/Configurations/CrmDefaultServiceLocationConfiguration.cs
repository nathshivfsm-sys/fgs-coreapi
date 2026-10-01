using Fgs.Crm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Crm.Infrastructure.Database.Configurations;

internal sealed class CrmDefaultServiceLocationConfiguration : IEntityTypeConfiguration<CrmDefaultServiceLocation>
{
    public void Configure(EntityTypeBuilder<CrmDefaultServiceLocation> entity)
    {
        entity.ToTable(
            "CrmDefaultServiceLocation",
            t => t.HasComment("Stores default values used when creating new customer service locations."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .UseIdentityByDefaultColumn()
            .HasComment("Primary key.");
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");

        entity.Property(e => e.DefaultLaborPricingMatrixId)
            .HasComment("Default labor pricing matrix applied to a new service location.");

        entity.Property(e => e.DefaultMaterialPricingMatrixId)
            .HasComment("Default material pricing matrix applied to a new service location.");

        entity.Property(e => e.DefaultOtherPricingMatrixId)
            .HasComment("Default miscellaneous pricing matrix applied to a new service location.");

        entity.Property(e => e.DefaultPaymentMethodId)
            .HasComment("Default payment method applied to a new service location.");

        entity.Property(e => e.EmailAllowed)
            .HasComment("Indicates whether email communication is enabled by default for a new service location.");

        entity.Property(e => e.EstimateEmailTemplateId)
            .HasComment("Default estimate email template applied to a new service location.");

        entity.Property(e => e.EstimateSmsTemplateId)
            .HasComment("Default estimate SMS template applied to a new service location.");

        entity.Property(e => e.InvoiceEmailTemplateId)
            .HasComment("Default invoice email template applied to a new service location.");

        entity.Property(e => e.InvoiceSmsTemplateId)
            .HasComment("Default invoice SMS template applied to a new service location.");

        entity.Property(e => e.SmsAllowed)
            .HasComment("Indicates whether SMS communication is enabled by default for a new service location.");

        entity.Property(e => e.TaxExempt)
            .HasComment("Indicates whether a new service location is tax exempt by default.");

        entity.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("User that created the record.");

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .HasComment("Record creation timestamp.");

        entity.Property(e => e.UpdatedBy)
            .HasMaxLength(100)
            .HasComment("User that last updated the record.");

        entity.Property(e => e.UpdatedOn)
            .HasColumnType("timestamptz")
            .HasComment("Last update timestamp.");
    }
}
