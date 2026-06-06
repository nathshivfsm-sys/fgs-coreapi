using Fgs.Contracts.Api;
using Fgs.Contracts.IntegrationEvents;
using Fgs.Contracts.Requests;
using Fgs.Setup.Application.Abstractions.Provisioning;
using Fgs.Setup.Application.Features.TenantProvisioning.Commands.ProvisionTenant;
using Moq;

namespace Fgs.Setup.Tests;

public sealed class ProvisionTenantCommandHandlerTests
{
    [Fact]
    public async Task Handle_CallsTenantProvisioningOrchestrator()
    {
        var orchestrator = new Mock<ITenantProvisioningOrchestrator>();
        var handler = new ProvisionTenantCommandHandler(orchestrator.Object);
        var correlationId = Guid.NewGuid();
        var request = new ProvisionTenantRequest(10, 1, "ACME", correlationId);

        var response = await handler.Handle(new ProvisionTenantCommand(request), CancellationToken.None);

        response.Success.Should().BeTrue();
        orchestrator.Verify(
            o => o.ProvisionAsync(
                It.Is<TenantProvisionRequestedEvent>(e =>
                    e.TenantId == 10
                    && e.CompanyId == 1
                    && e.TenantCode == "ACME"
                    && e.CorrelationId == correlationId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenOrchestratorFails_ReturnsFailureResponse()
    {
        var orchestrator = new Mock<ITenantProvisioningOrchestrator>();
        orchestrator
            .Setup(o => o.ProvisionAsync(It.IsAny<TenantProvisionRequestedEvent>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Provisioning failed"));

        var handler = new ProvisionTenantCommandHandler(orchestrator.Object);
        var response = await handler.Handle(
            new ProvisionTenantCommand(new ProvisionTenantRequest(10, 1, "ACME", Guid.NewGuid())),
            CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.InternalServerError);
        response.Errors.Should().Contain("Provisioning failed");
    }
}
