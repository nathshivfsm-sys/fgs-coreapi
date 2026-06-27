using Fgs.Asset.Domain.Entities;
using Fgs.Kernel.Entities;
using Fgs.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Asset.Infrastructure.Database.Configurations;

internal static class FgsAssetDbContextConfigurationExtensions
{
    internal static void ConfigureTenantCompanyColumns<T>(this EntityTypeBuilder<T> entity)
        where T : class, ITenantCompanyScoped =>
        EntityFrameworkExtensions.ConfigureTenantCompanyColumns(entity);

    internal static void ConfigureAuditColumns(this EntityTypeBuilder entity) =>
        entity.ConfigureTimestamptzAuditColumns();

    internal static void ConfigureTenantCompanyCacheFk<T>(
        this EntityTypeBuilder<T> entity,
        string constraintName)
        where T : class, ITenantCompanyScoped =>
        entity.ConfigureTenantCompanyCacheFk(typeof(FgsTenantCompanyCache), constraintName);

    internal static void ConfigureTenantCompanyCacheFkNonGeneric(
        this EntityTypeBuilder entity,
        string constraintName) =>
        entity.ConfigureTenantCompanyCacheFkNonGeneric(typeof(FgsTenantCompanyCache), constraintName);

    internal static void ConfigureServiceLocationCacheFk<T>(
        this EntityTypeBuilder<T> entity,
        string constraintName)
        where T : class, ITenantCompanyScoped
    {
        entity.HasOne(typeof(FgsServiceLocationCache))
            .WithMany()
            .HasForeignKey(
                nameof(ITenantCompanyScoped.TenantId),
                nameof(ITenantCompanyScoped.CompanyId),
                nameof(FgsAsset.ServiceLocationId))
            .HasPrincipalKey(
                nameof(FgsServiceLocationCache.TenantId),
                nameof(FgsServiceLocationCache.CompanyId),
                nameof(FgsServiceLocationCache.ServiceLocationId))
            .HasConstraintName(constraintName)
            .OnDelete(DeleteBehavior.Restrict);
    }

    internal static void ApplyTenantCompanyCacheForeignKeys(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyTenantCompanyCacheForeignKeys(
            typeof(FgsTenantCompanyCache),
            new HashSet<Type>
            {
                typeof(FgsTenantCompanyCache),
                typeof(FgsServiceLocationCache)
            });
}
