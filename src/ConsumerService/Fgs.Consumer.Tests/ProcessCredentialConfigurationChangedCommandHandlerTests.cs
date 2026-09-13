using Fgs.Contracts.IntegrationEvents;
using Fgs.Consumer.Application.Features.Credentials.Commands.ProcessCredentialConfigurationChanged;
using Fgs.Credentials.Abstractions;
using Fgs.Messaging.Consumer;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.Consumer.Tests;

public sealed class ProcessCredentialConfigurationChangedCommandHandlerTests
{
    [Fact]
    public async Task Handle_ReloadsCredentialConfiguration()
    {
        var provider = new Mock<ICredentialConfigurationProvider>();
        provider
            .Setup(p => p.ReloadAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new ProcessCredentialConfigurationChangedCommandHandler(
            provider.Object,
            NullLogger<ProcessCredentialConfigurationChangedCommandHandler>.Instance);

        await handler.Handle(
            new ProcessCredentialConfigurationChangedCommand(
                new CredentialConfigurationChangedEvent(DateTimeOffset.UtcNow),
                new ConsumerMessageContext
                {
                    RoutingKey = IntegrationEventRoutingKeys.CredentialConfigurationChanged,
                    MessageId = Guid.NewGuid().ToString("N"),
                    CorrelationId = Guid.NewGuid().ToString(),
                    RawBody = "{}"
                }),
            CancellationToken.None);

        provider.Verify(p => p.ReloadAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
