using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsJobTypeCategoryConfiguration : IEntityTypeConfiguration<FgsJobTypeCategory>
{
    public void Configure(EntityTypeBuilder<FgsJobTypeCategory> entity)
    {
        entity.ToTable("FgsJobTypeCategory", t =>
            t.HasComment(
                "Maps Job Categories to Job Types. A Job Type can contain one or more Job Categories, each with its own display order."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .UseIdentityByDefaultColumn()
            .HasComment("Unique identifier for the Job Type Category mapping.");

        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);

        entity.Property(e => e.TenantId)
            .HasComment("Identifier of the tenant that owns this Job Type Category mapping.");

        entity.Property(e => e.CompanyId)
            .HasComment("Identifier of the company within the tenant that owns this Job Type Category mapping.");

        entity.Property(e => e.JobTypeId)
            .HasComment("Identifier of the Job Type.");

        entity.Property(e => e.JobCategoryId)
            .HasComment("Identifier of the Job Category assigned to the Job Type.");

        entity.Property(e => e.DisplayOrder)
            .HasDefaultValue((short)1)
            .HasComment("Controls the display sequence of Job Categories within the Job Type.");

        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the Job Category assignment is active.");

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .HasComment("Date and time when the mapping was created.");

        entity.Property(e => e.CreatedBy)
            .HasMaxLength(100)
            .HasComment("User who created the mapping.");

        entity.Property(e => e.UpdatedOn)
            .HasColumnType("timestamptz")
            .HasComment("Date and time when the mapping was last modified.");

        entity.Property(e => e.UpdatedBy)
            .HasMaxLength(100)
            .HasComment("User who last modified the mapping.");

        entity.HasOne(e => e.JobType)
            .WithMany(e => e.JobTypeCategories)
            .HasForeignKey(e => e.JobTypeId)
            .HasConstraintName("FK_FgsJobTypeCategory_FgsJobType")
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(e => e.JobCategory)
            .WithMany()
            .HasForeignKey(e => e.JobCategoryId)
            .HasConstraintName("FK_FgsJobTypeCategory_FgsJobCategory")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsJobTypeCategory_Tenant_Company");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.JobTypeId })
            .HasDatabaseName("IX_FgsJobTypeCategory_Tenant_Company_JobType");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.JobCategoryId })
            .HasDatabaseName("IX_FgsJobTypeCategory_Tenant_Company_JobCategory");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.JobTypeId, e.JobCategoryId })
            .IsUnique()
            .HasDatabaseName("UX_FgsJobTypeCategory_Tenant_Company_JobType_Category");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.JobTypeId, e.DisplayOrder })
            .HasDatabaseName("IX_FgsJobTypeCategory_Tenant_Company_DisplayOrder");
    }
}
