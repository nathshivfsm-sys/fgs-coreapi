using System.Security.Claims;
using Fgs.Security.Abstractions;
using Fgs.Security.Options;
using Fgs.Security.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

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

        services.AddHttpContextAccessor();
        services.AddScoped<IFgsUserContext, HttpFgsUserContext>();

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = authority;
                options.MetadataAddress = entraOptions.ResolveMetadataAddress();
                options.Audience = entraOptions.ClientId;
                options.MapInboundClaims = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    NameClaimType = "name",
                    RoleClaimType = ClaimTypes.Role,
                    ClockSkew = TimeSpan.FromMinutes(1)
                };
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        if (context.Principal is null)
                        {
                            return;
                        }

                        var enricher = context.HttpContext.RequestServices.GetService<IFgsClaimsEnricher>();
                        if (enricher is not null)
                        {
                            await enricher.EnrichAsync(context.Principal, context.HttpContext.RequestAborted);
                        }
                    }
                };
            });

        services.AddFgsAuthorization();
        return services;
    }

    public static IServiceCollection AddFgsRemoteClaimsEnrichment(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<UserServiceClientOptions>(
            configuration.GetSection(UserServiceClientOptions.SectionName));

        var userServiceOptions = configuration
                                     .GetSection(UserServiceClientOptions.SectionName)
                                     .Get<UserServiceClientOptions>()
                                 ?? new UserServiceClientOptions();

        services.AddHttpClient<IFgsClaimsEnricher, RemoteFgsClaimsEnricher>(client =>
        {
            client.BaseAddress = new Uri(userServiceOptions.BaseUrl.TrimEnd('/') + "/");
        });

        return services;
    }
}
