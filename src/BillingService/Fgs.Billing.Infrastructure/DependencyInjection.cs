using Fgs.Credentials;
using Fgs.Credentials.Abstractions;
using Fgs.Credentials.Extensions;
using Fgs.Billing.Application.Abstractions.Invoices;
using Fgs.Billing.Application.Abstractions.Time;
using Fgs.Billing.Infrastructure.Common;
using Fgs.Billing.Infrastructure.Common.Time;
using Fgs.Billing.Infrastructure.Database;
using Fgs.Billing.Infrastructure.Persistence.Invoices;
using Fgs.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Billing.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsBillingInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddFgsStandardInfrastructure(configuration, "fgs-billing-service", "DATABASE");

        services.AddDbContext<FgsBillingDbContext>((sp, options) =>
        {
            var connectionString = ConnectionStringResolver.ResolveRequired(
                sp.GetRequiredService<IConfiguration>(),
                ConnectionStringNames.FgsBilling,
                "FGS_BILLING_DB",
                sp.GetService<ICredentialConfigurationProvider>());
            options.UseFgsNpgsql(
                connectionString,
                "__EFMigrationsHistory",
                FgsBillingDbContext.MigrationHistorySchema);
        });

        services.AddFgsPersistence<FgsBillingDbContext>();
        services.AddFgsDbContextReadyCheck<FgsBillingDbContext>();

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<BillingEntityAuditHelper>();
        services.AddScoped<IFgsInvoiceReadRepository, FgsInvoiceReadRepository>();
        services.AddScoped<IFgsInvoiceWriteService, FgsInvoiceWriteService>();

        return services;
    }
}
