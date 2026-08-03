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
using Fgs.User.Application.Abstractions.ApiClients;
using Fgs.User.Application.Abstractions.ApiEvents;
using Fgs.User.Application.Abstractions.ApiSecrets;
using Fgs.User.Application.Abstractions.ApiWebhooks;
using Fgs.User.Application.Abstractions.ApiWebhookSubscriptions;
using Fgs.User.Application.Abstractions.DataAccesses;
using Fgs.User.Application.Abstractions.DataAccessScopes;
using Fgs.User.Application.Abstractions.Permissions;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Abstractions.PublicEndpoints;
using Fgs.User.Application.Abstractions.RoleDataAccesses;
using Fgs.User.Application.Abstractions.RolePermissions;
using Fgs.User.Application.Abstractions.Roles;
using Fgs.User.Application.Abstractions.UserRoles;
using Fgs.User.Application.Abstractions.ServiceSetups;
using Fgs.User.Application.Abstractions.ServiceAccountsSetups;
using Fgs.User.Application.Abstractions.Users;
using Fgs.User.Infrastructure.Entities.ApiClients;
using Fgs.User.Infrastructure.Entities.ApiEvents;
using Fgs.User.Infrastructure.Entities.ApiSecrets;
using Fgs.User.Infrastructure.Entities.ApiWebhooks;
using Fgs.User.Infrastructure.Entities.ApiWebhookSubscriptions;
using Fgs.User.Infrastructure.Entities.DataAccesses;
using Fgs.User.Infrastructure.Entities.DataAccessScopes;
using Fgs.User.Infrastructure.Entities.Permissions;
using Fgs.User.Infrastructure.Entities.PublicEndpoints;
using Fgs.User.Infrastructure.Entities.RoleDataAccesses;
using Fgs.User.Infrastructure.Entities.RolePermissions;
using Fgs.User.Infrastructure.Entities.Roles;
using Fgs.User.Infrastructure.Entities.UserRoles;
using Fgs.User.Infrastructure.Entities.ServiceSetups;
using Fgs.User.Infrastructure.Entities.ServiceAccountsSetups;
using Fgs.User.Infrastructure.Entities.Users;
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
using Fgs.Foundation.Caching.Options;
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
                options.RequiredProviders = ["DATABASE", "ENTRA_EXTERNAL_ID", "AWS", "REDIS"];
            },
            typeof(EntraExternalIdOptions),
            typeof(AwsCredentialsOptions),
            typeof(RedisCacheOptions));

        services.AddFgsUserFacingSecurity(configuration);
        services.Configure<AwsCredentialsOptions>(configuration.GetSection(AwsCredentialsOptions.SectionName));
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
        services.AddScoped<IUserAuthorizationReadQuery, UserAuthorizationReadQuery>();
        services.AddScoped<IInvitationReadQuery, InvitationReadQuery>();
        services.AddScoped<ILoginPkceStore, LoginPkceStore>();
        services.AddScoped<ILoginAuthorizationProfileBuilder, LoginAuthorizationProfileBuilder>();
        services.AddScoped<IFgsRoleReadRepository, FgsRoleReadRepository>();
        services.AddScoped<IFgsRoleWriteService, FgsRoleWriteService>();
        services.AddScoped<IFgsPermissionReadRepository, FgsPermissionReadRepository>();
        services.AddScoped<IFgsPermissionWriteService, FgsPermissionWriteService>();
        services.AddScoped<IFgsDataAccessReadRepository, FgsDataAccessReadRepository>();
        services.AddScoped<IFgsDataAccessWriteService, FgsDataAccessWriteService>();
        services.AddScoped<IFgsDataAccessScopeReadRepository, FgsDataAccessScopeReadRepository>();
        services.AddScoped<IFgsDataAccessScopeWriteService, FgsDataAccessScopeWriteService>();
        services.AddScoped<IFgsUserRoleReadRepository, FgsUserRoleReadRepository>();
        services.AddScoped<IFgsUserRoleWriteService, FgsUserRoleWriteService>();
        services.AddScoped<IFgsUserReadRepository, FgsUserReadRepository>();
        services.AddScoped<IFgsUserWriteService, FgsUserWriteService>();
        services.AddScoped<IFgsTenantServiceSetupReadRepository, FgsTenantServiceSetupReadRepository>();
        services.AddScoped<IFgsTenantServiceSetupWriteService, FgsTenantServiceSetupWriteService>();
        services.AddScoped<IFgsTenantServiceAccountsSetupReadRepository, FgsTenantServiceAccountsSetupReadRepository>();
        services.AddScoped<IFgsTenantServiceAccountsSetupWriteService, FgsTenantServiceAccountsSetupWriteService>();
        services.AddScoped<IFgsRolePermissionReadRepository, FgsRolePermissionReadRepository>();
        services.AddScoped<IFgsRolePermissionWriteService, FgsRolePermissionWriteService>();
        services.AddScoped<IFgsRoleDataAccessReadRepository, FgsRoleDataAccessReadRepository>();
        services.AddScoped<IFgsRoleDataAccessWriteService, FgsRoleDataAccessWriteService>();
        services.AddScoped<IFgsPublicEndpointReadRepository, FgsPublicEndpointReadRepository>();
        services.AddScoped<IFgsPublicEndpointWriteService, FgsPublicEndpointWriteService>();
        services.AddScoped<IFgsApiEventReadRepository, FgsApiEventReadRepository>();
        services.AddScoped<IFgsApiEventWriteService, FgsApiEventWriteService>();
        services.AddScoped<IFgsApiClientReadRepository, FgsApiClientReadRepository>();
        services.AddScoped<IFgsApiClientWriteService, FgsApiClientWriteService>();
        services.AddScoped<IFgsApiSecretReadRepository, FgsApiSecretReadRepository>();
        services.AddScoped<IFgsApiSecretWriteService, FgsApiSecretWriteService>();
        services.AddScoped<IFgsApiWebhookReadRepository, FgsApiWebhookReadRepository>();
        services.AddScoped<IFgsApiWebhookWriteService, FgsApiWebhookWriteService>();
        services.AddScoped<IFgsApiWebhookSubscriptionReadRepository, FgsApiWebhookSubscriptionReadRepository>();
        services.AddScoped<IFgsApiWebhookSubscriptionWriteService, FgsApiWebhookSubscriptionWriteService>();
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
