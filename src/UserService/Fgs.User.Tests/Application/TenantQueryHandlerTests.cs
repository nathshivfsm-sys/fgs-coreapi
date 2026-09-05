using Fgs.Contracts.Clients;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.Security.Abstractions;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.Tenants.Queries.GetTenant;
using Fgs.User.Application.Features.Tenants.Queries.ListTenants;
using Fgs.User.Domain.Entities;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class TenantQueryHandlerTests
{
    [Fact]
    public async Task GetTenant_WhenNotFound_ReturnsNotFound()
    {
        var cache = new Mock<ICacheService>();
        cache.Setup(c => c.GetAsync<TenantDto>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TenantDto?)null);

        var repository = new Mock<IUserReadRepository<FgsTenant>>();
        repository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((FgsTenant?)null);

        var handler = new GetTenantQueryHandler(repository.Object, cache.Object, UnauthenticatedContext().Object);
        var response = await handler.Handle(new GetTenantQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(404);
        response.Errors.Should().Contain("Tenant not found.");
    }

    [Fact]
    public async Task GetTenant_WhenFound_CachesAndReturns()
    {
        var cache = new Mock<ICacheService>();
        cache.Setup(c => c.GetAsync<TenantDto>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TenantDto?)null);

        var repository = new Mock<IUserReadRepository<FgsTenant>>();
        repository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FgsTenant
            {
                Id = 1,
                TenantCode = "ACME",
                Name = "Acme",
                FgsTenantStatusId = 1,
                StorageBucketName = "bucket",
                IsActive = true
            });

        var handler = new GetTenantQueryHandler(repository.Object, cache.Object, UnauthenticatedContext().Object);
        var response = await handler.Handle(new GetTenantQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Code.Should().Be("ACME");
        cache.Verify(
            c => c.SetAsync(It.IsAny<string>(), It.IsAny<TenantDto>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task ListTenants_ReturnsPagedResults()
    {
        var tenants = new List<FgsTenant>
        {
            new()
            {
                Id = 1,
                TenantGuid = Guid.NewGuid(),
                TenantCode = "ACME",
                Name = "Acme",
                FgsTenantStatusId = 1,
                IsActive = true
            }
        };

        var repository = new Mock<IUserReadRepository<FgsTenant>>();
        repository.Setup(r => r.QueryListAsync<FgsTenant>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(tenants);
        repository.Setup(r => r.QueryFirstAsync<int>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var handler = new ListTenantsQueryHandler(repository.Object, UnauthenticatedContext().Object);
        var response = await handler.Handle(new ListTenantsQuery(new IdentityListQuery()), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Items.Should().ContainSingle(t => t.Code == "ACME");
        response.Data.TotalCount.Should().Be(1);
    }

    [Fact]
    public async Task ListTenants_WhenAuthenticated_ScopesToTenant()
    {
        var repository = new Mock<IUserReadRepository<FgsTenant>>();
        repository.Setup(r => r.QueryListAsync<FgsTenant>(
                It.Is<string>(sql => sql.Contains("@ScopedTenantId")),
                It.IsAny<object>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);
        repository.Setup(r => r.QueryFirstAsync<int>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        var userContext = new Mock<IFgsUserContext>();
        userContext.SetupGet(c => c.IsAuthenticated).Returns(true);
        userContext.SetupGet(c => c.TenantId).Returns(5L);

        var handler = new ListTenantsQueryHandler(repository.Object, userContext.Object);
        var response = await handler.Handle(new ListTenantsQuery(new IdentityListQuery()), CancellationToken.None);

        response.Success.Should().BeTrue();
        repository.Verify(
            r => r.QueryListAsync<FgsTenant>(
                It.Is<string>(sql => sql.Contains("@ScopedTenantId")),
                It.IsAny<object>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private static Mock<IFgsUserContext> UnauthenticatedContext()
    {
        var userContext = new Mock<IFgsUserContext>();
        userContext.SetupGet(c => c.IsAuthenticated).Returns(false);
        return userContext;
    }
}
