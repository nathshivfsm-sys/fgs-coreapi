using Fgs.Credentials;
using Fgs.Credentials.Abstractions;
using Fgs.Credentials.Extensions;
using Fgs.Persistence.Extensions;
using Fgs.ServiceAgreement.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.ServiceAgreement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsServiceAgreementInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddFgsStandardInfrastructure(configuration, "fgs-service-agreement-service", "DATABASE");

        services.AddDbContext<FgsServiceAgreementDbContext>((sp, options) =>
        {
            var connectionString = ConnectionStringResolver.ResolveRequired(
                sp.GetRequiredService<IConfiguration>(),
                ConnectionStringNames.FgsServiceAgreement,
                "FGS_SVC_DB",
                sp.GetService<ICredentialConfigurationProvider>());
            options.UseFgsNpgsql(
                connectionString,
                "__EFMigrationsHistory",
                FgsServiceAgreementDbContext.MigrationHistorySchema);
        });

        services.AddFgsPersistence<FgsServiceAgreementDbContext>();

        return services;
    }
}
