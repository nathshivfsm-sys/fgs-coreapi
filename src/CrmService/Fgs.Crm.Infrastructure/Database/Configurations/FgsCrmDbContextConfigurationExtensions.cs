using Fgs.Crm.Domain.Entities;
using Fgs.Kernel.Entities;
using Fgs.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Crm.Infrastructure.Database.Configurations;

internal static class FgsCrmDbContextConfigurationExtensions
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

    internal static void ApplyTenantCompanyCacheForeignKeys(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyTenantCompanyCacheForeignKeys(
            typeof(FgsTenantCompanyCache),
            new HashSet<Type> { typeof(FgsTenantCompanyCache) });

    internal static void ConfigureAuditActorColumns(ModelBuilder modelBuilder, int maxLength = 100) =>
        modelBuilder.ConfigureAuditActorColumns(maxLength);
}
