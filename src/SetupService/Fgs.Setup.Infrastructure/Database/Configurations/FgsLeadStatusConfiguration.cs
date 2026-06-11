using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class FgsLeadStatusConfiguration : IEntityTypeConfiguration<FgsLeadStatus>
{
    public void Configure(EntityTypeBuilder<FgsLeadStatus> entity)
    {
        entity.ToTable(
            "FgsLeadStatus",
            t => t.HasComment(
                "Stores tenant/company specific lead statuses used in the CRM lead lifecycle. Seeded from glo.GloLeadStatus during onboarding and may be customized by users."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityAlwaysColumn();

        entity.Property(e => e.TenantId).HasComment("Identifier of the tenant that owns the lead status.");
        entity.Property(e => e.CompanyId).HasComment("Identifier of the company that owns the lead status.");
        entity.Property(e => e.StatusCode)
            .HasMaxLength(50)
            .IsRequired()
            .HasComment("Unique business code for the lead status within a company. Examples: NEW, CONTACTED, QUALIFIED, CONVERTED.");
        entity.Property(e => e.StatusName)
            .HasMaxLength(100)
            .IsRequired()
            .HasComment("User-friendly name displayed throughout the application.");
        entity.Property(e => e.Description)
            .HasMaxLength(255)
            .HasComment("Optional description explaining the purpose of the lead status.");
        entity.Property(e => e.DisplayOrder)
            .HasDefaultValue((short)1)
            .HasComment("Determines the order in which statuses appear in dropdowns, lists, and reports.");
        entity.Property(e => e.IsSystem)
            .HasDefaultValue(false)
            .HasComment("Indicates whether the record was seeded by the system or created by a user.");
        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the status is available for selection and use.");

        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .HasComment("Date and time when the record was created.");
        entity.Property(e => e.CreatedBy).HasComment("User who created the record.");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz").HasComment("Date and time when the record was last updated.");
        entity.Property(e => e.UpdatedBy).HasComment("User who last updated the record.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.StatusCode })
            .IsUnique()
            .HasDatabaseName("UX_FgsLeadStatus_TenantId_CompanyId_StatusCode");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.StatusName })
            .IsUnique()
            .HasDatabaseName("UX_FgsLeadStatus_TenantId_CompanyId_StatusName");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.DisplayOrder })
            .HasDatabaseName("IX_FgsLeadStatus_TenantId_CompanyId_DisplayOrder");
    }
}
