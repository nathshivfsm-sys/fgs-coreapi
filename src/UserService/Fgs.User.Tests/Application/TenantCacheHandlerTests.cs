using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy.Constants;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Features.Tenants.Commands.UpdateTenantStatus;
using Fgs.User.Application.Features.Tenants.Queries.GetTenant;
using Fgs.Security.Abstractions;
using Fgs.User.Domain.Entities;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class TenantCacheHandlerTests
{
    [Fact]
    public async Task GetTenant_WhenCached_DoesNotCallRepository()
    {
        var tenantDto = new TenantDto(1, "TENANT", "Tenant Name", 1, "bucket");
        var cache = new Mock<ICacheService>();
        cache.Setup(c => c.GetAsync<TenantDto>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(tenantDto);

        var repository = new Mock<IUserReadRepository<FgsTenant>>();
        var userContext = new Mock<IFgsUserContext>();
        userContext.SetupGet(c => c.IsAuthenticated).Returns(false);
        var handler = new GetTenantQueryHandler(repository.Object, cache.Object, userContext.Object);

        var response = await handler.Handle(new GetTenantQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().BeEquivalentTo(tenantDto);
        repository.Verify(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateTenantStatus_InvalidatesTenantCache()
    {
        var tenant = new FgsTenant
        {
            Id = 5,
            TenantCode = "CODE",
            Name = "Tenant",
            FgsTenantStatusId = 1,
            IsActive = true
        };

        var repository = new Mock<IUserWriteRepository<FgsTenant>>();
        repository.Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(tenant);

        var unitOfWork = new Mock<Fgs.Persistence.Abstractions.IUnitOfWork>();
        unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var cache = new Mock<ICacheService>();
        var handler = new UpdateTenantStatusCommandHandler(repository.Object, unitOfWork.Object, cache.Object);

        var response = await handler.Handle(
            new UpdateTenantStatusCommand(5, new UpdateTenantStatusRequest(2)),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        cache.Verify(
            c => c.RemoveAsync(
                CacheKeys.Build(5, TenantScopeConstants.PlatformCompanyId, "tenant", "5"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
