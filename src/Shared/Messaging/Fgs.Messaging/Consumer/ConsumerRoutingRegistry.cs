namespace Fgs.Messaging.Consumer;

public sealed class ConsumerRoutingRegistry
{
    private readonly Dictionary<string, ConsumerRouteEntry> _routes = new(StringComparer.Ordinal);

    public void Register(ConsumerRouteEntry entry) =>
        _routes[entry.RoutingKey] = entry;

    public bool TryGet(string routingKey, out ConsumerRouteEntry entry) =>
        _routes.TryGetValue(routingKey, out entry!);

    public IReadOnlyCollection<string> RoutingKeys => _routes.Keys;
}
