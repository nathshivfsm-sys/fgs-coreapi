using Azure.Messaging.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Common.Abstractions;
using UserService.Application.Common.Configuration;
using UserService.Application.Common.Persistence;
using UserService.Infrastructure.Configuration;
using UserService.Infrastructure.Events;
using UserService.Infrastructure.Persistence;
using UserService.Infrastructure.Security;

namespace UserService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EntraOptions>(configuration.GetSection(EntraOptions.SectionName));
        services.Configure<EntraIdentityOptions>(configuration.GetSection(EntraIdentityOptions.SectionName));
        services.Configure<ServiceBusOptions>(configuration.GetSection(ServiceBusOptions.SectionName));

        var connectionString = configuration.GetConnectionString("UserServiceDb")
            ?? throw new InvalidOperationException("Connection string 'UserServiceDb' is not configured.");

        services.AddDbContext<UserServiceDbContext>(options =>
            options.UseNpgsql(connectionString, npgsql =>
                npgsql.MigrationsHistoryTable("__ef_migrations_history", "fgs")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IInviteTokenService, InviteTokenService>();

        var serviceBusConnection = configuration.GetValue<string>($"{ServiceBusOptions.SectionName}:ConnectionString");
        if (string.IsNullOrWhiteSpace(serviceBusConnection))
        {
            services.AddSingleton<IIntegrationEventPublisher, NoOpIntegrationEventPublisher>();
        }
        else
        {
            services.AddSingleton(_ => new ServiceBusClient(serviceBusConnection));
            services.AddSingleton<IIntegrationEventPublisher, ServiceBusIntegrationEventPublisher>();
        }

        return services;
    }
}
