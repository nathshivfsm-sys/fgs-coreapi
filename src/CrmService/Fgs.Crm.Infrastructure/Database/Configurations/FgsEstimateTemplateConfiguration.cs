using Fgs.Crm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Crm.Infrastructure.Database.Configurations;

internal sealed class FgsEstimateTemplateConfiguration : IEntityTypeConfiguration<FgsEstimateTemplate>
{
    public void Configure(EntityTypeBuilder<FgsEstimateTemplate> entity)
    {
        entity.ToTable(
            "FgsEstimateTemplate",
            t =>
            {
                t.HasComment(
                    "Stores reusable estimate templates used to generate estimate options and pricing lines.");
                t.HasCheckConstraint("CK_FgsEstimateTemplate_DisplayOrder", "\"DisplayOrder\" > 0");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityAlwaysColumn().HasComment("Primary key.");
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");
        entity.Property(e => e.CategoryId).HasComment("Template category.");
        entity.Property(e => e.TemplateCode).HasMaxLength(50).IsRequired()
            .HasComment("Unique internal template code within a company.");
        entity.Property(e => e.Name).HasMaxLength(255).IsRequired()
            .HasComment("User-facing template name.");
        entity.Property(e => e.TemplateDescription).HasColumnType("text")
            .HasComment("Description copied into estimate description when estimate is generated from template.");
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1)
            .HasComment("Controls display sequence within a category.");
        entity.Property(e => e.ShowToFieldTechnician).HasDefaultValue(true)
            .HasComment("Indicates whether template-generated content should be visible to field technicians.");
        entity.Property(e => e.IsActive).HasDefaultValue(true)
            .HasComment("Indicates whether template is available for use.");

        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).HasComment("User or process that created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz")
            .HasComment("Date and time the record was last updated.");
        entity.Property(e => e.UpdatedBy).HasComment("User or process that last updated the record.");

        entity.HasOne<FgsEstimateTemplateCategory>()
            .WithMany()
            .HasForeignKey(e => e.CategoryId)
            .HasConstraintName("FK_FgsEstimateTemplate_Category")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.TemplateCode })
            .IsUnique()
            .HasDatabaseName("UX_FgsEstimateTemplate_TenantId_CompanyId_TemplateCode");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CategoryId, e.Name })
            .IsUnique()
            .HasDatabaseName("UX_FgsEstimateTemplate_TenantId_CompanyId_CategoryId_Name");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsEstimateTemplate_TenantId_CompanyId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CategoryId })
            .HasDatabaseName("IX_FgsEstimateTemplate_TenantId_CompanyId_CategoryId");
    }
}
