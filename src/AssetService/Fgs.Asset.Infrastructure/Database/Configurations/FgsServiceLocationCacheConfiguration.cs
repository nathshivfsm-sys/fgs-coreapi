using Fgs.Asset.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Asset.Infrastructure.Database.Configurations;

internal sealed class FgsServiceLocationCacheConfiguration : IEntityTypeConfiguration<FgsServiceLocationCache>
{
    public void Configure(EntityTypeBuilder<FgsServiceLocationCache> entity)
    {
        entity.ToTable(
            "FgsServiceLocationCache",
            t => t.HasComment(
                "Local cache of CRM service location information used by the asset schema to eliminate cross-schema dependencies on crm.CrmServiceLocation."));

        entity.HasKey(e => new { e.TenantId, e.CompanyId, e.ServiceLocationId })
            .HasName("PK_FgsServiceLocationCache");

        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");
        entity.Property(e => e.ServiceLocationId).HasComment("Service location identifier mapped from crm.CrmServiceLocation.Id.");
        entity.Property(e => e.CustomerId).HasComment("Customer identifier that owns the service location.");
        entity.Property(e => e.LocationSequence).HasComment("Sequential location number within the customer.");
        entity.Property(e => e.LocationNumber).HasMaxLength(50).IsRequired()
            .HasComment("User-visible service location number.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz")
            .HasComment("Timestamp of the most recent synchronization from crm.CrmServiceLocation.");

        entity.ConfigureTenantCompanyCacheFk("FK_FgsServiceLocationCache_FgsTenantCompanyCache_TenantId_CompanyId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CustomerId })
            .HasDatabaseName("IX_FgsServiceLocationCache_TenantId_CompanyId_CustomerId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.LocationNumber })
            .HasDatabaseName("IX_FgsServiceLocationCache_TenantId_CompanyId_LocationNumber");
    }
}
