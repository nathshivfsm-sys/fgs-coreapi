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
using Fgs.Notification.Application.Notifications.Dispatch;
using Fgs.Notification.Application.Notifications.Queues;
using Fgs.Notification.Application.Notifications.Preferences;
using Fgs.Notification.Application.Notifications.Templates;
using Fgs.Notification.Application.Reporting;
using Fgs.Notification.Infrastructure.Audit;
using Fgs.Notification.Infrastructure.BackgroundJobs;
using Fgs.Notification.Infrastructure.Configuration;
using Fgs.Notification.Infrastructure.Database;
using Fgs.Persistence.Extensions;
using Fgs.Notification.Infrastructure.Integrations.QuickBooks;
using Fgs.Notification.Infrastructure.Integrations.SendGrid;
using Fgs.Notification.Infrastructure.Integrations.Stripe;
using Fgs.Notification.Infrastructure.Integrations.Twilio;
using Fgs.Notification.Infrastructure.Notifications.Channels;
using Fgs.Notification.Infrastructure.Notifications.History;
using Fgs.Notification.Infrastructure.Notifications.Providers;
using Fgs.Notification.Infrastructure.Notifications.Providers.Email;
using Fgs.Notification.Infrastructure.Notifications.Providers.Push;
using Fgs.Notification.Infrastructure.Notifications.Providers.Sms;
using Fgs.Notification.Infrastructure.Notifications.Queues;
using Fgs.Notification.Infrastructure.Notifications.Preferences;
using Fgs.Notification.Infrastructure.Notifications.Templates;
using Fgs.Notification.Infrastructure.Options;
using Fgs.Notification.Infrastructure.Reporting;
using Fgs.Credentials;
using Fgs.Credentials.Abstractions;
using Fgs.Credentials.Extensions;
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
        services.AddFgsCredentialConsumer(
            configuration,
            configuration,
            options =>
            {
                options.ServiceName = "fgs-notification-service";
                options.RequiredProviders = ["DATABASE", "SENDGRID"];
            },
            typeof(SendGridOptions));

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.Configure<SendGridOptions>(configuration.GetSection(SendGridOptions.SectionName));
        services.Configure<SetupServiceClientOptions>(configuration.GetSection(SetupServiceClientOptions.SectionName));
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
            options.UseFgsNpgsql(connectionString, "__EFMigrationsHistory", FgsNotificationDbContext.MigrationHistorySchema);
        });

        services.AddScoped<INotificationHistoryRepository, NotificationHistoryRepository>();
        services.AddScoped<IIdempotencyStore, IdempotencyStore>();
        services.AddScoped<ICommunicationTemplateRepository, CommunicationTemplateRepository>();
        services.AddScoped<ICommunicationTemplateService, CommunicationTemplateService>();
        services.AddSingleton<ITemplateRenderer, TemplateRenderer>();
        services.AddScoped<INotificationTemplateRenderer, DatabaseNotificationTemplateRenderer>();
        services.AddSingleton<INotificationPreferenceService, PlaceholderNotificationPreferenceService>();
        services.AddSingleton<IIntegrationEventMapper, IntegrationEventMapper>();
        services.AddSingleton<INotificationDispatchRequestResolver, NotificationDispatchRequestResolver>();
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

        return services;
    }
}
