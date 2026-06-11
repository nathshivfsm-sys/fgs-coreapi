using Fgs.Crm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Crm.Infrastructure.Database.Configurations;

internal sealed class CrmCustomerConfiguration : IEntityTypeConfiguration<CrmCustomer>
{
    public void Configure(EntityTypeBuilder<CrmCustomer> entity)
    {
        entity.ToTable("CrmCustomer");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.CustomerNumber).HasMaxLength(30).IsRequired();
        entity.Property(e => e.LastServiceLocationSequence).HasDefaultValue(0);
        entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
        entity.Property(e => e.DisplayName).HasMaxLength(200).IsRequired();
        entity.Property(e => e.AddressLine1).HasMaxLength(200);
        entity.Property(e => e.AddressLine2).HasMaxLength(200);
        entity.Property(e => e.AddressLine3).HasMaxLength(200);
        entity.Property(e => e.AddressLine4).HasMaxLength(200);
        entity.Property(e => e.City).HasMaxLength(100);
        entity.Property(e => e.State).HasMaxLength(100);
        entity.Property(e => e.County).HasMaxLength(100);
        entity.Property(e => e.Country).HasMaxLength(100);
        entity.Property(e => e.PostalCode).HasMaxLength(20);
        entity.Property(e => e.FormattedAddress).HasMaxLength(1000);
        entity.Property(e => e.Latitude).HasColumnType("numeric(18,10)");
        entity.Property(e => e.Longitude).HasColumnType("numeric(18,10)");
        entity.Property(e => e.PlaceId).HasMaxLength(500);
        entity.Property(e => e.DefaultPORequired).HasDefaultValue(false);
        entity.Property(e => e.TaxExempt).HasDefaultValue(false);
        entity.Property(e => e.TaxExemptNumber).HasMaxLength(100);
        entity.Property(e => e.CustomerAccountNumber).HasMaxLength(100);
        entity.Property(e => e.ExternalEntityId).HasMaxLength(200);
        entity.Property(e => e.ExternalVersion).HasMaxLength(100);
        entity.Property(e => e.IsActive).HasDefaultValue(true);

        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");

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
