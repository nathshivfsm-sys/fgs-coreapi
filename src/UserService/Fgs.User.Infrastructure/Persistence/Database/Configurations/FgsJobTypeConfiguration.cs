using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Persistence.Database.Configurations;

internal class FgsJobTypeConfiguration : IEntityTypeConfiguration<FgsJobType>
{
    public void Configure(EntityTypeBuilder<FgsJobType> entity)
    {
        entity.ToTable("FgsJobType");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);
        entity.ConfigureTenantCompanySetupFk("FK_FgsJobType_FgsTenantCompany_TenantId_CompanyId");
        entity.Property(e => e.JobTypeCode).HasMaxLength(50);
        entity.Property(e => e.TaskName).HasMaxLength(200);
        entity.Property(e => e.Description).HasColumnType("text");
        entity.Property(e => e.UsedFor).HasMaxLength(50);
        entity.Property(e => e.Trade).HasMaxLength(100);
        entity.Property(e => e.BusinessUnit).HasMaxLength(100);
        entity.Property(e => e.Priority).HasDefaultValue((short)5);
        entity.Property(e => e.BackgroundColor).HasMaxLength(20);
        entity.Property(e => e.TextColor).HasMaxLength(20);
        entity.Property(e => e.ShowToFieldTech).HasDefaultValue(true);
        entity.Property(e => e.ShowOnCustomerPortal).HasDefaultValue(true);
        entity.Property(e => e.DisplayOrder).HasDefaultValue((short)1);
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        entity.HasOne(e => e.JobTypeCategory)
            .WithMany()
            .HasForeignKey(e => e.JobTypeCategoryId)
            .HasConstraintName("FK_FgsJobType_FgsJobTypeCategory")
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasOne(e => e.JobTypeSubCategory)
            .WithMany()
            .HasForeignKey(e => e.JobTypeSubCategoryId)
            .HasConstraintName("FK_FgsJobType_FgsJobTypeSubCategory")
            .OnDelete(DeleteBehavior.Restrict);
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.JobTypeCode })
            .IsUnique()
            .HasDatabaseName("UX_FgsJobType_Tenant_Company_JobTypeCode");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsJobType_Tenant_Company");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.UsedFor })
            .HasDatabaseName("IX_FgsJobType_Tenant_Company_UsedFor");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.BusinessUnit })
            .HasDatabaseName("IX_FgsJobType_Tenant_Company_BusinessUnit");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Trade })
            .HasDatabaseName("IX_FgsJobType_Tenant_Company_Trade");
    }
}
