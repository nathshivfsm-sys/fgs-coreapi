using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UserService.Application.Common.Abstractions;
using UserService.Domain.IntegrationEvents;
using UserService.Infrastructure.Configuration;

namespace UserService.Infrastructure.Events;

public sealed class ServiceBusIntegrationEventPublisher : IIntegrationEventPublisher
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    private readonly ServiceBusClient _client;
    private readonly ServiceBusOptions _options;
    private readonly ILogger<ServiceBusIntegrationEventPublisher> _logger;

    public ServiceBusIntegrationEventPublisher(
        ServiceBusClient client,
        IOptions<ServiceBusOptions> options,
        ILogger<ServiceBusIntegrationEventPublisher> logger)
    {
        _client = client;
        _options = options.Value;
        _logger = logger;
    }

    public async Task PublishAdminUserInviteCreatedAsync(
        AdminUserInviteCreatedEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        await using var sender = _client.CreateSender(_options.InviteEventsPath);

        var body = JsonSerializer.SerializeToUtf8Bytes(integrationEvent, JsonOptions);
        var message = new ServiceBusMessage(body)
        {
            Subject = nameof(AdminUserInviteCreatedEvent),
            ContentType = "application/json",
            ApplicationProperties =
            {
                ["EventType"] = nameof(AdminUserInviteCreatedEvent)
            }
        };

        try
        {
            await sender.SendMessageAsync(message, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(
                ex,
                "Failed to publish {Event}. Signup succeeded in DB; manual reconciliation may be required for {Email}.",
                nameof(AdminUserInviteCreatedEvent),
                integrationEvent.Email);

            throw;
        }
    }
}
