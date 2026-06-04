using Fgs.Kernel.Entities;
using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Fgs.User.Infrastructure.Database.Configurations;

internal static class FgsUserDbContextConfigurationExtensions
{
    private static readonly ValueConverter<string?, long?> BigintActorIdConverter = new(
        v => string.IsNullOrWhiteSpace(v) ? null : long.Parse(v),
        v => v.HasValue ? v.Value.ToString() : null);

    internal static void ConfigureGloEntityBigintAuditColumns<T>(this EntityTypeBuilder<T> entity)
        where T : GloEntityBase
    {
        entity.Property(e => e.CreatedOn)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.CreatedBy)
            .HasColumnType("bigint")
            .HasConversion(BigintActorIdConverter);
        entity.Property(e => e.UpdatedBy)
            .HasColumnType("bigint")
            .HasConversion(BigintActorIdConverter);
    }

    internal static void ConfigureTenantCompanySetupColumns<T>(
        this EntityTypeBuilder<T> entity,
        bool includeTenantCompanyIndex = true,
        string? tenantCompanyIndexName = null)
        where T : FgsTenantCompanySetupEntityBase<long>
    {
        entity.Property(e => e.Id).HasColumnOrder(0);
        entity.Property(e => e.TenantId).HasColumnOrder(1);
        entity.Property(e => e.CompanyId).HasColumnOrder(2);
        entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
        entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");

        if (includeTenantCompanyIndex)
        {
            var index = entity.HasIndex(e => new { e.TenantId, e.CompanyId });
            if (tenantCompanyIndexName is not null)
            {
                index.HasDatabaseName(tenantCompanyIndexName);
            }
        }
    }

    internal static void ConfigureTenantCompanyGuidSetupColumns<T>(this EntityTypeBuilder<T> entity)
        where T : FgsTenantCompanySetupEntityBase<Guid>
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
        where T : FgsTenantCompanySetupEntityBase<long>
    {    }

    internal static void ConfigureTenantCompanyGuidSetupFk<T>(
        this EntityTypeBuilder<T> entity,
        string constraintName)
        where T : FgsTenantCompanySetupEntityBase<Guid>
    {    }
}
