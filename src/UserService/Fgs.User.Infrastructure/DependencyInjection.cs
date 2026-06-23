using Fgs.Messaging.Abstractions;
using Fgs.Messaging.Options;
using Fgs.User.Infrastructure.Extensions;
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
using Fgs.User.Infrastructure.Database.Read;
using Fgs.User.Infrastructure.Persistence.Queries;
using Fgs.User.Infrastructure.Persistence.Read;
using Fgs.User.Infrastructure.Persistence.Write;
using Fgs.Persistence.Extensions;
using Microsoft.Extensions.Http.Resilience;
using Refit;
using Fgs.Credentials;
using Fgs.Credentials.Abstractions;
using Fgs.Credentials.Extensions;
using Fgs.User.Infrastructure.Messaging;
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
        services.AddFgsCredentialConsumer(
            configuration,
            configuration,
            options =>
            {
                options.ServiceName = "fgs-user-service";
                options.RequiredProviders = ["DATABASE", "ENTRA_EXTERNAL_ID", "AWS"];
            },
            typeof(EntraExternalIdOptions),
            typeof(AwsCredentialsOptions));

        services.AddFgsUserFacingSecurity(configuration);
        services.Configure<AwsCredentialsOptions>(configuration.GetSection(AwsCredentialsOptions.SectionName));
        services.AddScoped<IFgsUserRoleResolver, FgsUserRoleResolver>();
        services.AddScoped<IFgsUserProfileResolver, FgsUserProfileResolver>();
        services.Configure<EntraExternalIdOptions>(configuration.GetSection(EntraExternalIdOptions.SectionName));
        services.Configure<OutboxOptions>(configuration.GetSection(OutboxOptions.SectionName));
        services.Configure<SignupLocaleOptions>(configuration.GetSection(SignupLocaleOptions.SectionName));

        services.AddDbContext<FgsUserDbContext>((sp, options) =>
        {
            var appConfiguration = sp.GetRequiredService<IConfiguration>();
            var credentialProvider = sp.GetService<ICredentialConfigurationProvider>();
            var connectionString = FgsUserConnectionString.ResolveRequired(appConfiguration, credentialProvider);
            options.UseFgsNpgsql(
                connectionString,
                "__EFMigrationsHistory",
                FgsUserDbContext.MigrationHistorySchema);
            options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
        });

        services.AddFgsPersistence<FgsUserDbContext>();
        services.AddSingleton<IUserReadConnectionFactory, FgsUserReadConnectionFactory>();
        services.AddScoped(typeof(IUserReadRepository<>), typeof(UserDapperReadRepository<>));
        services.AddScoped(typeof(IUserWriteRepository<>), typeof(UserEfWriteRepository<>));
        services.AddScoped<ITenantCompanyDetailsReadQuery, TenantCompanyDetailsReadQuery>();
        services.AddScoped<IUserRoleCodesReadQuery, UserRoleCodesReadQuery>();
        services.AddScoped<IInvitationReadQuery, InvitationReadQuery>();
        services.AddScoped<IOutboxWriter, OutboxWriter>();
        services.AddScoped<IAddressLocaleResolver, AddressLocaleResolver>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IEmailNormalizer, EmailNormalizer>();
        services.AddSingleton<IInvitationTokenService, InvitationTokenService>();
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
