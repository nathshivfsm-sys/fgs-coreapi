using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal sealed class FgsPriceBookConfiguration : IEntityTypeConfiguration<FgsPriceBook>
{
    public void Configure(EntityTypeBuilder<FgsPriceBook> entity)
    {
        entity.ToTable("FgsPriceBook", t =>
        {
            t.HasComment(
                "Defines the master catalog of services offered by a company. Each price book header represents a reusable service template used by estimates, work orders, invoices, scheduling, and pricing.");
            t.HasCheckConstraint(
                "CK_FgsPriceBook_PricingModel",
                "\"PricingModel\" IN ('Flat Rate', 'Dynamic')");
        });

        entity.HasKey(e => e.Id).HasName("PK_FgsPriceBook");
        entity.Property(e => e.Id)
            .UseIdentityByDefaultColumn()
            .HasComment("Unique identifier of the price book record.");

        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);

        entity.Property(e => e.TenantId)
            .HasComment("Identifier of the tenant that owns the record.");
        entity.Property(e => e.CompanyId)
            .HasComment("Identifier of the company that owns the record.");
        entity.Property(e => e.PriceBookCode).HasMaxLength(50).IsRequired()
            .HasComment("Unique business code of the price book item.");
        entity.Property(e => e.PriceBookName).HasMaxLength(200).IsRequired()
            .HasComment("Display name of the service offered in the price book.");
        entity.Property(e => e.Description).HasColumnType("text")
            .HasComment("Detailed description of the service.");
        entity.Property(e => e.JobTypeId).IsRequired()
            .HasComment("Default work order type associated with this service.");
        entity.Property(e => e.PricingModel).HasMaxLength(20).IsRequired()
            .HasComment("Determines whether pricing is Flat Rate or Dynamic.");
        entity.Property(e => e.EstimatedDurationMinutes).HasDefaultValue(60).IsRequired()
            .HasComment("Estimated time in minutes required to complete the service.");
        entity.Property(e => e.BasePrice).HasPrecision(18, 2)
            .HasComment("Base selling price when the pricing model is Flat Rate. Null for Dynamic pricing.");
        entity.Property(e => e.IsTaxable).HasDefaultValue(true).IsRequired()
            .HasComment("Indicates whether the service is taxable by default.");
        entity.Property(e => e.IsActive).HasDefaultValue(true).IsRequired()
            .HasComment("Indicates whether the price book item is active and available for use.");
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .HasComment("Date and time when the record was created.");
        entity.Property(e => e.CreatedBy).HasMaxLength(100)
            .HasComment("User who created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz")
            .HasComment("Date and time when the record was last updated.");
        entity.Property(e => e.UpdatedBy).HasMaxLength(100)
            .HasComment("User who last updated the record.");

        entity.HasOne(e => e.JobType)
            .WithMany()
            .HasForeignKey(e => e.JobTypeId)
            .HasConstraintName("FK_FgsPriceBook_JobTypeId")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasMany(e => e.Items)
            .WithOne(i => i.PriceBook)
            .HasForeignKey(i => i.PriceBookId)
            .HasConstraintName("FK_FgsPriceBookItem_FgsPriceBook")
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsPriceBook_Tenant_Company");
        entity.HasIndex(e => e.JobTypeId)
            .HasDatabaseName("IX_FgsPriceBook_JobTypeId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsActive })
            .HasDatabaseName("IX_FgsPriceBook_Tenant_Company_IsActive");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.PriceBookCode })
            .IsUnique()
            .HasDatabaseName("UX_FgsPriceBook_Tenant_Company_PriceBookCode");
    }
}
