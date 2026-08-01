using Fgs.Notification.Application.Configuration;
using Fgs.Notification.Application.Integrations.SendGrid;
using Fgs.Notification.Application.Notifications.Channels;
using Fgs.Notification.Application.Notifications.History;
using Fgs.Notification.Application.Notifications.Providers;
using Fgs.Notification.Application.Notifications.Dispatch;
using Fgs.Notification.Application.Notifications.Queues;
using Fgs.Notification.Application.Notifications.Templates;
using Fgs.Notification.Domain.Enums;
using Fgs.Notification.Infrastructure.Configuration;
using Fgs.Notification.Infrastructure.Database;
using Fgs.Notification.Infrastructure.Database.Schemas;
using Fgs.Persistence.Extensions;
using Fgs.Notification.Infrastructure.Integrations.SendGrid;
using Fgs.Notification.Infrastructure.Notifications.Channels;
using Fgs.Notification.Infrastructure.Notifications.History;
using Fgs.Notification.Infrastructure.Notifications.Providers;
using Fgs.Notification.Infrastructure.Notifications.Providers.Email;
using Fgs.Notification.Infrastructure.Notifications.Providers.Push;
using Fgs.Notification.Infrastructure.Notifications.Providers.Sms;
using Fgs.Notification.Infrastructure.Notifications.Queues;
using Fgs.Notification.Infrastructure.Notifications.Templates;
using Fgs.Notification.Infrastructure.Options;
using Fgs.Credentials;
using Fgs.Credentials.Abstractions;
using Fgs.Credentials.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Notification.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsNotificationInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddFgsCredentialConsumer(
            configuration,
            configuration,
            options =>
            {
                options.ServiceName = "fgs-notification-service";
                options.RequiredProviders = ["DATABASE", "SENDGRID"];
            },
            typeof(SendGridOptions));

        services.Configure<SendGridOptions>(configuration.GetSection(SendGridOptions.SectionName));
        services.Configure<TenantProviderOptions>(configuration.GetSection(TenantProviderOptions.SectionName));
        services.Configure<NotificationFeatureFlagsOptions>(configuration.GetSection(NotificationFeatureFlagsOptions.SectionName));
        services.Configure<NotificationOptions>(configuration.GetSection(NotificationOptions.SectionName));

        services.AddDbContext<FgsNotificationDbContext>((sp, options) =>
        {
            var connectionString = ConnectionStringResolver.ResolveRequired(
                sp.GetRequiredService<IConfiguration>(),
                ConnectionStringNames.FgsNotification,
                "FGS_NOTIFICATION_DB",
                sp.GetService<ICredentialConfigurationProvider>());
            var nullTranslator = new Npgsql.NameTranslation.NpgsqlNullNameTranslator();
            options.UseFgsNpgsql(
                connectionString,
                "__EFMigrationsHistory",
                FgsNotificationDbContext.MigrationHistorySchema,
                npgsql =>
                {
                    npgsql.MapEnum<NotificationStatus>(
                        "notification_status", FgsDatabaseSchemas.Notification, nameTranslator: nullTranslator);
                    npgsql.MapEnum<NotificationSourceApplication>(
                        "source_application", FgsDatabaseSchemas.Notification, nameTranslator: nullTranslator);
                });
        });

        services.AddScoped<INotificationHistoryRepository, NotificationHistoryRepository>();
        services.AddScoped<IIdempotencyStore, IdempotencyStore>();
        services.AddScoped<ICommunicationTemplateRepository, CommunicationTemplateRepository>();
        services.AddScoped<ICommunicationTemplateService, CommunicationTemplateService>();
        services.AddSingleton<ITemplateRenderer, TemplateRenderer>();
        services.AddScoped<INotificationTemplateRenderer, DatabaseNotificationTemplateRenderer>();
        services.AddSingleton<IIntegrationEventMapper, IntegrationEventMapper>();
        services.AddSingleton<INotificationDispatchRequestResolver, NotificationDispatchRequestResolver>();
        services.AddScoped<INotificationDispatcher, NotificationDispatcher>();
        services.AddSingleton<INotificationProviderFactory, NotificationProviderFactory>();

        services.AddSingleton<SendGridEmailProvider>();
        services.AddSingleton<SmtpEmailProvider>();
        services.AddSingleton<TwilioSmsProvider>();
        services.AddSingleton<FirebasePushProvider>();

        services.AddSingleton<ISendGridIntegrationClient, SendGridIntegrationClient>();

        services.AddSingleton<ITenantConfigurationResolver, TenantConfigurationResolver>();

        return services;
    }
}
