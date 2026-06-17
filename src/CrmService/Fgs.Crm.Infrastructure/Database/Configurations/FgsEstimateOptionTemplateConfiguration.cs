using Fgs.Crm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Crm.Infrastructure.Database.Configurations;

internal sealed class FgsEstimateOptionTemplateConfiguration : IEntityTypeConfiguration<FgsEstimateOptionTemplate>
{
    public void Configure(EntityTypeBuilder<FgsEstimateOptionTemplate> entity)
    {
        entity.ToTable(
            "FgsEstimateOptionTemplate",
            t =>
            {
                t.HasComment(
                    "Stores estimate templates applied to an estimate option and tracks template contributions to pricing lines, clauses, and other estimate content.");
                t.HasCheckConstraint("CK_FgsEstimateOptionTemplate_DisplayOrder", "\"DisplayOrder\" > 0");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityAlwaysColumn().HasComment("Primary key.");
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");
        entity.Property(e => e.EstimateOptionId).HasComment("Parent estimate option.");
        entity.Property(e => e.EstimateTemplateId).HasComment("Source estimate template applied to the estimate option.");
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1)
            .HasComment("Order in which templates were applied to the estimate option.");
        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).HasComment("User or process that created the record.");

        entity.HasOne<FgsEstimateOption>()
            .WithMany()
            .HasForeignKey(e => e.EstimateOptionId)
            .HasConstraintName("FK_FgsEstimateOptionTemplate_EstimateOption")
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne<FgsEstimateTemplate>()
            .WithMany()
            .HasForeignKey(e => e.EstimateTemplateId)
            .HasConstraintName("FK_FgsEstimateOptionTemplate_EstimateTemplate")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.EstimateOptionId, e.EstimateTemplateId })
            .IsUnique()
            .HasDatabaseName("UX_FgsEstimateOptionTemplate_TenantId_CompanyId_OptionId_TemplateId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsEstimateOptionTemplate_TenantId_CompanyId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.EstimateOptionId })
            .HasDatabaseName("IX_FgsEstimateOptionTemplate_TenantId_CompanyId_EstimateOptionId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.EstimateTemplateId })
            .HasDatabaseName("IX_FgsEstimateOptionTemplate_TenantId_CompanyId_EstimateTemplateId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.EstimateOptionId, e.DisplayOrder })
            .HasDatabaseName("IX_FgsEstimateOptionTemplate_TenantId_CompanyId_DisplayOrder");
    }
}
