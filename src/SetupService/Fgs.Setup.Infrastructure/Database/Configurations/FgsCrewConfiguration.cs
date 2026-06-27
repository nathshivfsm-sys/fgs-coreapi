using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal sealed class FgsCrewConfiguration : IEntityTypeConfiguration<FgsCrew>
{
    public void Configure(EntityTypeBuilder<FgsCrew> entity)
    {
        entity.ToTable(
            "FgsCrew",
            t => t.HasComment("Stores technician crew definitions used for dispatching and scheduling."));

        entity.HasKey(e => e.Id).HasName("PK_FgsCrew");
        entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);

        entity.Property(e => e.CrewCode).HasMaxLength(25).IsRequired()
            .HasComment("Unique crew code displayed on dispatch boards.");
        entity.Property(e => e.CrewName).HasMaxLength(100).IsRequired()
            .HasComment("User-friendly crew name.");
        entity.Property(e => e.Description).HasColumnType("text")
            .HasComment("Optional crew description.");
        entity.Property(e => e.IsActive).HasDefaultValue(true)
            .HasComment("Indicates whether the crew is active and available for dispatching.");
        entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("timestamptz").HasDefaultValueSql("now()");
        entity.Property(e => e.CreatedBy).HasMaxLength(100);
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.UpdatedBy).HasMaxLength(100);

        entity.HasIndex(e => new { e.TenantId, e.CompanyId })
            .HasDatabaseName("IX_FgsCrew_TenantId_CompanyId");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CrewCode })
            .IsUnique()
            .HasDatabaseName("UX_FgsCrew_TenantId_CompanyId_CrewCode");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CrewName })
            .IsUnique()
            .HasDatabaseName("UX_FgsCrew_TenantId_CompanyId_CrewName");
    }
}
