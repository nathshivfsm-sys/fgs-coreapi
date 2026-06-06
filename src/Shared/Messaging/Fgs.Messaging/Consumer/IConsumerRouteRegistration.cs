namespace Fgs.Messaging.Consumer;

public interface IConsumerRouteRegistration
{
    void Apply(ConsumerRoutingRegistry registry);
}
