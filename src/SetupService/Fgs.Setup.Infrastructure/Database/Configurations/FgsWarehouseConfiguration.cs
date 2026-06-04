using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsWarehouseConfiguration : IEntityTypeConfiguration<FgsWarehouse>
{
    public void Configure(EntityTypeBuilder<FgsWarehouse> entity)
    {
        entity.ToTable(
            "FgsWarehouse",
            t =>
            {
                t.HasComment(
                    "Stores inventory warehouse, truck, trailer, job site, consignment, and vendor storage locations.");
                t.HasCheckConstraint(
                    "CK_FgsWarehouse_WarehouseType",
                    "\"WarehouseType\" IN ('Warehouse', 'Truck', 'Trailer', 'JobSite', 'Consignment', 'Vendor')");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .UseIdentityByDefaultColumn()
            .HasComment("Primary key.");

        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);

        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");

        entity.Property(e => e.WarehouseCode)
            .HasMaxLength(50)
            .IsRequired()
            .HasComment("Unique warehouse code within the tenant and company scope.");

        entity.Property(e => e.Name)
            .HasMaxLength(200)
            .IsRequired()
            .HasComment("Display name of the warehouse or inventory location.");

        entity.Property(e => e.WarehouseType)
            .HasMaxLength(30)
            .IsRequired()
            .HasComment(
                "Type of inventory location. Allowed values: Warehouse, Truck, Trailer, JobSite, Consignment, Vendor.");

        entity.Property(e => e.AddressId)
            .HasComment("Optional reference to the warehouse address record.");

        entity.Property(e => e.Description)
            .HasColumnType("text")
            .HasComment("Optional description or notes for the warehouse.");

        entity.Property(e => e.IsDefault)
            .HasDefaultValue(false)
            .HasComment("Indicates whether this warehouse is the default inventory location for the company.");

        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the warehouse is active and available for inventory operations.");

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");

        entity.Property(e => e.CreatedBy)
            .HasMaxLength(100)
            .HasComment("User who created the record.");

        entity.Property(e => e.UpdatedOn)
            .HasColumnType("timestamptz")
            .HasComment("Date and time the record was last updated.");

        entity.Property(e => e.UpdatedBy)
            .HasMaxLength(100)
            .HasComment("User who last updated the record.");

        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.WarehouseCode })
            .HasName("UQ_FgsWarehouse_TenantId_CompanyId_WarehouseCode");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsActive })
            .HasDatabaseName("IX_FgsWarehouse_TenantId_CompanyId_IsActive");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.WarehouseType })
            .HasDatabaseName("IX_FgsWarehouse_TenantId_CompanyId_WarehouseType");    }
}
