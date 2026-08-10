using Fgs.Messaging.Consumer;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.Messaging.Tests;

public sealed class MessageDispatcherTests
{
    private readonly Mock<IConsumerMessageRouter> _router = new();
    private readonly Mock<IConsumerIdempotencyStore> _idempotency = new();

    [Fact]
    public async Task DispatchAsync_WhenAlreadyProcessed_SkipsRoutingAndDoesNotMark()
    {
        _router.Setup(r => r.CanRoute("tenant.provision.requested")).Returns(true);
        _idempotency
            .Setup(i => i.HasBeenProcessedAsync("msg-1", "tenant.provision.requested", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var dispatcher = CreateDispatcher();
        await dispatcher.DispatchAsync(
            "tenant.provision.requested",
            ReadOnlyMemory<byte>.Empty,
            CreateContext("msg-1"),
            CancellationToken.None);

        _router.Verify(
            r => r.RouteAsync(
                It.IsAny<string>(),
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<ConsumerMessageContext>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
        _idempotency.Verify(
            i => i.TryMarkProcessedAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task DispatchAsync_WhenRoutingSucceeds_MarksProcessedAfterRoute()
    {
        var markCalled = false;
        _router.Setup(r => r.CanRoute("tenant.provision.requested")).Returns(true);
        _idempotency
            .Setup(i => i.HasBeenProcessedAsync("msg-1", "tenant.provision.requested", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _router
            .Setup(r => r.RouteAsync(
                "tenant.provision.requested",
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<ConsumerMessageContext>(),
                It.IsAny<CancellationToken>()))
            .Returns(() =>
            {
                markCalled.Should().BeFalse();
                return Task.CompletedTask;
            });
        _idempotency
            .Setup(i => i.TryMarkProcessedAsync("msg-1", "tenant.provision.requested", It.IsAny<CancellationToken>()))
            .Returns(() =>
            {
                markCalled = true;
                return Task.FromResult(true);
            });

        var dispatcher = CreateDispatcher();
        await dispatcher.DispatchAsync(
            "tenant.provision.requested",
            "payload"u8.ToArray(),
            CreateContext("msg-1"),
            CancellationToken.None);

        markCalled.Should().BeTrue();
        _router.Verify(
            r => r.RouteAsync(
                "tenant.provision.requested",
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<ConsumerMessageContext>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task DispatchAsync_WhenRoutingFails_DoesNotMarkProcessed()
    {
        _router.Setup(r => r.CanRoute("tenant.provision.requested")).Returns(true);
        _idempotency
            .Setup(i => i.HasBeenProcessedAsync("msg-1", "tenant.provision.requested", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _router
            .Setup(r => r.RouteAsync(
                It.IsAny<string>(),
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<ConsumerMessageContext>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("downstream failed"));

        var dispatcher = CreateDispatcher();
        var act = () => dispatcher.DispatchAsync(
            "tenant.provision.requested",
            "payload"u8.ToArray(),
            CreateContext("msg-1"),
            CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>();
        _idempotency.Verify(
            i => i.TryMarkProcessedAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task DispatchAsync_WhenRoutingFailsThenSucceeds_AllowsRetryAndMarksOnce()
    {
        _router.Setup(r => r.CanRoute("tenant.provision.requested")).Returns(true);
        _idempotency
            .Setup(i => i.HasBeenProcessedAsync("msg-1", "tenant.provision.requested", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _idempotency
            .Setup(i => i.TryMarkProcessedAsync("msg-1", "tenant.provision.requested", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var attempts = 0;
        _router
            .Setup(r => r.RouteAsync(
                It.IsAny<string>(),
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<ConsumerMessageContext>(),
                It.IsAny<CancellationToken>()))
            .Returns(() =>
            {
                attempts++;
                if (attempts == 1)
                {
                    return Task.FromException(new InvalidOperationException("transient"));
                }

                return Task.CompletedTask;
            });

        var dispatcher = CreateDispatcher();
        var context = CreateContext("msg-1");
        var body = "payload"u8.ToArray();

        var first = () => dispatcher.DispatchAsync(
            "tenant.provision.requested",
            body,
            context,
            CancellationToken.None);
        await first.Should().ThrowAsync<InvalidOperationException>();
        _idempotency.Verify(
            i => i.TryMarkProcessedAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);

        await dispatcher.DispatchAsync(
            "tenant.provision.requested",
            body,
            context,
            CancellationToken.None);

        _router.Verify(
            r => r.RouteAsync(
                "tenant.provision.requested",
                It.IsAny<ReadOnlyMemory<byte>>(),
                It.IsAny<ConsumerMessageContext>(),
                It.IsAny<CancellationToken>()),
            Times.Exactly(2));
        _idempotency.Verify(
            i => i.TryMarkProcessedAsync("msg-1", "tenant.provision.requested", It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task DispatchAsync_WhenNoRoute_DoesNotTouchIdempotency()
    {
        _router.Setup(r => r.CanRoute("unknown")).Returns(false);

        var dispatcher = CreateDispatcher();
        await dispatcher.DispatchAsync(
            "unknown",
            ReadOnlyMemory<byte>.Empty,
            CreateContext("msg-1"),
            CancellationToken.None);

        _idempotency.Verify(
            i => i.HasBeenProcessedAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _idempotency.Verify(
            i => i.TryMarkProcessedAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    private MessageDispatcher CreateDispatcher() =>
        new(
            _router.Object,
            _idempotency.Object,
            NullLogger<MessageDispatcher>.Instance);

    private static ConsumerMessageContext CreateContext(string messageId) => new()
    {
        RoutingKey = "tenant.provision.requested",
        MessageId = messageId,
        CorrelationId = "corr-1",
        RawBody = "{}"
    };
}
