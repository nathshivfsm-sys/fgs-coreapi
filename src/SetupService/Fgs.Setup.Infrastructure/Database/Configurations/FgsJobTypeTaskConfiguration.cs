using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsJobTypeTaskConfiguration : IEntityTypeConfiguration<FgsJobTypeTask>
{
    public void Configure(EntityTypeBuilder<FgsJobTypeTask> entity)
    {
        entity.ToTable("FgsJobTypeTask", t =>
            t.HasComment(
                "Stores the tasks that belong to a Job Type Category. Each task defines the work to be performed, along with its associated Trade, Priority, and estimated labor hours."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .UseIdentityByDefaultColumn()
            .HasComment("Unique identifier for the Job Type Task.");

        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);

        entity.Property(e => e.TenantId)
            .HasComment("Identifier of the tenant that owns this Job Type Task.");

        entity.Property(e => e.CompanyId)
            .HasComment("Identifier of the company within the tenant that owns this Job Type Task.");

        entity.Property(e => e.JobTypeCategoryId)
            .HasComment("Identifier of the Job Type Category that owns this task.");

        entity.Property(e => e.TradeId)
            .HasComment("Identifier of the Trade responsible for performing this task.");

        entity.Property(e => e.TaskName)
            .HasMaxLength(200)
            .HasComment("Name of the task to be performed.");

        entity.Property(e => e.Priority)
            .HasDefaultValue((short)5)
            .HasComment("Execution priority for the task. Lower values typically represent higher priority.");

        entity.Property(e => e.EstimatedHours)
            .HasPrecision(5, 2)
            .HasDefaultValue(1.00m)
            .HasComment("Estimated labor hours required to complete the task.");

        entity.Property(e => e.DisplayOrder)
            .HasDefaultValue((short)1)
            .HasComment("Controls the display sequence of tasks within the Job Type Category.");

        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the Job Type Task is active and available for use.");

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .HasComment("Date and time when the Job Type Task was created.");

        entity.Property(e => e.CreatedBy)
            .HasMaxLength(100)
            .HasComment("User who created the Job Type Task.");

        entity.Property(e => e.UpdatedOn)
            .HasColumnType("timestamptz")
            .HasComment("Date and time when the Job Type Task was last modified.");

        entity.Property(e => e.UpdatedBy)
            .HasMaxLength(100)
            .HasComment("User who last modified the Job Type Task.");

        entity.HasOne(e => e.JobTypeCategory)
            .WithMany(e => e.Tasks)
            .HasForeignKey(e => e.JobTypeCategoryId)
            .HasConstraintName("FK_FgsJobTypeTask_FgsJobTypeCategory")
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(e => e.Trade)
            .WithMany()
            .HasForeignKey(e => e.TradeId)
            .HasConstraintName("FK_FgsJobTypeTask_FgsSetupTechTrade")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsJobTypeTask_Tenant_Company");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.JobTypeCategoryId })
            .HasDatabaseName("IX_FgsJobTypeTask_Tenant_Company_JobTypeCategory");
    }
}
