using Fgs.Notification.Application.Audit;
using Fgs.Notification.Application.BackgroundJobs;
using Fgs.Notification.Application.Configuration;
using Fgs.Notification.Application.Integrations.QuickBooks;
using Fgs.Notification.Application.Integrations.SendGrid;
using Fgs.Notification.Application.Integrations.Stripe;
using Fgs.Notification.Application.Integrations.Twilio;
using Fgs.Notification.Application.Notifications.Channels;
using Fgs.Notification.Application.Notifications.History;
using Fgs.Notification.Application.Notifications.Providers;
using Fgs.Notification.Application.Notifications.Queues;
using Fgs.Notification.Application.Notifications.Preferences;
using Fgs.Notification.Application.Notifications.Templates;
using Fgs.Notification.Application.Reporting;
using Fgs.Notification.Infrastructure.Audit;
using Fgs.Notification.Infrastructure.BackgroundJobs;
using Fgs.Notification.Infrastructure.Configuration;
using Fgs.Notification.Infrastructure.Database;
using Fgs.Notification.Infrastructure.Templates;
using Fgs.Notification.Infrastructure.Integrations.QuickBooks;
using Fgs.Notification.Infrastructure.Integrations.SendGrid;
using Fgs.Notification.Infrastructure.Integrations.Stripe;
using Fgs.Notification.Infrastructure.Integrations.Twilio;
using Fgs.Notification.Infrastructure.Messaging;
using Fgs.Notification.Infrastructure.Notifications.Channels;
using Fgs.Notification.Infrastructure.Notifications.History;
using Fgs.Notification.Infrastructure.Notifications.Providers;
using Fgs.Notification.Infrastructure.Notifications.Providers.Email;
using Fgs.Notification.Infrastructure.Notifications.Providers.Push;
using Fgs.Notification.Infrastructure.Notifications.Providers.Sms;
using Fgs.Notification.Infrastructure.Notifications.Queues;
using Fgs.Notification.Infrastructure.Notifications.Preferences;
using Fgs.Notification.Infrastructure.Notifications.Templates;
using Fgs.Notification.Infrastructure.Notifications.Workers;
using Fgs.Security.Authorization;
using Fgs.Security.Extensions;
using Fgs.Messaging.Extensions;
using Fgs.Messaging.Options;
using Fgs.Messaging.RabbitMq;
using Fgs.Notification.Infrastructure.Options;
using Fgs.Notification.Infrastructure.Credentials;
using Fgs.Notification.Infrastructure.Reporting;
using System.Reflection;
using MediatR;
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
        services.AddFgsEntraAuthentication(configuration);
        services.AddFgsRemoteClaimsEnrichment(configuration);
        services.AddNotificationResolvedCredentialConfiguration(configuration, configuration);

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.SectionName));
        services.PostConfigure<RabbitMqOptions>(options =>
        {
            options.ClientProvidedName = "Fgs.Notification";
            options.AutomaticRecoveryEnabled = true;
        });
        services.Configure<SendGridOptions>(configuration.GetSection(SendGridOptions.SectionName));
        services.Configure<TenantProviderOptions>(configuration.GetSection(TenantProviderOptions.SectionName));
        services.Configure<NotificationFeatureFlagsOptions>(configuration.GetSection(NotificationFeatureFlagsOptions.SectionName));
        services.Configure<NotificationWorkerOptions>(configuration.GetSection(NotificationWorkerOptions.SectionName));
        services.Configure<NotificationOptions>(configuration.GetSection(NotificationOptions.SectionName));

        var connectionString = FgsNotificationConnectionString.ResolveRequired(configuration);

        services.AddDbContext<FgsNotificationDbContext>((_, options) =>
        {
            options.UseNpgsql(connectionString, npgsql =>
            {
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", FgsNotificationDbContext.MigrationHistorySchema);
                npgsql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            });
        });

        services.AddFgsRabbitMqConnectionFactory();
        services.AddSingleton<IRabbitMqEffectiveOptionsProvider, NotificationRabbitMqEffectiveOptionsProvider>();
        services.AddSingleton<NotificationRabbitMqTopologyInitializer>();

        services.AddScoped<INotificationHistoryRepository, NotificationHistoryRepository>();
        services.AddScoped<IIdempotencyStore, IdempotencyStore>();
        services.AddSetupTemplateClient(configuration);
        services.AddScoped<ICommunicationTemplateRepository, CommunicationTemplateRepository>();
        services.AddScoped<ICommunicationTemplateService, CommunicationTemplateService>();
        services.AddSingleton<ITemplateRenderer, TemplateRenderer>();
        services.AddScoped<INotificationTemplateRenderer, DatabaseNotificationTemplateRenderer>();
        services.AddSingleton<INotificationPreferenceService, PlaceholderNotificationPreferenceService>();
        services.AddSingleton<IIntegrationEventMapper, IntegrationEventMapper>();
        services.AddScoped<INotificationDispatcher, NotificationDispatcher>();
        services.AddSingleton<INotificationProviderFactory, NotificationProviderFactory>();

        services.AddSingleton<SendGridEmailProvider>();
        services.AddSingleton<SmtpEmailProvider>();
        services.AddSingleton<TwilioSmsProvider>();
        services.AddSingleton<FirebasePushProvider>();

        services.AddSingleton<ISendGridIntegrationClient, SendGridIntegrationClient>();
        services.AddSingleton<IQuickBooksIntegrationClient, QuickBooksIntegrationClient>();
        services.AddSingleton<IStripeIntegrationClient, StripeIntegrationClient>();
        services.AddSingleton<ITwilioIntegrationClient, TwilioIntegrationClient>();

        services.AddSingleton<ITenantConfigurationResolver, TenantConfigurationResolver>();
        services.AddSingleton<IAuditLogger, NoOpAuditLogger>();
        services.AddSingleton<IBackgroundJobQueue, InMemoryBackgroundJobQueue>();
        services.AddSingleton<IReportExporter, PlaceholderReportExporter>();

        services.AddHostedService<CredentialConfigurationBootstrapHostedService>();
        services.AddHostedService<NotificationQueueWorker>();
        services.AddHostedService<CredentialConfigurationReloadConsumerService>();

        return services;
    }
}
