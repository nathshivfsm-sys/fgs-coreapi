using Fgs.Contracts.Api;
using Fgs.Contracts.IntegrationEvents;
using Fgs.Contracts.Requests;
using Fgs.Credentials.Options;
using Fgs.Setup.Application.Abstractions.Provisioning;
using Fgs.Setup.Application.Features.TenantProvisioning.Commands.ProvisionTenant;
using Microsoft.Extensions.Options;
using Moq;

namespace Fgs.Setup.Tests;

public sealed class ProvisionTenantCommandHandlerTests
{
    private const string InternalServiceKey = "test-internal-key";

    [Fact]
    public async Task Handle_WithValidInternalServiceKey_CallsTenantProvisioningOrchestrator()
    {
        var orchestrator = new Mock<ITenantProvisioningOrchestrator>();
        var handler = CreateHandler(orchestrator.Object);
        var correlationId = Guid.NewGuid();
        var request = new ProvisionTenantRequest(10, 1, "ACME", correlationId);

        var response = await handler.Handle(
            new ProvisionTenantCommand(request, InternalServiceKey),
            CancellationToken.None);

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
    public async Task Handle_WithInvalidInternalServiceKey_ReturnsUnauthorized()
    {
        var orchestrator = new Mock<ITenantProvisioningOrchestrator>();
        var handler = CreateHandler(orchestrator.Object);

        var response = await handler.Handle(
            new ProvisionTenantCommand(
                new ProvisionTenantRequest(10, 1, "ACME", Guid.NewGuid()),
                "wrong-key"),
            CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.Unauthorized);
        orchestrator.Verify(
            o => o.ProvisionAsync(It.IsAny<TenantProvisionRequestedEvent>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenOrchestratorFails_Throws()
    {
        var orchestrator = new Mock<ITenantProvisioningOrchestrator>();
        orchestrator
            .Setup(o => o.ProvisionAsync(It.IsAny<TenantProvisionRequestedEvent>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Provisioning failed"));

        var handler = CreateHandler(orchestrator.Object);

        var act = () => handler.Handle(
            new ProvisionTenantCommand(
                new ProvisionTenantRequest(10, 1, "ACME", Guid.NewGuid()),
                InternalServiceKey),
            CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Provisioning failed");
    }

    private static ProvisionTenantCommandHandler CreateHandler(ITenantProvisioningOrchestrator orchestrator) =>
        new(
            orchestrator,
            Options.Create(new CredentialDistributionOptions
            {
                InternalServiceKey = InternalServiceKey
            }));
}
