using Fgs.Billing.Domain.Entities;
using Fgs.Kernel.Entities;
using Fgs.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Billing.Infrastructure.Database.Configurations;

internal static class FgsBillingDbContextConfigurationExtensions
{
    internal static void ConfigureTenantCompanyColumns<T>(this EntityTypeBuilder<T> entity)
        where T : class, ITenantCompanyScoped =>
        EntityFrameworkExtensions.ConfigureTenantCompanyColumns(entity);

    internal static void ApplyTenantCompanyCacheForeignKeys(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyTenantCompanyCacheForeignKeys(
            typeof(FgsTenantCompanyCache),
            new HashSet<Type> { typeof(FgsTenantCompanyCache) },
            tableName => tableName switch
            {
                "FgsInvoice" => "FK_FgsInvoice_TenantCompany",
                "FgsInvoiceBatch" => "FK_FgsInvoiceBatch_TenantCompany",
                "FgsPayment" => "FK_FgsPayment_TenantCompany",
                "FgsInvoicePaymentApplication" => "FK_FgsInvoicePaymentApplication_TenantCompany",
                "FgsPaymentTransaction" => "FK_FgsPaymentTransaction_TenantCompany",
                "FgsInvoiceWorkDescription" => "FK_FgsInvoiceWorkDescription_TenantCompany",
                _ => $"FK_{tableName}_FgsTenantCompanyCache_TenantId_CompanyId"
            });
}
