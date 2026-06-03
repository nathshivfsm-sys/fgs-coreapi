using Fgs.Messaging.Options;
using Fgs.Messaging.RabbitMq;

namespace Fgs.Platform.Infrastructure.Credentials;

public sealed class PlatformRabbitMqEffectiveOptionsProvider(RabbitMqOptionsResolver resolver)
    : IRabbitMqEffectiveOptionsProvider
{
    public RabbitMqOptions GetEffectiveOptions() => resolver.Resolve();
}
