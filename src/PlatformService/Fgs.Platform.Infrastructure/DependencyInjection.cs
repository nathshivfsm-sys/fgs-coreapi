using Fgs.Platform.Application.Audit;
using Fgs.Platform.Application.BackgroundJobs;
using Fgs.Platform.Application.Configuration;
using Fgs.Platform.Application.Integrations.QuickBooks;
using Fgs.Platform.Application.Integrations.SendGrid;
using Fgs.Platform.Application.Integrations.Stripe;
using Fgs.Platform.Application.Integrations.Twilio;
using Fgs.Platform.Application.Notifications.Channels;
using Fgs.Platform.Application.Notifications.History;
using Fgs.Platform.Application.Notifications.Providers;
using Fgs.Platform.Application.Notifications.Queues;
using Fgs.Platform.Application.Notifications.Preferences;
using Fgs.Platform.Application.Notifications.Templates;
using Fgs.Platform.Application.Reporting;
using Fgs.Platform.Infrastructure.Audit;
using Fgs.Platform.Infrastructure.BackgroundJobs;
using Fgs.Platform.Infrastructure.Configuration;
using Fgs.Platform.Infrastructure.Database;
using Fgs.Platform.Infrastructure.Integrations.QuickBooks;
using Fgs.Platform.Infrastructure.Integrations.SendGrid;
using Fgs.Platform.Infrastructure.Integrations.Stripe;
using Fgs.Platform.Infrastructure.Integrations.Twilio;
using Fgs.Platform.Infrastructure.Messaging;
using Fgs.Platform.Infrastructure.Notifications.Channels;
using Fgs.Platform.Infrastructure.Notifications.History;
using Fgs.Platform.Infrastructure.Notifications.Providers;
using Fgs.Platform.Infrastructure.Notifications.Providers.Email;
using Fgs.Platform.Infrastructure.Notifications.Providers.Push;
using Fgs.Platform.Infrastructure.Notifications.Providers.Sms;
using Fgs.Platform.Infrastructure.Notifications.Queues;
using Fgs.Platform.Infrastructure.Notifications.Preferences;
using Fgs.Platform.Infrastructure.Notifications.Templates;
using Fgs.Platform.Infrastructure.Notifications.Workers;
using Fgs.Platform.Infrastructure.Options;
using Fgs.Platform.Infrastructure.Reporting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Platform.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsPlatformInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.SectionName));
        services.Configure<SendGridOptions>(configuration.GetSection(SendGridOptions.SectionName));
        services.Configure<TenantProviderOptions>(configuration.GetSection(TenantProviderOptions.SectionName));
        services.Configure<PlatformFeatureFlagsOptions>(configuration.GetSection(PlatformFeatureFlagsOptions.SectionName));
        services.Configure<NotificationWorkerOptions>(configuration.GetSection(NotificationWorkerOptions.SectionName));

        var connectionString = FgsPlatformConnectionString.ResolveRequired(configuration);

        services.AddDbContext<FgsPlatformDbContext>((_, options) =>
        {
            options.UseNpgsql(connectionString, npgsql =>
            {
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", FgsPlatformDbContext.FgsSchema);
                npgsql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            });
        });

        services.AddSingleton<RabbitMqConnectionFactory>();

        services.AddScoped<INotificationHistoryRepository, NotificationHistoryRepository>();
        services.AddScoped<IIdempotencyStore, IdempotencyStore>();
        services.AddSingleton<INotificationTemplateRenderer, NotificationTemplateRenderer>();
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

        services.AddHostedService<NotificationQueueWorker>();

        return services;
    }
}
