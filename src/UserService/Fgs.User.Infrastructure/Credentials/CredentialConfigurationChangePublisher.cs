using System.Text.Json;
using Fgs.Contracts.IntegrationEvents;
using Fgs.Messaging.Abstractions;
using Fgs.Messaging.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fgs.User.Infrastructure.Credentials;

public interface ICredentialConfigurationChangePublisher
{
    Task PublishAsync(CancellationToken cancellationToken = default);
}

public sealed class CredentialConfigurationChangePublisher(
    IMessagePublisher publisher,
    IOptions<RabbitMqOptions> rabbitOptions,
    ILogger<CredentialConfigurationChangePublisher> logger) : ICredentialConfigurationChangePublisher
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task PublishAsync(CancellationToken cancellationToken = default)
    {
        var exchange = rabbitOptions.Value.ExchangeName;
        if (string.IsNullOrWhiteSpace(exchange))
        {
            logger.LogWarning("Skipping credential configuration change publish: RabbitMq:ExchangeName is not configured.");
            return;
        }

        var payload = new CredentialConfigurationChangedEvent(DateTimeOffset.UtcNow);
        var body = JsonSerializer.SerializeToUtf8Bytes(payload, JsonOptions);

        await publisher.PublishAsync(
            exchange,
            IntegrationEventRoutingKeys.CredentialConfigurationChanged,
            body,
            cancellationToken: cancellationToken);

        logger.LogInformation(
            "Published {EventType} to exchange {Exchange} with routing key {RoutingKey}.",
            IntegrationEventTypes.CredentialConfigurationChanged,
            exchange,
            IntegrationEventRoutingKeys.CredentialConfigurationChanged);
    }
}
