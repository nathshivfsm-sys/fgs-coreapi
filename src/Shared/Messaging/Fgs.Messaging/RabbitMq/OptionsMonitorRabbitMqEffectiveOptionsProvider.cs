using Fgs.Messaging.Options;
using Microsoft.Extensions.Options;

namespace Fgs.Messaging.RabbitMq;

public sealed class OptionsMonitorRabbitMqEffectiveOptionsProvider(IOptionsMonitor<RabbitMqOptions> optionsMonitor)
    : IRabbitMqEffectiveOptionsProvider
{
    public RabbitMqOptions GetEffectiveOptions() => optionsMonitor.CurrentValue;
}
