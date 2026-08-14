using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Contracts.IntegrationEvents;
using Fgs.MultiTenancy.Persistence;
using Fgs.Setup.Application.Abstractions.Provisioning;
using Fgs.Setup.Application.Features.TenantProvisioning;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Database;
using Fgs.Setup.Infrastructure.Provisioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.Setup.Tests;

public sealed class TenantProvisioningOrchestratorTests
{
    [Fact]
    public async Task ProvisionAsync_ResolvesGloBusinessTypeIdsFromSetupFgsBusinessType()
    {
        await using var context = await CreateContextAsync();
        var now = DateTimeOffset.UtcNow;

        context.GloBusinessTypes.AddRange(
            new GloBusinessType { Id = 5, Code = "HVAC", Name = "HVAC", IsActive = true, CreatedOn = now },
            new GloBusinessType { Id = 9, Code = "PLUMBING", Name = "Plumbing", IsActive = true, CreatedOn = now });
        context.FgsBusinessTypes.AddRange(
            new FgsBusinessType { Id = 1, TenantId = 10, CompanyId = 1, Code = "HVAC", Name = "HVAC", IsActive = true, CreatedOn = now },
            new FgsBusinessType { Id = 2, TenantId = 10, CompanyId = 1, Code = "PLUMBING", Name = "Plumbing", IsActive = true, CreatedOn = now });
        await context.SaveChangesAsync();

        IReadOnlyList<int>? capturedGloIds = null;
        var seedingEngine = new Mock<ITenantDataSeedingEngine>();
        seedingEngine
            .Setup(s => s.SeedTenantDataAsync(10, 1, It.IsAny<IReadOnlyList<int>?>(), It.IsAny<CancellationToken>()))
            .Callback<long, long, IReadOnlyList<int>?, CancellationToken>((_, _, ids, _) => capturedGloIds = ids)
            .ReturnsAsync(new TenantDataSeedResult(1, 0, 0, []));

        var userTenantClient = CreateUserTenantClientMock();
        var fileTenantClient = CreateFileTenantClientMock();

        var orchestrator = new TenantProvisioningOrchestrator(
            userTenantClient.Object,
            CreateUserCompanyClientMock().Object,
            fileTenantClient.Object,
            seedingEngine.Object,
            context,
            NullLogger<TenantProvisioningOrchestrator>.Instance);

        await orchestrator.ProvisionAsync(
            new TenantProvisionRequestedEvent(10, 1, "ACME", Guid.NewGuid()),
            CancellationToken.None);

        capturedGloIds.Should().NotBeNull();
        capturedGloIds!.OrderBy(id => id).Should().Equal(5, 9);
    }

    [Fact]
    public async Task ProvisionAsync_PassesEmptyGloIdsWhenNoFgsBusinessTypesExist()
    {
        await using var context = await CreateContextAsync();

        IReadOnlyList<int>? capturedGloIds = null;
        var seedingEngine = new Mock<ITenantDataSeedingEngine>();
        seedingEngine
            .Setup(s => s.SeedTenantDataAsync(10, 1, It.IsAny<IReadOnlyList<int>?>(), It.IsAny<CancellationToken>()))
            .Callback<long, long, IReadOnlyList<int>?, CancellationToken>((_, _, ids, _) => capturedGloIds = ids)
            .ReturnsAsync(new TenantDataSeedResult(0, 0, 0, []));

        var orchestrator = new TenantProvisioningOrchestrator(
            CreateUserTenantClientMock().Object,
            CreateUserCompanyClientMock().Object,
            CreateFileTenantClientMock().Object,
            seedingEngine.Object,
            context,
            NullLogger<TenantProvisioningOrchestrator>.Instance);

        await orchestrator.ProvisionAsync(
            new TenantProvisionRequestedEvent(10, 1, "ACME", Guid.NewGuid()),
            CancellationToken.None);

        capturedGloIds.Should().NotBeNull();
        capturedGloIds!.Should().BeEmpty();
    }

    [Fact]
    public async Task ProvisionAsync_WhenSeedHasFailures_ThrowsAndSetsProvisioningFailed()
    {
        await using var context = await CreateContextAsync();

        var seedingEngine = new Mock<ITenantDataSeedingEngine>();
        seedingEngine
            .Setup(s => s.SeedTenantDataAsync(10, 1, It.IsAny<IReadOnlyList<int>?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TenantDataSeedResult(
                0,
                0,
                1,
                [new TenantSeedTableResult("tableX", TenantSeedTableOutcome.Failed, "seed failed")]));

        var userTenantClient = CreateUserTenantClientMock();
        var fileTenantClient = CreateFileTenantClientMock();

        var orchestrator = new TenantProvisioningOrchestrator(
            userTenantClient.Object,
            CreateUserCompanyClientMock().Object,
            fileTenantClient.Object,
            seedingEngine.Object,
            context,
            NullLogger<TenantProvisioningOrchestrator>.Instance);

        var act = () => orchestrator.ProvisionAsync(
            new TenantProvisionRequestedEvent(10, 1, "ACME", Guid.NewGuid()),
            CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*seed completed with failures*");

        userTenantClient.Verify(
            c => c.UpdateStatusAsync(
                10,
                It.Is<UpdateTenantStatusRequest>(r => r.FgsTenantStatusId == TenantStatusIds.ProvisioningFailed),
                It.IsAny<CancellationToken>()),
            Times.Once);

        fileTenantClient.Verify(
            c => c.ProvisionBucketAsync(It.IsAny<long>(), It.IsAny<ProvisionTenantBucketRequest>(), It.IsAny<CancellationToken>()),
            Times.Never);

        userTenantClient.Verify(
            c => c.UpdateStatusAsync(
                10,
                It.Is<UpdateTenantStatusRequest>(r => r.FgsTenantStatusId == TenantStatusIds.Active),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    private static Mock<IUserTenantClient> CreateUserTenantClientMock()
    {
        var mock = new Mock<IUserTenantClient>();
        mock.Setup(c => c.GetTenantAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<TenantDto>.Ok(new TenantDto(10, "ACME", "Acme", TenantStatusIds.Pending, null)));
        mock.Setup(c => c.UpdateStatusAsync(10, It.IsAny<UpdateTenantStatusRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<object>.Ok(new object()));
        mock.Setup(c => c.UpdateStorageBucketAsync(10, It.IsAny<UpdateTenantStorageBucketRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<object>.Ok(new object()));
        return mock;
    }

    private static Mock<IUserCompanyClient> CreateUserCompanyClientMock()
    {
        var mock = new Mock<IUserCompanyClient>();
        mock.Setup(c => c.GetCompaniesAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<IReadOnlyList<TenantCompanyDto>>.Ok(
                [new TenantCompanyDto(1, 10, 1, Guid.NewGuid(), "ACME", "Acme", true)]));
        return mock;
    }

    private static Mock<IFileTenantClient> CreateFileTenantClientMock()
    {
        var mock = new Mock<IFileTenantClient>();
        mock.Setup(c => c.ProvisionBucketAsync(10, It.IsAny<ProvisionTenantBucketRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApiResponse<ProvisionTenantBucketResponse>.Ok(new ProvisionTenantBucketResponse("tenant-10-bucket")));
        return mock;
    }

    private static async Task<FgsSetupDbContext> CreateContextAsync()
    {
        var options = new DbContextOptionsBuilder<FgsSetupDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        var context = new FgsSetupDbContext(options, new DesignTimeTenantContextAccessor());
        await context.Database.EnsureCreatedAsync();
        return context;
    }
}
