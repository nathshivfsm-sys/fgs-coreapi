using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsBillingCategoryConfiguration : IEntityTypeConfiguration<FgsBillingCategory>
{
    public void Configure(EntityTypeBuilder<FgsBillingCategory> entity)
    {
        entity.ToTable("FgsBillingCategory", t =>
            t.HasComment(
                "Stores tenant/company specific billing categories used for invoicing, service billing, maintenance plans, and other billing operations. Seeded initially from GloBillingCategory but fully managed by each tenant/company independently."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .UseIdentityByDefaultColumn()
            .HasComment("Primary key identity of the billing category record.");

        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);

        entity.Property(e => e.TenantId)
            .HasComment("Tenant identifier owning this billing category.");

        entity.Property(e => e.CompanyId)
            .HasComment("Company identifier within the tenant owning this billing category.");

        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.BillingCategoryType, e.BillingCategoryName })
            .HasName("UQ_FgsBillingCategory_TenantId_CompanyId_BillingCategoryType");

        entity.Property(e => e.BillingCategoryType)
            .HasMaxLength(2)
            .HasComment("Short billing category code such as IN, PM, SR, or other tenant-defined values.");

        entity.Property(e => e.BillingCategoryName)
            .HasMaxLength(100)
            .HasComment("Display name of the billing category shown throughout the application.");

        entity.Property(e => e.Description)
            .HasColumnType("text")
            .HasComment("Optional internal description or notes for the billing category.");

        entity.Property(e => e.DisplayOrder)
            .HasColumnType("smallint")
            .HasDefaultValue((short)1)
            .HasComment("Controls sorting/display order of billing categories in dropdowns and setup screens.");

        entity.Property(e => e.IsSystemDefined)
            .HasDefaultValue(false)
            .HasComment("Indicates whether the billing category was system seeded or manually created by the tenant/company.");

        entity.Property(e => e.ShowToFieldTech)
            .HasDefaultValue(false)
            .HasComment(
                "Indicates whether the billing category is visible to field technicians in mobile and field service applications.");

        entity.Property(e => e.AllowToPick)
            .HasDefaultValue(true)
            .HasComment(
                "Determines whether office users are allowed to manually select this billing category during estimate, invoice, or billing entry. Categories such as tax may be system controlled and not manually selectable.");

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .HasComment("Date and time the billing category record was created.");

        entity.Property(e => e.CreatedBy)
            .HasMaxLength(100)
            .HasComment("User identifier that created the billing category record.");

        entity.Property(e => e.UpdatedOn)
            .HasColumnType("timestamptz")
            .HasComment("Date and time the billing category record was last updated.");

        entity.Property(e => e.UpdatedBy)
            .HasMaxLength(100)
            .HasComment("User identifier that last updated the billing category record.");

        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the billing category is active and available for use.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsActive })
            .HasDatabaseName("IX_FgsBillingCategory_TenantId_CompanyId_IsActive");
    }
}
