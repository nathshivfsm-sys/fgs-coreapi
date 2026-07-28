using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsJobCategoryConfiguration : IEntityTypeConfiguration<FgsJobCategory>
{
    public void Configure(EntityTypeBuilder<FgsJobCategory> entity)
    {
        entity.ToTable("FgsJobCategory", t =>
            t.HasComment(
                "Stores the master list of Job Categories available for configuring Job Types. Categories organize related Job Tasks within a Job Type."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .UseIdentityByDefaultColumn()
            .HasComment("Unique identifier for the Job Category.");

        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);

        entity.Property(e => e.TenantId)
            .HasComment("Identifier of the tenant that owns this Job Category.");

        entity.Property(e => e.CompanyId)
            .HasComment("Identifier of the company within the tenant that owns this Job Category.");

        entity.Property(e => e.CategoryCode)
            .HasMaxLength(50)
            .HasComment("Unique business code used to identify the Job Category within a tenant and company.");

        entity.Property(e => e.Name)
            .HasMaxLength(150)
            .HasComment("Display name of the Job Category.");

        entity.Property(e => e.DisplayOrder)
            .HasDefaultValue((short)1)
            .HasComment("Controls the display sequence of Job Categories in lists and selection controls.");

        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the Job Category is active and available for selection.");

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .HasComment("Date and time when the Job Category was created.");

        entity.Property(e => e.CreatedBy)
            .HasMaxLength(100)
            .HasComment("User who created the Job Category.");

        entity.Property(e => e.UpdatedOn)
            .HasColumnType("timestamptz")
            .HasComment("Date and time when the Job Category was last modified.");

        entity.Property(e => e.UpdatedBy)
            .HasMaxLength(100)
            .HasComment("User who last modified the Job Category.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsJobCategory_Tenant_Company");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CategoryCode })
            .IsUnique()
            .HasDatabaseName("UX_FgsJobCategory_Tenant_Company_CategoryCode");
    }
}
