using Fgs.Contracts.Clients;
using Fgs.Contracts.IntegrationEvents;
using Fgs.Credentials.Extensions;
using Fgs.Messaging.Options;
using Fgs.Consumer.Application.Features.Audit.Commands.ProcessCredentialAuditRequested;
using Fgs.Consumer.Application.Features.Notifications.Commands.ProcessCompanySignupInviteEmail;
using Fgs.Consumer.Application.Features.TenantProvisioning.Commands.ProcessTenantProvisionRequested;
using Fgs.Consumer.Infrastructure.Messaging;
using Fgs.Foundation.Extensions;
using Fgs.Messaging.Consumer;
using Fgs.Messaging.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Consumer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsConsumerInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddFgsCredentialConsumer(
            configuration,
            configuration,
            options =>
            {
                options.ServiceName = "fgs-consumer-service";
                options.RequiredProviders = ["RABBITMQ"];
                options.RegisterSetupClient = false;
            },
            typeof(RabbitMqOptions));
        services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.SectionName));

        services.AddFgsSetupClient(configuration);

        services.AddFgsRefitClient<INotificationDispatchClient>(
            configuration,
            "NotificationService:BaseUrl",
            "http://notification-service:5002");

        services.AddFgsInternalServiceRefitClient<IAuditClient>(
            configuration,
            "AuditService:BaseUrl",
            "http://audit-service:5008");

        services.AddFgsRabbitMqConsumerFramework(configuration);
        services.AddScoped<IConsumerMessageRouter, MediatRConsumerMessageRouter>();

        services.AddConsumerRouting<TenantProvisionRequestedEvent>(
            IntegrationEventRoutingKeys.TenantProvisionRequested,
            (evt, ctx) => new ProcessTenantProvisionRequestedCommand(evt, ctx));

        services.AddConsumerRouting<CompanySignupInviteEmailEvent>(
            IntegrationEventRoutingKeys.CompanySignupInviteEmail,
            (evt, ctx) => new ProcessCompanySignupInviteEmailCommand(evt, ctx));

        services.AddConsumerRouting<CredentialAuditRequestedEvent>(
            IntegrationEventRoutingKeys.CredentialAuditRequested,
            (evt, ctx) => new ProcessCredentialAuditRequestedCommand(evt, ctx));

        return services;
    }
}
