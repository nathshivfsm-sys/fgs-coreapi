using Fgs.Credentials;
using Fgs.Credentials.Abstractions;
using Fgs.Credentials.Extensions;
using Fgs.Persistence.Extensions;
using Fgs.ServiceAgreement.Application.Abstractions.ServiceAgreements;
using Fgs.ServiceAgreement.Infrastructure.Common;
using Fgs.ServiceAgreement.Infrastructure.Database;
using Fgs.ServiceAgreement.Infrastructure.Persistence.ServiceAgreements;
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
        services.AddFgsDbContextReadyCheck<FgsServiceAgreementDbContext>();

        services.AddScoped<ServiceAgreementEntityAuditHelper>();
        services.AddScoped<IFgsServiceAgreementReadRepository, FgsServiceAgreementReadRepository>();
        services.AddScoped<IFgsServiceAgreementWriteService, FgsServiceAgreementWriteService>();

        return services;
    }
}
