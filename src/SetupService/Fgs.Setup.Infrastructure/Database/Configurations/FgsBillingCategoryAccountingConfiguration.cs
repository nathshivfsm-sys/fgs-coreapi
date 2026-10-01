using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsBillingCategoryAccountingConfiguration : IEntityTypeConfiguration<FgsBillingCategoryAccounting>
{
    public void Configure(EntityTypeBuilder<FgsBillingCategoryAccounting> entity)
    {
        entity.ToTable("FgsBillingCategoryAccounting", t =>
            t.HasComment(
                "Stores optional accounting configuration for tenant/company specific billing categories, including Revenue, COGS, and Inventory Offset GL account mappings. Accounting configuration is maintained separately from the billing category so it can remain optional."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .UseIdentityByDefaultColumn()
            .HasComment("Primary key identity of the billing category accounting record.");

        entity.Property(e => e.TenantId)
            .HasComment("Tenant identifier owning this billing category accounting configuration.");

        entity.Property(e => e.CompanyId)
            .HasComment("Company identifier within the tenant owning this billing category accounting configuration.");

        entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.BillingCategoryId })
            .HasName("UQ_FgsBillingCategoryAccounting_TenantId_CompanyId_BillingCategoryId");

        entity.Property(e => e.BillingCategoryId)
            .HasComment("Identifier of the billing category to which the accounting configuration applies.");

        entity.Property(e => e.RevenueAccountId)
            .HasComment("Optional GL account used to record revenue generated from the billing category.");

        entity.Property(e => e.CogsAccountId)
            .HasComment("Optional GL account used to record cost of goods sold associated with the billing category.");

        entity.Property(e => e.InventoryOffsetAccountId)
            .HasComment(
                "Optional GL account used as the inventory offset account for inventory billing categories. This account applies only when the associated billing category is an Inventory billing category.");

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .HasComment("Date and time the billing category accounting configuration was created.");

        entity.Property(e => e.CreatedBy)
            .HasMaxLength(100)
            .HasComment("User identifier that created the billing category accounting configuration.");

        entity.Property(e => e.UpdatedOn)
            .HasColumnType("timestamptz")
            .HasComment("Date and time the billing category accounting configuration was last updated.");

        entity.Property(e => e.UpdatedBy)
            .HasMaxLength(100)
            .HasComment("User identifier that last updated the billing category accounting configuration.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsBillingCategoryAccounting_TenantId_CompanyId");

        entity.HasIndex(e => e.BillingCategoryId)
            .HasDatabaseName("IX_FgsBillingCategoryAccounting_BillingCategoryId");

        entity.HasOne<FgsBillingCategory>()
            .WithMany()
            .HasForeignKey(e => e.BillingCategoryId)
            .HasConstraintName("FK_FgsBillingCategoryAccounting_FgsBillingCategory")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
