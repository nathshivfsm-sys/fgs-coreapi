using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsLeadDisqualificationReasonConfiguration : IEntityTypeConfiguration<FgsLeadDisqualificationReason>
{
    public void Configure(EntityTypeBuilder<FgsLeadDisqualificationReason> entity)
    {
        entity.ToTable(
            "FgsLeadDisqualificationReason",
            t => t.HasComment(
                "Stores tenant/company specific lead disqualification reasons used when leads are marked as disqualified."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityAlwaysColumn();

        entity.Property(e => e.ReasonCode)
            .HasMaxLength(50)
            .IsRequired()
            .HasComment("Unique business code for the disqualification reason within a company.");
        entity.Property(e => e.ReasonName)
            .HasMaxLength(100)
            .IsRequired()
            .HasComment("User-friendly name displayed throughout the application.");
        entity.Property(e => e.Description)
            .HasMaxLength(255)
            .HasComment("Optional description explaining the reason.");
        entity.Property(e => e.DisplayOrder)
            .HasDefaultValue((short)1)
            .HasComment("Controls the order in which reasons are displayed in dropdowns and lists.");
        entity.Property(e => e.IsSystem)
            .HasDefaultValue(false)
            .HasComment("Indicates whether the reason was seeded by the system or created by a user.");
        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the reason is available for selection.");

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsLeadDisqualificationReason_TenantId_CompanyId");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ReasonCode })
            .IsUnique()
            .HasDatabaseName("UX_FgsLeadDisqualificationReason_TenantId_CompanyId_ReasonCode");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.ReasonName })
            .IsUnique()
            .HasDatabaseName("UX_FgsLeadDisqualificationReason_TenantId_CompanyId_ReasonName");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.DisplayOrder })
            .HasDatabaseName("IX_FgsLeadDisqualificationReason_TenantId_CompanyId_DisplayOrder");
    }
}
