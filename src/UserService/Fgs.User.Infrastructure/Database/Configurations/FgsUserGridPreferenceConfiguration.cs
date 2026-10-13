using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal class FgsUserGridPreferenceConfiguration : IEntityTypeConfiguration<FgsUserGridPreference>
{
    public void Configure(EntityTypeBuilder<FgsUserGridPreference> entity)
    {
        entity.ToTable(
            "FgsUserGridPreference",
            t => t.HasComment(
                "Stores user-specific grid layout and display preferences for each grid within a tenant and company."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
            .UseIdentityByDefaultColumn()
            .HasComment("Unique identifier for the user grid preference record.");

        entity.Property(e => e.TenantId)
            .HasComment("Identifier of the tenant to which the user and grid preference belong.");
        entity.Property(e => e.CompanyId)
            .HasComment("Identifier of the company within the tenant to which the user and grid preference belong.");
        entity.Property(e => e.UserId)
            .HasComment("Identifier of the user whose grid preference is being stored.");
        entity.Property(e => e.GridCode)
            .HasMaxLength(100)
            .IsRequired()
            .HasComment(
                "Unique application-defined code identifying the grid for which the user preferences are stored, such as WorkOrderList or EstimateList.");
        entity.Property(e => e.GridConfiguration)
            .HasColumnType("jsonb")
            .IsRequired()
            .HasDefaultValueSql("'{}'::jsonb")
            .HasComment(
                "JSON configuration containing user-specific grid preferences such as column width, visibility, display order, pinning, sorting, filtering, grouping, and other supported grid settings.");

        entity.Property(e => e.CreatedOn)
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .HasComment("UTC timestamp when the user grid preference record was created.");
        entity.Property(e => e.CreatedBy)
            .HasMaxLength(100)
            .HasComment("Identifier of the user or process that created the user grid preference record.");
        entity.Property(e => e.UpdatedOn)
            .HasColumnType("timestamptz")
            .HasComment("UTC timestamp when the user grid preference record was last updated.");
        entity.Property(e => e.UpdatedBy)
            .HasMaxLength(100)
            .HasComment("Identifier of the user or process that last updated the user grid preference record.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.UserId, e.GridCode })
            .IsUnique()
            .HasDatabaseName("UQ_FgsUserGridPreference_TenantId_CompanyId_UserId_GridCode");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.GridCode })
            .HasDatabaseName("IX_FgsUserGridPreference_TenantId_CompanyId_GridCode");

        entity.HasOne(e => e.User)
            .WithMany(u => u.GridPreferences)
            .HasForeignKey(e => new { e.UserId, e.TenantId, e.CompanyId })
            .HasPrincipalKey(u => new { u.Id, u.TenantId, u.CompanyId })
            .HasConstraintName("FK_FgsUserGridPreference_FgsUser")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
