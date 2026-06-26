using Fgs.Kernel.Entities;
using Fgs.Persistence.Extensions;
using Fgs.ServiceAgreement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.ServiceAgreement.Infrastructure.Database.Configurations;

internal static class FgsServiceAgreementDbContextConfigurationExtensions
{
    internal static void ConfigureTenantCompanyColumns<T>(this EntityTypeBuilder<T> entity)
        where T : class, ITenantCompanyScoped =>
        EntityFrameworkExtensions.ConfigureTenantCompanyColumns(entity);

    internal static void ConfigureAuditColumns(this EntityTypeBuilder entity) =>
        entity.ConfigureTimestamptzAuditColumns();

    internal static void ConfigureTenantCompanyCacheFkNonGeneric(
        this EntityTypeBuilder entity,
        string constraintName) =>
        entity.ConfigureTenantCompanyCacheFkNonGeneric(typeof(FgsTenantCompanyCache), constraintName);

    internal static void ApplyTenantCompanyCacheForeignKeys(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyTenantCompanyCacheForeignKeys(
            typeof(FgsTenantCompanyCache),
            new HashSet<Type> { typeof(FgsTenantCompanyCache) });
}
