namespace Fgs.Messaging.Consumer;

public sealed class ConsumerRouteRegistration<TMessage> : IConsumerRouteRegistration
    where TMessage : class
{
    private readonly string _routingKey;
    private readonly Func<TMessage, ConsumerMessageContext, object> _commandFactory;

    public ConsumerRouteRegistration(
        string routingKey,
        Func<TMessage, ConsumerMessageContext, object> commandFactory)
    {
        _routingKey = routingKey;
        _commandFactory = commandFactory;
    }

    public void Apply(ConsumerRoutingRegistry registry)
    {
        registry.Register(new ConsumerRouteEntry
        {
            RoutingKey = _routingKey,
            MessageType = typeof(TMessage),
            CreateCommand = (message, context) => _commandFactory((TMessage)message, context)
        });
    }
}
