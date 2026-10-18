using Fgs.Contracts.Api;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Provisioning;
using Fgs.Setup.Application.Features.TenantProvisioning;
using Fgs.Setup.Application.Features.TenantProvisioning.Commands.SyncTenantCatalog;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.Setup.Tests;

public sealed class SyncTenantCatalogCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenTenantContextIsMissing_ReturnsBadRequestAndDoesNotSync()
    {
        var orchestrator = new Mock<ITenantProvisioningOrchestrator>();
        var handler = CreateHandler(orchestrator.Object, current: null);

        var response = await handler.Handle(new SyncTenantCatalogCommand(), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.BadRequest);
        response.Data.Should().BeNull();
        orchestrator.Verify(
            o => o.SyncMissingCatalogAsync(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    [InlineData(-1, 1)]
    [InlineData(1, -1)]
    public async Task Handle_WhenTenantOrCompanyIsNotPositive_ReturnsBadRequestAndDoesNotSync(
        long tenantId,
        long companyId)
    {
        var orchestrator = new Mock<ITenantProvisioningOrchestrator>();
        var handler = CreateHandler(
            orchestrator.Object,
            new TenantContext { TenantId = tenantId, CompanyId = companyId });

        var response = await handler.Handle(new SyncTenantCatalogCommand(), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.BadRequest);
        orchestrator.Verify(
            o => o.SyncMissingCatalogAsync(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenSeedSucceeds_ReturnsCountsAndTables()
    {
        var orchestrator = new Mock<ITenantProvisioningOrchestrator>();
        orchestrator
            .Setup(o => o.SyncMissingCatalogAsync(10, 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TenantDataSeedResult(
                2,
                1,
                0,
                [
                    new TenantSeedTableResult("billingcategory", TenantSeedTableOutcome.Succeeded, null, 4),
                    new TenantSeedTableResult("zone", TenantSeedTableOutcome.Skipped, "already present", 0)
                ]));

        var handler = CreateHandler(
            orchestrator.Object,
            new TenantContext { TenantId = 10, CompanyId = 1 });

        var response = await handler.Handle(new SyncTenantCatalogCommand(), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
        response.Data.Should().NotBeNull();
        response.Data!.SucceededCount.Should().Be(2);
        response.Data.SkippedCount.Should().Be(1);
        response.Data.FailedCount.Should().Be(0);
        response.Data.Tables.Should().BeEquivalentTo(
        [
            new TenantCatalogSyncTableDto("billingcategory", "Succeeded", 4, null),
            new TenantCatalogSyncTableDto("zone", "Skipped", 0, "already present")
        ]);
    }

    [Fact]
    public async Task Handle_WhenSeedHasFailures_ReturnsInternalServerError()
    {
        var orchestrator = new Mock<ITenantProvisioningOrchestrator>();
        orchestrator
            .Setup(o => o.SyncMissingCatalogAsync(10, 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TenantDataSeedResult(
                1,
                0,
                1,
                [new TenantSeedTableResult("tableX", TenantSeedTableOutcome.Failed, "seed failed")]));

        var handler = CreateHandler(
            orchestrator.Object,
            new TenantContext { TenantId = 10, CompanyId = 1 });

        var response = await handler.Handle(new SyncTenantCatalogCommand(), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.InternalServerError);
        response.Data.Should().BeNull();
        response.Errors.Should().Contain("seed failed");
    }

    private static SyncTenantCatalogCommandHandler CreateHandler(
        ITenantProvisioningOrchestrator orchestrator,
        ITenantContext? current)
    {
        var accessor = new Mock<ITenantContextAccessor>();
        accessor.SetupGet(a => a.Current).Returns(current);

        return new SyncTenantCatalogCommandHandler(
            orchestrator,
            accessor.Object,
            NullLogger<SyncTenantCatalogCommandHandler>.Instance);
    }
}
