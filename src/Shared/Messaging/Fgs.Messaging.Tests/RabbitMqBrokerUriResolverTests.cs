using FluentAssertions;
using Fgs.Messaging.Options;
using Fgs.Messaging.RabbitMq;
using Xunit;

namespace Fgs.Messaging.Tests;

public sealed class RabbitMqBrokerUriResolverTests
{
    [Fact]
    public void Resolve_RewritesLocalhostConnectionUri_WhenComposeHostNameIsRabbitmq()
    {
        var options = new RabbitMqOptions
        {
            ConnectionUri = "amqp://fgs:secret@localhost:5672/%2f",
            HostName = "rabbitmq",
            Port = 5672
        };

        var uri = RabbitMqBrokerUriResolver.Resolve(options);

        uri.Host.Should().Be("rabbitmq");
        uri.Port.Should().Be(5672);
        uri.UserInfo.Should().Be("fgs:secret");
    }

    [Fact]
    public void Resolve_KeepsAmazonMqConnectionUri_WhenHostNameIsRabbitmq()
    {
        var options = new RabbitMqOptions
        {
            ConnectionUri = "amqps://user:pass@b-abc.mq.us-east-1.on.aws:5671",
            HostName = "rabbitmq",
            Port = 5672
        };

        var uri = RabbitMqBrokerUriResolver.Resolve(options);

        uri.Host.Should().Be("b-abc.mq.us-east-1.on.aws");
        uri.Port.Should().Be(5671);
        uri.Scheme.Should().Be("amqps");
    }

    [Fact]
    public void Resolve_KeepsLocalhostConnectionUri_WhenHostNameIsAlsoLocalhost()
    {
        var options = new RabbitMqOptions
        {
            ConnectionUri = "amqp://guest:guest@127.0.0.1:5672/",
            HostName = "localhost",
            Port = 5672
        };

        var uri = RabbitMqBrokerUriResolver.Resolve(options);

        uri.Host.Should().Be("127.0.0.1");
    }

    [Fact]
    public void Resolve_UsesHostName_WhenConnectionUriMissing()
    {
        var options = new RabbitMqOptions
        {
            HostName = "rabbitmq",
            Port = 5672,
            UserName = "fgs",
            Password = "secret"
        };

        var uri = RabbitMqBrokerUriResolver.Resolve(options);

        uri.Should().Be(new Uri("amqp://fgs:secret@rabbitmq:5672/"));
    }
}
