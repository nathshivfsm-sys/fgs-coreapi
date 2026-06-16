using Fgs.Crm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Crm.Infrastructure.Database.Configurations;

internal sealed class FgsEstimateTemplateOptionConfiguration : IEntityTypeConfiguration<FgsEstimateTemplateOption>
{
    public void Configure(EntityTypeBuilder<FgsEstimateTemplateOption> entity)
    {
        entity.ToTable(
            "FgsEstimateTemplateOption",
            t =>
            {
                t.HasComment(
                    "Stores reusable estimate options belonging to an estimate template. Template options are copied into estimate options when a template is applied.");
                t.HasCheckConstraint("CK_FgsEstimateTemplateOption_DisplayOrder", "\"DisplayOrder\" > 0");
            });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityAlwaysColumn().HasComment("Primary key.");
        entity.ConfigureTenantCompanyColumns();

        entity.Property(e => e.TenantId).HasComment("Tenant identifier.");
        entity.Property(e => e.CompanyId).HasComment("Company identifier.");
        entity.Property(e => e.EstimateTemplateId).HasComment("Parent estimate template.");
        entity.Property(e => e.EstimateFlavorId).HasComment(
            "Flavor assigned to the option such as Standard, Good, Better, Best, or Add-On.");
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1)
            .HasComment("Controls display sequence within the template.");
        entity.Property(e => e.OptionName).HasMaxLength(255).IsRequired()
            .HasComment("Customer-facing option name copied to the estimate option.");
        entity.Property(e => e.OptionDescription).HasColumnType("text")
            .HasComment("Customer-facing option description copied to the estimate option.");
        entity.Property(e => e.ShowOnProposal).HasDefaultValue(true)
            .HasComment("Indicates whether the option should be displayed on customer-facing proposals.");
        entity.Property(e => e.ShowPriceOnProposal).HasDefaultValue(true)
            .HasComment("Indicates whether pricing should be displayed on customer-facing proposals.");
        entity.Property(e => e.IsSelectedByDefault).HasDefaultValue(false)
            .HasComment("Indicates whether the option should be selected by default when the template is applied.");
        entity.Property(e => e.AllowQuantityChange).HasDefaultValue(true)
            .HasComment("Indicates whether quantity may be modified after template application.");
        entity.Property(e => e.AllowPriceChange).HasDefaultValue(true)
            .HasComment("Indicates whether pricing may be modified after template application.");

        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()")
            .HasComment("Date and time the record was created.");
        entity.Property(e => e.CreatedBy).HasComment("User or process that created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz")
            .HasComment("Date and time the record was last updated.");
        entity.Property(e => e.UpdatedBy).HasComment("User or process that last updated the record.");

        entity.HasOne<FgsEstimateTemplate>()
            .WithMany()
            .HasForeignKey(e => e.EstimateTemplateId)
            .HasConstraintName("FK_FgsEstimateTemplateOption_EstimateTemplate")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne<FgsEstimateFlavor>()
            .WithMany()
            .HasForeignKey(e => e.EstimateFlavorId)
            .HasConstraintName("FK_FgsEstimateTemplateOption_EstimateFlavor")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsEstimateTemplateOption_TenantId_CompanyId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.EstimateTemplateId })
            .HasDatabaseName("IX_FgsEstimateTemplateOption_TenantId_CompanyId_EstimateTemplateId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.EstimateFlavorId })
            .HasDatabaseName("IX_FgsEstimateTemplateOption_TenantId_CompanyId_EstimateFlavorId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.EstimateTemplateId, e.DisplayOrder })
            .IsUnique()
            .HasDatabaseName("UX_FgsEstimateTemplateOption_TenantId_CompanyId_EstimateTemplateId_DisplayOrder");
    }
}
