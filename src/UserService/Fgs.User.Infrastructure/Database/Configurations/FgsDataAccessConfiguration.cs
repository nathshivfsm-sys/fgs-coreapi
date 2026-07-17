using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal class FgsDataAccessConfiguration : IEntityTypeConfiguration<FgsDataAccess>
{
    public void Configure(EntityTypeBuilder<FgsDataAccess> entity)
    {
        entity.ToTable(
            "FgsDataAccess",
            t => t.HasComment(
                "Stores reusable data access profiles that define the scope of data a role can access."));

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).UseIdentityByDefaultColumn();
        entity.ConfigureTenantCompanySetupColumns(includeTenantCompanyIndex: false);

        entity.Property(e => e.DataAccessCode)
            .HasMaxLength(50)
            .HasComment("Unique system identifier for the data access profile.");
        entity.Property(e => e.Name)
            .HasMaxLength(100)
            .HasComment("Display name of the data access profile.");
        entity.Property(e => e.Description)
            .HasMaxLength(255)
            .HasComment("Optional description explaining the purpose of the data access profile.");
        entity.Property(e => e.IsBuiltIn)
            .HasDefaultValue(false)
            .HasComment("Indicates whether the data access profile was provided by the platform. Built-in profiles cannot be edited but may be cloned.");
        entity.Property(e => e.DisplayOrder)
            .HasDefaultValue((short)1)
            .HasComment("Controls the display order within the user interface.");
        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasComment("Indicates whether the data access profile is available for assignment.");

        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.DataAccessCode })
            .IsUnique()
            .HasDatabaseName("IX_FgsDataAccess_TenantId_CompanyId_DataAccessCode");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Name })
            .HasDatabaseName("IX_FgsDataAccess_TenantId_CompanyId_Name");
        entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.IsBuiltIn })
            .HasDatabaseName("IX_FgsDataAccess_TenantId_CompanyId_IsBuiltIn");
    }
}
