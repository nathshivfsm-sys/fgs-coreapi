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
            t =>
            {
                t.HasComment(
                    "Physical customer location where field service work is performed.");
                t.HasCheckConstraint(
                    "CK_CrmServiceLocation_ServiceLocationType",
                    "\"ServiceLocationType\" IN (1, 2, 3, 4, 5)");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .UseIdentityByDefaultColumn()
            .HasComment("Primary key for the service location.");
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier within the tenant.");
        entity.Property(e => e.CustomerId).HasComment("Customer that owns this service location.");

        entity.Property(e => e.LocationSequence)
            .HasComment("Sequential service location number within a customer.");

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
            .HasComment("Service location name displayed to users and customers.");

        entity.Property(e => e.ServiceLocationType)
            .HasConversion<short>()
            .HasColumnType("smallint")
            .IsRequired()
            .HasComment(
                "Specifies the type of the service location. Valid values: 1=Residential, 2=Commercial, 3=Industrial, 4=Government, 5=Other. Corresponds to the ServiceLocationType enum in the application.");

        entity.Property(e => e.AddressLine1)
            .HasMaxLength(200)
            .HasComment("Primary service location address line.");
        entity.Property(e => e.AddressLine2)
            .HasMaxLength(200)
            .HasComment("Secondary service location address line.");
        entity.Property(e => e.AddressLine3)
            .HasMaxLength(200)
            .HasComment("Additional service location address information.");
        entity.Property(e => e.AddressLine4)
            .HasMaxLength(200)
            .HasComment("Additional service location address information.");
        entity.Property(e => e.City)
            .HasMaxLength(100)
            .HasComment("Service location city.");
        entity.Property(e => e.State)
            .HasMaxLength(100)
            .HasComment("Service location state or province.");
        entity.Property(e => e.County)
            .HasMaxLength(100)
            .HasComment("Service location county or district.");
        entity.Property(e => e.Country)
            .HasMaxLength(100)
            .HasComment("Service location country.");
        entity.Property(e => e.PostalCode)
            .HasMaxLength(20)
            .HasComment("Service location postal or ZIP code.");
        entity.Property(e => e.FormattedAddress)
            .HasMaxLength(1000)
            .HasComment("Formatted service location address returned or constructed by the address or mapping provider.");
        entity.Property(e => e.Latitude)
            .HasColumnType("numeric(18,10)")
            .HasComment("Latitude coordinate associated with the service location.");
        entity.Property(e => e.Longitude)
            .HasColumnType("numeric(18,10)")
            .HasComment("Longitude coordinate associated with the service location.");
        entity.Property(e => e.PlaceId)
            .HasMaxLength(500)
            .HasComment("Place identifier returned by the address or mapping provider.");

        entity.Property(e => e.DefaultPaymentMethodId)
            .HasComment("Default payment method assigned to the service location.");
        entity.Property(e => e.DefaultMaterialPricingMatrixId)
            .HasComment("Default material pricing matrix assigned to the service location.");
        entity.Property(e => e.DefaultLaborPricingMatrixId)
            .HasComment("Default labor pricing matrix assigned to the service location.");
        entity.Property(e => e.DefaultOtherPricingMatrixId)
            .HasComment("Default miscellaneous or other pricing matrix assigned to the service location.");

        entity.Property(e => e.InvoiceEmailTemplateId)
            .HasComment("Default email template used when sending invoices for the service location.");
        entity.Property(e => e.EstimateEmailTemplateId)
            .HasComment("Default email template used when sending estimates for the service location.");
        entity.Property(e => e.InvoiceSmsTemplateId)
            .HasComment("Default SMS template used when sending invoices for the service location.");
        entity.Property(e => e.EstimateSmsTemplateId)
            .HasComment("Default SMS template used when sending estimates for the service location.");

        entity.Property(e => e.TaxExempt)
            .HasDefaultValue(false)
            .HasComment("Indicates whether the service location is tax exempt.");

        entity.Property(e => e.EmailAllowed)
            .HasDefaultValue(true)
            .HasComment("Indicates whether email communication is permitted for the service location.");

        entity.Property(e => e.SmsAllowed)
            .HasDefaultValue(true)
            .HasComment("Indicates whether SMS communication is permitted for the service location.");

        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the service location is active.");

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .HasComment("Timestamp when the service location was created.");

        entity.Property(e => e.CreatedBy)
            .HasComment("User or process that created the service location.");

        entity.Property(e => e.UpdatedOn)
            .HasColumnType("timestamptz")
            .HasComment("Timestamp when the service location was last updated.");

        entity.Property(e => e.UpdatedBy)
            .HasComment("User or process that last updated the service location.");

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
