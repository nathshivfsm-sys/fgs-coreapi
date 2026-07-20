using Fgs.Audit.Application.Abstractions;
using Fgs.Audit.Domain.Enums;
using Fgs.Audit.Infrastructure.Audit;
using Fgs.Audit.Infrastructure.Database;
using Fgs.Audit.Infrastructure.Database.Schemas;
using Fgs.Credentials;
using Fgs.Credentials.Abstractions;
using Fgs.Credentials.Extensions;
using Fgs.Persistence.Extensions;
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
        services.AddFgsStandardInfrastructure(configuration, "fgs-audit-service", "DATABASE");

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
                FgsAuditDbContext.MigrationHistorySchema,
                npgsql =>
                {
                    npgsql.MapEnum<AuditRecordType>("record_type", FgsDatabaseSchemas.Audit, nameTranslator: new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
                    npgsql.MapEnum<AuditEventSource>("event_source", FgsDatabaseSchemas.Audit, nameTranslator: new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
                    npgsql.MapEnum<AuditEventDetailType>("event_detail_type", FgsDatabaseSchemas.Audit, nameTranslator: new Npgsql.NameTranslation.NpgsqlNullNameTranslator());
                });
        });

        services.AddFgsPersistence<FgsAuditDbContext>();
        services.AddScoped<ICredentialAuditWriter, CredentialAuditWriter>();

        return services;
    }
}
