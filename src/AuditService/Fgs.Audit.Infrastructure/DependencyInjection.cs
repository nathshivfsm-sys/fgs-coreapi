using Fgs.Audit.Application.Abstractions;
using Fgs.Audit.Infrastructure.Audit;
using Fgs.Audit.Infrastructure.Database;
using Fgs.Credentials;
using Fgs.Credentials.Abstractions;
using Fgs.Credentials.Extensions;
using Fgs.Persistence.Extensions;
using Fgs.Security.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Audit.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsAuditInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddFgsCredentialConsumer(
            configuration,
            configuration,
            options =>
            {
                options.ServiceName = "fgs-audit-service";
                options.RequiredProviders = ["DATABASE"];
            });

        services.AddFgsApiSecurity(configuration);

        services.AddDbContext<FgsAuditDbContext>((sp, options) =>
        {
            var connectionString = ConnectionStringResolver.ResolveRequired(
                sp.GetRequiredService<IConfiguration>(),
                ConnectionStringNames.FgsAudit,
                "FGS_AUDIT_DB",
                sp.GetService<ICredentialConfigurationProvider>());
            options.UseFgsNpgsql(
                connectionString,
                "__EFMigrationsHistory",
                FgsAuditDbContext.MigrationHistorySchema);
        });

        services.AddFgsPersistence<FgsAuditDbContext>();
        services.AddScoped<ICredentialAuditWriter, CredentialAuditWriter>();

        return services;
    }
}
