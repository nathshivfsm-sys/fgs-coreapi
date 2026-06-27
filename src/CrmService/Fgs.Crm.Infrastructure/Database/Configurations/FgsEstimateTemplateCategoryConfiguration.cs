using Fgs.Crm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Crm.Infrastructure.Database.Configurations;

internal sealed class FgsEstimateTemplateCategoryConfiguration : IEntityTypeConfiguration<FgsEstimateTemplateCategory>
{
    public void Configure(EntityTypeBuilder<FgsEstimateTemplateCategory> entity)
    {
        entity.ToTable(
            "FgsEstimateTemplateCategory",
            t =>
            {
                t.HasComment(
                    "Stores estimate template categories used to organize estimate templates into logical groups.");
                t.HasCheckConstraint("CK_FgsEstimateTemplateCategory_DisplayOrder", "\"DisplayOrder\" > 0");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityAlwaysColumn().HasComment("Primary key.");
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");
        entity.Property(e => e.CategoryCode).HasMaxLength(50).IsRequired()
            .HasComment("Unique internal category code within a company.");
        entity.Property(e => e.Name).HasMaxLength(100).IsRequired()
            .HasComment("User-facing category name.");
        entity.Property(e => e.Description).HasMaxLength(500)
            .HasComment("Optional category description.");
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1)
            .HasComment("Controls display sequence of categories.");

        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).HasComment("User or process that created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz")
            .HasComment("Date and time the record was last updated.");
        entity.Property(e => e.UpdatedBy).HasComment("User or process that last updated the record.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CategoryCode })
            .IsUnique()
            .HasDatabaseName("UX_FgsEstimateTemplateCategory_TenantId_CompanyId_CategoryCode");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Name })
            .IsUnique()
            .HasDatabaseName("UX_FgsEstimateTemplateCategory_TenantId_CompanyId_Name");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsEstimateTemplateCategory_TenantId_CompanyId");
    }
}
