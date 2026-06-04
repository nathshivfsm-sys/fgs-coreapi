using Fgs.Messaging.Abstractions;
using Fgs.Messaging.Extensions;
using Fgs.Messaging.Options;
using Fgs.Persistence.Abstractions;
using Fgs.Security.Abstractions;
using Fgs.Security.Extensions;
using Fgs.User.Application.Abstractions.Geo;
using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Application.Abstractions.Security;
using Fgs.User.Application.Abstractions.Time;
using Fgs.User.Infrastructure.Common.Geo;
using Fgs.Contracts.Clients;
using Fgs.Foundation.Extensions;
using Fgs.User.Infrastructure.Common.Identity;
using Fgs.User.Infrastructure.Common.Options;
using Fgs.User.Infrastructure.Common.Security;
using Fgs.User.Infrastructure.Common.Time;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Infrastructure.Database;
using Fgs.User.Infrastructure.Database.UnitOfWorks;
using Fgs.Persistence.Extensions;
using Microsoft.Extensions.Http.Resilience;
using Refit;
using Fgs.User.Infrastructure.Outbox;
using Fgs.Setup.Infrastructure.Database;
using Fgs.Setup.Infrastructure.Messaging;
using SetupOutboxWriter = Fgs.Setup.Infrastructure.Messaging.OutboxWriter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.User.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsUserInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddFgsEntraAuthentication(configuration);
        services.AddScoped<IFgsClaimsEnricher, DbFgsClaimsEnricher>();
        services.AddScoped<IFgsUserRoleResolver, FgsUserRoleResolver>();
        services.AddScoped<IFgsUserProfileResolver, FgsUserProfileResolver>();
        services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.SectionName));
        services.PostConfigure<RabbitMqOptions>(options =>
        {
            options.ClientProvidedName = "Fgs.User";
            options.AutomaticRecoveryEnabled = false;
        });
        services.Configure<EntraExternalIdOptions>(configuration.GetSection(EntraExternalIdOptions.SectionName));
        services.Configure<OutboxOptions>(configuration.GetSection(OutboxOptions.SectionName));
        services.Configure<SignupLocaleOptions>(configuration.GetSection(SignupLocaleOptions.SectionName));

        var connectionString = FgsUserConnectionString.ResolveRequired(configuration);
        var setupConnectionString = configuration.GetConnectionString("FgsSetup") ?? connectionString;
        services.AddDbContext<FgsSetupDbContext>((_, options) =>
        {
            options.UseNpgsql(setupConnectionString, npgsql =>
            {
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", FgsSetupDbContext.MigrationHistorySchema);
            });
        });
        services.AddDbContext<FgsUserDbContext>((_, options) =>
        {
            options.UseFgsNpgsql(
                connectionString,
                "__EFMigrationsHistory",
                FgsUserDbContext.MigrationHistorySchema);
            options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
        });

        services.AddFgsPersistence<FgsUserDbContext>();
        services.AddScoped<SetupUnitOfWork>();
        services.AddScoped<ISetupUnitOfWork>(sp => sp.GetRequiredService<SetupUnitOfWork>());
        services.AddScoped<IOutboxWriter, SetupOutboxWriter>();
        services.AddScoped<IOutboxStore, Fgs.Setup.Infrastructure.Outbox.GloOutboxStore>();
        services.AddSingleton<IOutboxRoutingResolver, UserOutboxRoutingResolver>();
        services.AddFgsRabbitMqPublisher();
        services.AddFgsOutboxProcessor();
        services.AddScoped<IAddressLocaleResolver, AddressLocaleResolver>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IEmailNormalizer, EmailNormalizer>();
        services.AddSingleton<IInvitationTokenService, InvitationTokenService>();
        services.AddSingleton<RabbitMqTopologyService>();
        services.AddScoped<IEntraExternalIdService, EntraExternalIdRefitService>();
        services.AddRefitClient<IEntraOAuthClient>()
            .ConfigureHttpClient((sp, client) =>
            {
                var entra = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<EntraExternalIdOptions>>().Value;
                var tokenEndpoint = string.IsNullOrWhiteSpace(entra.TokenEndpoint)
                    ? $"{entra.Authority.TrimEnd('/')}/{entra.TenantId.Trim('/')}/oauth2/v2.0/token"
                    : entra.TokenEndpoint;
                client.BaseAddress = new Uri(tokenEndpoint);
            })
            .AddStandardResilienceHandler();

        return services;
    }
}
