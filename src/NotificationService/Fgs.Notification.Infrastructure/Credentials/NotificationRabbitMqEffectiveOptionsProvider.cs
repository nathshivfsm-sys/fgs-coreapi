using Fgs.Messaging.Options;
using Fgs.Messaging.RabbitMq;

namespace Fgs.Notification.Infrastructure.Credentials;

public sealed class NotificationRabbitMqEffectiveOptionsProvider(RabbitMqOptionsResolver resolver)
    : IRabbitMqEffectiveOptionsProvider
{
    public RabbitMqOptions GetEffectiveOptions() => resolver.Resolve();
}
