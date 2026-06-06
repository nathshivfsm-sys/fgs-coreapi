using Fgs.Messaging.Consumer;

namespace Fgs.Messaging.Tests;

public sealed class ConsumerRoutingRegistryTests
{
    [Fact]
    public void Register_ResolvesRoutingKey()
    {
        var registry = new ConsumerRoutingRegistry();
        var registration = new ConsumerRouteRegistration<TestMessage>(
            "test.routing.key",
            (message, _) => message);

        registration.Apply(registry);

        registry.TryGet("test.routing.key", out var entry).Should().BeTrue();
        entry!.MessageType.Should().Be(typeof(TestMessage));
        entry.CreateCommand(new TestMessage("value"), new ConsumerMessageContext
        {
            RoutingKey = "test.routing.key",
            MessageId = "m1",
            RawBody = "{}"
        }).Should().BeOfType<TestMessage>();
    }

    private sealed record TestMessage(string Value);
}
