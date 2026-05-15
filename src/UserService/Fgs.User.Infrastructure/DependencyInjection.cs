using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Application.Abstractions.Messaging;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Abstractions.Security;
using Fgs.User.Application.Abstractions.Time;
using Fgs.User.Infrastructure.Background;
using Fgs.User.Infrastructure.Database;
using Fgs.User.Infrastructure.Identity;
using Fgs.User.Infrastructure.Messaging;
using Fgs.User.Infrastructure.Options;
using Fgs.User.Infrastructure.Persistence;
using Fgs.User.Infrastructure.Security;
using Fgs.User.Infrastructure.Time;
using Microsoft.EntityFrameworkCore;
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
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IOutboxWriter, OutboxWriter>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IPasswordHasher, PasswordHasherService>();
        services.AddSingleton<IEmailNormalizer, EmailNormalizer>();
        services.AddSingleton<IInvitationTokenService, InvitationTokenService>();
        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        services.AddSingleton<IRabbitMqPublisher, RabbitMqPublisher>();
        services.AddHttpClient<IEntraExternalIdService, EntraExternalIdService>();
        //services.AddHostedService<OutboxProcessorService>();

        return services;
    }
}
