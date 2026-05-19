using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal static class FgsUserDbContextConfigurationExtensions
{
    internal static void ConfigureTenantCompanySetupColumns<T>(this EntityTypeBuilder<T> entity)
        where T : FgsTenantCompanySetupEntityBase
    {
        entity.Property(e => e.Id).HasColumnOrder(0);
        entity.Property(e => e.TenantId).HasColumnOrder(1);
        entity.Property(e => e.CompanyId).HasColumnOrder(2);
        entity.HasIndex(e => new { e.TenantId, e.CompanyId });
        entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
    }

    internal static void ConfigureTenantCompanySetupFk<T>(
        this EntityTypeBuilder<T> entity,
        string constraintName)
        where T : FgsTenantCompanySetupEntityBase
    {
        entity.HasOne<FgsTenantCompany>()
            .WithMany()
            .HasForeignKey(e => new { e.TenantId, e.CompanyId })
            .HasPrincipalKey(tc => new { tc.TenantId, tc.CompanyNumber })
            .HasConstraintName(constraintName)
            .OnDelete(DeleteBehavior.Restrict);
    }

    internal static void ConfigureTenantCompanyGuidSetupColumns<T>(this EntityTypeBuilder<T> entity)
        where T : FgsTenantCompanySetupGuidEntityBase
    {
        entity.Property(e => e.Id).HasColumnOrder(0);
        entity.Property(e => e.TenantId).HasColumnOrder(1);
        entity.Property(e => e.CompanyId).HasColumnOrder(2);
        entity.HasIndex(e => new { e.TenantId, e.CompanyId });
        entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
    }

    internal static void ConfigureTenantCompanyGuidSetupFk<T>(
        this EntityTypeBuilder<T> entity,
        string constraintName)
        where T : FgsTenantCompanySetupGuidEntityBase
    {
        entity.HasOne<FgsTenantCompany>()
            .WithMany()
            .HasForeignKey(e => new { e.TenantId, e.CompanyId })
            .HasPrincipalKey(tc => new { tc.TenantId, tc.CompanyNumber })
            .HasConstraintName(constraintName)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
