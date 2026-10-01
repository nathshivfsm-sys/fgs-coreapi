using Fgs.Crm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Crm.Infrastructure.Database.Configurations;

internal sealed class CrmCustomerConfiguration : IEntityTypeConfiguration<CrmCustomer>
{
    public void Configure(EntityTypeBuilder<CrmCustomer> entity)
    {
        entity.ToTable(
            "CrmCustomer",
            t => t.HasComment(
                "Represents a customer account that can own one or more service locations and is responsible for billing."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .UseIdentityByDefaultColumn()
            .HasComment("Primary key for the customer.");
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier within the tenant.");

        entity.Property(e => e.CustomerNumber)
            .HasMaxLength(30)
            .IsRequired()
            .HasComment("Unique business identifier assigned to the customer.");

        entity.Property(e => e.LastServiceLocationSequence)
            .HasDefaultValue(0)
            .HasComment("Last service location sequence number assigned to this customer. Used to generate the next service location sequence.");

        entity.Property(e => e.Name)
            .HasMaxLength(200)
            .IsRequired()
            .HasComment("Customer name.");

        entity.Property(e => e.DisplayName)
            .HasMaxLength(200)
            .IsRequired()
            .HasComment("Customer name displayed to users and customers.");

        entity.Property(e => e.AddressLine1)
            .HasMaxLength(200)
            .HasComment("Primary customer address line.");

        entity.Property(e => e.AddressLine2)
            .HasMaxLength(200)
            .HasComment("Secondary customer address line.");

        entity.Property(e => e.AddressLine3)
            .HasMaxLength(200)
            .HasComment("Additional customer address information.");

        entity.Property(e => e.AddressLine4)
            .HasMaxLength(200)
            .HasComment("Additional customer address information.");

        entity.Property(e => e.City)
            .HasMaxLength(100)
            .HasComment("Customer city.");

        entity.Property(e => e.State)
            .HasMaxLength(100)
            .HasComment("Customer state or province.");

        entity.Property(e => e.County)
            .HasMaxLength(100)
            .HasComment("Customer county or district.");

        entity.Property(e => e.Country)
            .HasMaxLength(100)
            .HasComment("Customer country.");

        entity.Property(e => e.PostalCode)
            .HasMaxLength(20)
            .HasComment("Customer postal or ZIP code.");

        entity.Property(e => e.FormattedAddress)
            .HasMaxLength(1000)
            .HasComment("Formatted customer address returned or constructed by the address or mapping provider.");

        entity.Property(e => e.Latitude)
            .HasColumnType("numeric(18,10)")
            .HasComment("Latitude coordinate associated with the customer address.");

        entity.Property(e => e.Longitude)
            .HasColumnType("numeric(18,10)")
            .HasComment("Longitude coordinate associated with the customer address.");

        entity.Property(e => e.PlaceId)
            .HasMaxLength(500)
            .HasComment("Place identifier returned by the address or mapping provider.");

        entity.Property(e => e.DefaultPaymentTermId)
            .HasComment("Default payment term assigned to the customer.");

        entity.Property(e => e.DefaultMaterialPricingMatrixId)
            .HasComment("Default material pricing matrix assigned to the customer.");

        entity.Property(e => e.DefaultLaborPricingMatrixId)
            .HasComment("Default labor pricing matrix assigned to the customer.");

        entity.Property(e => e.DefaultOtherPricingMatrixId)
            .HasComment("Default miscellaneous or other pricing matrix assigned to the customer.");

        entity.Property(e => e.DefaultPORequired)
            .HasDefaultValue(false)
            .HasComment("Indicates whether a purchase order is required by default for transactions for this customer.");

        entity.Property(e => e.TaxExempt)
            .HasDefaultValue(false)
            .HasComment("Indicates whether the customer is tax exempt.");

        entity.Property(e => e.TaxExemptNumber)
            .HasMaxLength(100)
            .HasComment("Tax exemption certificate or reference number associated with the customer.");

        entity.Property(e => e.CustomerAccountNumber)
            .HasMaxLength(100)
            .HasComment("Customer account number maintained by the customer or an external accounting system.");

        entity.Property(e => e.ExternalEntityId)
            .HasMaxLength(200)
            .HasComment("Identifier of the customer in an external system.");

        entity.Property(e => e.ExternalVersion)
            .HasMaxLength(100)
            .HasComment("Version or synchronization version associated with the customer in an external system.");

        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the customer is active.");

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .HasComment("Timestamp when the customer was created.");

        entity.Property(e => e.CreatedBy)
            .HasComment("User or process that created the customer.");

        entity.Property(e => e.UpdatedOn)
            .HasColumnType("timestamptz")
            .HasComment("Timestamp when the customer was last updated.");

        entity.Property(e => e.UpdatedBy)
            .HasComment("User or process that last updated the customer.");

        entity.Property(e => e.IsPreferredCustomer)
            .HasDefaultValue(false)
            .HasComment(
                "Indicates whether the customer is designated as a preferred customer. TRUE indicates preferred customer status; FALSE indicates standard customer status.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CustomerNumber })
            .IsUnique()
            .HasDatabaseName("UQ_CrmCustomer_CustomerNumber");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.DisplayName })
            .HasDatabaseName("IX_CrmCustomer_DisplayName");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CustomerAccountNumber })
            .HasDatabaseName("IX_CrmCustomer_CustomerAccountNumber");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ExternalEntityId })
            .HasDatabaseName("IX_CrmCustomer_ExternalEntityId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsActive })
            .HasDatabaseName("IX_CrmCustomer_IsActive");
    }
}
