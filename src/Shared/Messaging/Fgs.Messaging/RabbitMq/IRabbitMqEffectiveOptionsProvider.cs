using Fgs.Messaging.Options;

namespace Fgs.Messaging.RabbitMq;

/// <summary>
/// Supplies the effective RabbitMQ options used for connections (may merge vault credentials with appsettings).
/// </summary>
public interface IRabbitMqEffectiveOptionsProvider
{
    RabbitMqOptions GetEffectiveOptions();
}
