using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsJobTypeConfiguration : IEntityTypeConfiguration<FgsJobType>
{
    public void Configure(EntityTypeBuilder<FgsJobType> entity)
    {
        entity.ToTable("FgsJobType", t =>
        {
            t.HasComment(
                "Defines reusable Job Types that represent the type of work performed. A Job Type serves as the header for one or more Job Type Categories and their associated tasks.");
            t.HasCheckConstraint(
                "CK_FgsJobType_UsedFor",
                "\"UsedFor\" IN (1, 2, 3, 4)");
        });

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .UseIdentityByDefaultColumn()
            .HasComment("Unique identifier for the Job Type.");

        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);

        entity.Property(e => e.TenantId)
            .HasComment("Identifier of the tenant that owns this Job Type.");

        entity.Property(e => e.CompanyId)
            .HasComment("Identifier of the company within the tenant that owns this Job Type.");

        entity.Property(e => e.JobTypeCode)
            .HasMaxLength(50)
            .HasComment("Unique business code used to identify the Job Type within a tenant and company.");

        entity.Property(e => e.Name)
            .HasMaxLength(200)
            .HasComment("Display name of the Job Type shown throughout the application.");

        entity.Property(e => e.UsedFor)
            .HasConversion<short>()
            .HasColumnType("smallint")
            .HasComment(
                "Specifies the business process for the Job Type. Valid values: 1=Service, 2=Maintenance, 3=Warranty, 4=Installation. Corresponds to the JobTypeUsedFor enum in the application.");

        entity.Property(e => e.BusinessUnit)
            .HasMaxLength(100)
            .HasComment("Optional business unit or department responsible for this Job Type.");

        entity.Property(e => e.BackgroundColor)
            .HasMaxLength(20)
            .HasComment("Optional background color used when displaying the Job Type in the user interface.");

        entity.Property(e => e.TextColor)
            .HasMaxLength(20)
            .HasComment("Optional text color used when displaying the Job Type in the user interface.");

        entity.Property(e => e.ShowToFieldTech)
            .HasDefaultValue(true)
            .HasComment("Indicates whether this Job Type is visible to field technicians in the mobile application.");

        entity.Property(e => e.ShowOnCustomerPortal)
            .HasDefaultValue(true)
            .HasComment("Indicates whether this Job Type is available for customers through the customer portal.");

        entity.Property(e => e.DisplayOrder)
            .HasDefaultValue((short)1)
            .HasComment("Controls the display sequence of Job Types in lists and selection controls.");

        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the Job Type is active and available for new work orders.");

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .HasComment("Date and time when the Job Type was created.");

        entity.Property(e => e.CreatedBy)
            .HasMaxLength(100)
            .HasComment("User who created the Job Type.");

        entity.Property(e => e.UpdatedOn)
            .HasColumnType("timestamptz")
            .HasComment("Date and time when the Job Type was last modified.");

        entity.Property(e => e.UpdatedBy)
            .HasMaxLength(100)
            .HasComment("User who last modified the Job Type.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsJobType_Tenant_Company");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.BusinessUnit })
            .HasDatabaseName("IX_FgsJobType_Tenant_Company_BusinessUnit");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.UsedFor })
            .HasDatabaseName("IX_FgsJobType_Tenant_Company_UsedFor");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.JobTypeCode })
            .IsUnique()
            .HasDatabaseName("UX_FgsJobType_Tenant_Company_JobTypeCode");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Name })
            .IsUnique()
            .HasDatabaseName("UX_FgsJobType_Tenant_Company_Name");
    }
}
