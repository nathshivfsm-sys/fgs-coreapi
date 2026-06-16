using Fgs.Kernel.Entities;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Database.Schemas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal static class FgsSetupDbContextConfigurationExtensions
{
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
        where T : class, ITenantCompanyScoped
    {
        ((EntityTypeBuilder)entity).ConfigureTenantCompanySetupFk(constraintName);
    }

    internal static void ConfigureTenantCompanySetupFk(
        this EntityTypeBuilder entity,
        string constraintName)
    {
        entity.HasOne(typeof(FgsTenantCompanyCache))
            .WithMany()
            .HasForeignKey(nameof(ITenantCompanyScoped.TenantId), nameof(ITenantCompanyScoped.CompanyId))
            .HasConstraintName(constraintName)
            .OnDelete(DeleteBehavior.Restrict);
    }

    internal static void ConfigureTenantCompanyGuidSetupFk<T>(
        this EntityTypeBuilder<T> entity,
        string constraintName)
        where T : class, ITenantCompanyScoped
    {
        entity.ConfigureTenantCompanySetupFk(constraintName);
    }

    internal static void ApplyTenantCompanyCacheForeignKeys(ModelBuilder modelBuilder)
    {
        var excludedTypes = new HashSet<Type>
        {
            typeof(FgsLocation),
            typeof(FgsTenantCompanyCache),
            typeof(GloCredentialProviderTypeCache),
            typeof(GloResolutionTypeCache),
        };

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;
            if (clrType is null || excludedTypes.Contains(clrType))
            {
                continue;
            }

            if (!typeof(ITenantCompanyScoped).IsAssignableFrom(clrType))
            {
                continue;
            }

            if (!string.Equals(entityType.GetSchema(), FgsDatabaseSchemas.Setup, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var tableName = entityType.GetTableName();
            if (string.IsNullOrEmpty(tableName))
            {
                continue;
            }

            modelBuilder.Entity(clrType)
                .ConfigureTenantCompanySetupFk($"FK_{tableName}_FgsTenantCompanyCache_TenantId_CompanyId");
        }
    }
}
