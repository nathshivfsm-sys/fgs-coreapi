using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Contracts.IntegrationEvents;
using Fgs.Contracts.Requests;
using Fgs.Consumer.Application.Features.TenantProvisioning.Commands.ProcessTenantProvisionRequested;
using Fgs.Messaging.Consumer;
using Moq;

namespace Fgs.Consumer.Tests;

public sealed class ProcessTenantProvisionRequestedCommandHandlerTests
{
    [Fact]
    public async Task Handle_CallsSetupProvisioningClient()
    {
        var client = new Mock<ISetupClient>();
        client.Setup(c => c.ProvisionTenantAsync(It.IsAny<ProvisionTenantRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<object>.Ok(new object(), ApiStatusCodes.NoContent));

        var handler = new ProcessTenantProvisionRequestedCommandHandler(client.Object);
        var correlationId = Guid.NewGuid();
        var evt = new TenantProvisionRequestedEvent(10, 1, "ACME", correlationId);

        await handler.Handle(
            new ProcessTenantProvisionRequestedCommand(evt, CreateContext()),
            CancellationToken.None);

        client.Verify(
            c => c.ProvisionTenantAsync(
                It.Is<ProvisionTenantRequest>(r =>
                    r.TenantId == 10
                    && r.CompanyId == 1
                    && r.TenantCode == "ACME"
                    && r.CorrelationId == correlationId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenClientFails_Throws()
    {
        var client = new Mock<ISetupClient>();
        client.Setup(c => c.ProvisionTenantAsync(It.IsAny<ProvisionTenantRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<object>.Fail(["failed"], ApiStatusCodes.InternalServerError));

        var handler = new ProcessTenantProvisionRequestedCommandHandler(client.Object);

        var act = () => handler.Handle(
            new ProcessTenantProvisionRequestedCommand(
                new TenantProvisionRequestedEvent(10, 1, "ACME", Guid.NewGuid()),
                CreateContext()),
            CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    private static ConsumerMessageContext CreateContext() => new()
    {
        RoutingKey = IntegrationEventRoutingKeys.TenantProvisionRequested,
        MessageId = Guid.NewGuid().ToString("N"),
        CorrelationId = Guid.NewGuid().ToString(),
        RawBody = "{}"
    };
}
