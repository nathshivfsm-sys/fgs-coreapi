using Fgs.Integration.Domain.Entities;
using Fgs.Kernel.Entities;
using Fgs.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Integration.Infrastructure.Database.Configurations;

internal static class FgsIntegrationDbContextConfigurationExtensions
{
    internal static void ConfigureTenantCompanyColumns<T>(this EntityTypeBuilder<T> entity)
        where T : class, ITenantCompanyScoped =>
        EntityFrameworkExtensions.ConfigureTenantCompanyColumns(entity);

    internal static void ApplyTenantCompanyCacheForeignKeys(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyTenantCompanyCacheForeignKeys(
            typeof(FgsTenantCompanyCache),
            new HashSet<Type>
            {
                typeof(FgsTenantCompanyCache),
                typeof(FgsPaymentTransactionPayload)
            },
            tableName => tableName switch
            {
                "FgsPaymentTransactionPayload" => "FK_FgsPaymentTransactionPayload_TenantCompany",
                _ => $"FK_{tableName}_FgsTenantCompanyCache_TenantId_CompanyId"
            });
}
