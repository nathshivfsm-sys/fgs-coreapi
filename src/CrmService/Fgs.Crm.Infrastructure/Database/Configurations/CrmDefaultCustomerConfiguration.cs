using Fgs.Crm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Crm.Infrastructure.Database.Configurations;

internal sealed class CrmDefaultCustomerConfiguration : IEntityTypeConfiguration<CrmDefaultCustomer>
{
    public void Configure(EntityTypeBuilder<CrmDefaultCustomer> entity)
    {
        entity.ToTable(
            "CrmDefaultCustomer",
            t => t.HasComment("Stores default values used when creating new customers."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .UseIdentityByDefaultColumn()
            .HasComment("Primary key.");
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");

        entity.Property(e => e.DefaultPaymentTermId)
            .HasComment("Default payment term applied to a new customer.");

        entity.Property(e => e.DefaultMaterialPricingMatrixId)
            .HasComment("Default material pricing matrix applied to a new customer.");

        entity.Property(e => e.DefaultLaborPricingMatrixId)
            .HasComment("Default labor pricing matrix applied to a new customer.");

        entity.Property(e => e.DefaultOtherPricingMatrixId)
            .HasComment("Default miscellaneous pricing matrix applied to a new customer.");

        entity.Property(e => e.DefaultPORequired)
            .HasComment("Indicates whether a purchase order is required by default for a new customer.");

        entity.Property(e => e.TaxExempt)
            .HasComment("Indicates whether a new customer is tax exempt by default.");

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .HasComment("Record creation timestamp.");

        entity.Property(e => e.CreatedBy)
            .HasMaxLength(100)
            .HasComment("User that created the record.");

        entity.Property(e => e.UpdatedOn)
            .HasColumnType("timestamptz")
            .HasComment("Last update timestamp.");

        entity.Property(e => e.UpdatedBy)
            .HasMaxLength(100)
            .HasComment("User that last updated the record.");
    }
}
