using Fgs.Security.Abstractions;
using Fgs.Security.Options;
using Fgs.Security.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fgs.Security.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFgsEntraAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<EntraExternalIdAuthOptions>(
            configuration.GetSection(EntraExternalIdAuthOptions.SectionName));

        var entraOptions = configuration
                               .GetSection(EntraExternalIdAuthOptions.SectionName)
                               .Get<EntraExternalIdAuthOptions>()
                           ?? new EntraExternalIdAuthOptions();

        if (string.IsNullOrWhiteSpace(entraOptions.ClientId))
        {
            throw new InvalidOperationException(
                $"Entra client id is not configured. Set {EntraExternalIdAuthOptions.SectionName}:ClientId.");
        }

        var authority = entraOptions.ResolveAuthority();
        var clientId = entraOptions.ClientId;
        var signingKeyResolver = new FgsEntraSigningKeyResolver(entraOptions);

        services.AddHttpContextAccessor();
        services.AddScoped<IFgsUserContext, HttpFgsUserContext>();

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = authority;
                options.MetadataAddress = entraOptions.ResolveMetadataAddress();
                options.MapInboundClaims = false;
                options.RefreshOnIssuerKeyNotFound = true;
                options.TokenValidationParameters = FgsEntraTokenValidation.CreateValidationParameters(entraOptions);
                options.TokenValidationParameters.IssuerSigningKeyResolver =
                    signingKeyResolver.Resolve;
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Token;
                        if (string.IsNullOrWhiteSpace(token))
                        {
                            token = FgsRequestAuthContext.ExtractBearerToken(context.HttpContext);
                        }

                        var normalized = FgsEntraGraphAccessTokenNormalizer.NormalizeIfRequired(
                            token ?? string.Empty);
                        if (!string.IsNullOrEmpty(normalized))
                        {
                            context.Token = normalized;
                        }

                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        var logger = context.HttpContext.RequestServices
                            .GetService<ILoggerFactory>()
                            ?.CreateLogger("JwtBearer");
                        logger?.LogWarning(
                            context.Exception,
                            "JWT authentication failed: {Message}",
                            context.Exception.Message);
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        if (!FgsEntraTokenValidation.ValidateGraphAudienceAppId(context.Principal, clientId))
                        {
                            context.Fail("Access token appid does not match configured Entra client id.");
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        services.AddFgsAuthorization();
        return services;
    }

    public static IServiceCollection AddFgsApiSecurity(
        this IServiceCollection services,
        IConfiguration configuration) =>
        services
            .AddFgsEntraAuthentication(configuration)
            .AddFgsActiveUserValidation(configuration);

    public static IServiceCollection AddFgsWorkerSecurity(
        this IServiceCollection services,
        IConfiguration configuration) => services;
}
