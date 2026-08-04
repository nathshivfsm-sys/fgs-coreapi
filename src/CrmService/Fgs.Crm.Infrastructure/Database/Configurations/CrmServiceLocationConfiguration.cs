using Fgs.Crm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Crm.Infrastructure.Database.Configurations;

internal sealed class CrmServiceLocationConfiguration : IEntityTypeConfiguration<CrmServiceLocation>
{
    public void Configure(EntityTypeBuilder<CrmServiceLocation> entity)
    {
        entity.ToTable(
            "CrmServiceLocation",
            t => t.HasComment(
                "Physical customer locations where field service work is performed."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn().HasComment("Primary key.");
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");
        entity.Property(e => e.CustomerId).HasComment("Customer that owns this service location.");

        entity.Property(e => e.LocationSequence)
            .HasComment("Sequential location number within a customer.");

        entity.Property(e => e.LocationNumber)
            .HasMaxLength(50)
            .IsRequired()
            .HasComment("Business identifier for the service location.");

        entity.Property(e => e.Name)
            .HasMaxLength(200)
            .IsRequired()
            .HasDefaultValue(string.Empty)
            .HasComment("Internal service location name.");

        entity.Property(e => e.DisplayName)
            .HasMaxLength(200)
            .IsRequired()
            .HasDefaultValue(string.Empty)
            .HasComment("Display name shown to users and customers.");

        entity.Property(e => e.ServiceLocationTypeId)
            .HasDefaultValue((short)0)
            .HasComment("Lookup to service location type.");

        entity.Property(e => e.AddressLine1).HasMaxLength(200).HasComment("Primary street address.");
        entity.Property(e => e.AddressLine2).HasMaxLength(200).HasComment("Secondary street address.");
        entity.Property(e => e.AddressLine3).HasMaxLength(200).HasComment("Additional address information.");
        entity.Property(e => e.AddressLine4).HasMaxLength(200).HasComment("Additional address information.");
        entity.Property(e => e.City).HasMaxLength(100).HasComment("City.");
        entity.Property(e => e.State).HasMaxLength(100).HasComment("State or province.");
        entity.Property(e => e.County).HasMaxLength(100).HasComment("County or district.");
        entity.Property(e => e.Country).HasMaxLength(100).HasComment("Country.");
        entity.Property(e => e.PostalCode).HasMaxLength(20).HasComment("Postal or ZIP code.");
        entity.Property(e => e.FormattedAddress).HasMaxLength(1000).HasComment("Formatted address returned by mapping provider.");
        entity.Property(e => e.Latitude).HasColumnType("numeric(18,10)").HasComment("Latitude coordinate.");
        entity.Property(e => e.Longitude).HasColumnType("numeric(18,10)").HasComment("Longitude coordinate.");
        entity.Property(e => e.PlaceId).HasMaxLength(500).HasComment("Google or mapping provider Place Id.");

        entity.Property(e => e.DefaultPaymentMethodId).HasComment("Default payment method for this location.");
        entity.Property(e => e.DefaultMaterialPricingMatrixId).HasComment("Default material pricing matrix.");
        entity.Property(e => e.DefaultLaborPricingMatrixId).HasComment("Default labor pricing matrix.");
        entity.Property(e => e.DefaultOtherPricingMatrixId).HasComment("Default miscellaneous pricing matrix.");

        entity.Property(e => e.InvoiceEmailTemplateId).HasComment("Default invoice email template.");
        entity.Property(e => e.EstimateEmailTemplateId).HasComment("Default estimate email template.");
        entity.Property(e => e.InvoiceSmsTemplateId).HasComment("Default invoice SMS template.");
        entity.Property(e => e.EstimateSmsTemplateId).HasComment("Default estimate SMS template.");

        entity.Property(e => e.TaxExempt)
            .HasDefaultValue(false)
            .HasComment("Indicates whether this service location is tax exempt.");

        entity.Property(e => e.EmailAllowed)
            .HasDefaultValue(true)
            .HasComment("Whether email communication is permitted.");

        entity.Property(e => e.SmsAllowed)
            .HasDefaultValue(true)
            .HasComment("Whether SMS communication is permitted.");

        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment("Indicates whether this service location is active.");

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .HasComment("Record creation timestamp.");

        entity.Property(e => e.CreatedBy).HasComment("User that created the record.");

        entity.Property(e => e.UpdatedOn)
            .HasColumnType("timestamptz")
            .HasComment("Last update timestamp.");

        entity.Property(e => e.UpdatedBy).HasComment("User that last updated the record.");

        entity.HasOne<CrmCustomer>()
            .WithMany()
            .HasForeignKey(e => e.CustomerId)
            .HasConstraintName("FK_CrmServiceLocation_Customer")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.LocationNumber })
            .IsUnique()
            .HasDatabaseName("UQ_CrmServiceLocation_LocationNumber");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CustomerId, e.LocationSequence })
            .IsUnique()
            .HasDatabaseName("UQ_CrmServiceLocation_Customer_LocationSequence");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CustomerId })
            .HasDatabaseName("IX_CrmServiceLocation_CustomerId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Name })
            .HasDatabaseName("IX_CrmServiceLocation_Name");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.DisplayName })
            .HasDatabaseName("IX_CrmServiceLocation_DisplayName");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.City })
            .HasDatabaseName("IX_CrmServiceLocation_City");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.State })
            .HasDatabaseName("IX_CrmServiceLocation_State");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.PostalCode })
            .HasDatabaseName("IX_CrmServiceLocation_PostalCode");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.PlaceId })
            .HasDatabaseName("IX_CrmServiceLocation_PlaceId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsActive })
            .HasDatabaseName("IX_CrmServiceLocation_IsActive");
    }
}
