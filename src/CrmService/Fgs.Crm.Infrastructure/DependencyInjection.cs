using Fgs.Credentials;
using Fgs.Credentials.Abstractions;
using Fgs.Credentials.Extensions;
using Fgs.Crm.Application.Abstractions.Customers;
using Fgs.Crm.Domain.Enums;
using Fgs.Crm.Infrastructure.Common;
using Fgs.Crm.Infrastructure.Database;
using Fgs.Crm.Infrastructure.Database.Schemas;
using Fgs.Crm.Infrastructure.Persistence.Customers;
using Fgs.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Crm.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsCrmInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddFgsStandardInfrastructure(configuration, "fgs-crm-service", "DATABASE");

        services.AddDbContext<FgsCrmDbContext>((sp, options) =>
        {
            var connectionString = ConnectionStringResolver.ResolveRequired(
                sp.GetRequiredService<IConfiguration>(),
                ConnectionStringNames.FgsCrm,
                "FGS_CRM_DB",
                sp.GetService<ICredentialConfigurationProvider>());
            options.UseFgsNpgsql(
                connectionString,
                "__EFMigrationsHistory",
                FgsCrmDbContext.MigrationHistorySchema,
                npgsql => npgsql.MapEnum<SalesPriority>(
                    "SalesPriority",
                    FgsDatabaseSchemas.Crm,
                    nameTranslator: new Npgsql.NameTranslation.NpgsqlNullNameTranslator()));
        });

        services.AddFgsPersistence<FgsCrmDbContext>();
        services.AddFgsDbContextReadyCheck<FgsCrmDbContext>();

        services.AddScoped<CrmEntityAuditHelper>();
        services.AddScoped<ICrmCustomerReadRepository, CrmCustomerReadRepository>();
        services.AddScoped<ICrmCustomerWriteService, CrmCustomerWriteService>();

        return services;
    }
}
