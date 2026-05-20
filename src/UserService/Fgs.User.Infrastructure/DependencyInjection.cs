using Fgs.User.Application.Abstractions.Geo;
using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Application.Abstractions.Messaging;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Abstractions.Security;
using Fgs.User.Application.Abstractions.Time;
using Fgs.User.Infrastructure.Background;
using Fgs.User.Infrastructure.Common.Geo;
using Fgs.User.Infrastructure.Common.Identity;
using Fgs.User.Infrastructure.Common.Options;
using Fgs.User.Infrastructure.Common.Security;
using Fgs.User.Infrastructure.Common.Time;
using Fgs.User.Infrastructure.Messaging;
using Fgs.User.Infrastructure.Persistence.Database.DbContexts;
using Fgs.User.Infrastructure.Persistence.Database.Repositories;
using Fgs.User.Infrastructure.Persistence.Database.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.User.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsUserInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.SectionName));
        services.Configure<EntraExternalIdOptions>(configuration.GetSection(EntraExternalIdOptions.SectionName));
        services.Configure<OutboxOptions>(configuration.GetSection(OutboxOptions.SectionName));
        services.Configure<SignupLocaleOptions>(configuration.GetSection(SignupLocaleOptions.SectionName));

        var connectionString = FgsUserConnectionString.ResolveRequired(configuration);

        services.AddDbContext<FgsUserDbContext>((_, options) =>
        {
            options.UseNpgsql(connectionString, npgsql =>
            {
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", FgsUserDbContext.FgsSchema);
                npgsql.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorCodesToAdd: null);
            });
            // SQL-script migrations may drift from the snapshot; do not block startup or drop constraints.
            options.ConfigureWarnings(w =>
                w.Ignore(RelationalEventId.PendingModelChangesWarning));
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IOutboxWriter, OutboxWriter>();
        services.AddScoped<IAddressLocaleResolver, AddressLocaleResolver>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IEmailNormalizer, EmailNormalizer>();
        services.AddSingleton<IInvitationTokenService, InvitationTokenService>();
        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        services.AddSingleton<IRabbitMqPublisher, RabbitMqPublisher>();
        services.AddHttpClient<IEntraExternalIdService, EntraExternalIdService>();
        services.AddHostedService<OutboxProcessorService>();

        return services;
    }
}
