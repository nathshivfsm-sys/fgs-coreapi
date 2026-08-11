using Fgs.Foundation.Caching.Abstractions;
using Fgs.Security.Abstractions;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Abstractions.ServiceAccountsSetups;
using Fgs.User.Application.Abstractions.ServiceSetups;
using Fgs.User.Application.Features.Companies.Dtos;
using Fgs.User.Application.Features.Companies.Queries.GetCompanyAggregate;
using Fgs.User.Application.Features.ServiceAccountsSetups.Dtos;
using Fgs.User.Application.Features.ServiceSetups.Dtos;
using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class GetCompanyAggregateQueryHandlerTests
{
    [Fact]
    public async Task Handle_WhenCached_DoesNotCallRepositories()
    {
        var aggregate = CreateAggregate();
        var cache = new Mock<ICacheService>();
        cache.Setup(c => c.GetAsync<CompanyAggregateDto>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(aggregate);

        var tenantRepo = new Mock<IUserReadRepository<FgsTenant>>();
        var companyQuery = new Mock<ICompanyDetailsReadQuery>();
        var serviceSetupRepo = new Mock<IFgsTenantServiceSetupReadRepository>();
        var serviceAccountsRepo = new Mock<IFgsTenantServiceAccountsSetupReadRepository>();
        var userContext = UnauthenticatedContext();

        var handler = new GetCompanyAggregateQueryHandler(
            tenantRepo.Object,
            companyQuery.Object,
            serviceSetupRepo.Object,
            serviceAccountsRepo.Object,
            cache.Object,
            userContext.Object);

        var response = await handler.Handle(new GetCompanyAggregateQuery(10, 1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().BeEquivalentTo(aggregate);
        tenantRepo.Verify(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Never);
        companyQuery.Verify(
            q => q.GetAsync(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenTenantMissing_ReturnsNotFound()
    {
        var cache = new Mock<ICacheService>();
        cache.Setup(c => c.GetAsync<CompanyAggregateDto>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((CompanyAggregateDto?)null);

        var tenantRepo = new Mock<IUserReadRepository<FgsTenant>>();
        tenantRepo.Setup(r => r.GetByIdAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync((FgsTenant?)null);

        var handler = new GetCompanyAggregateQueryHandler(
            tenantRepo.Object,
            new Mock<ICompanyDetailsReadQuery>().Object,
            new Mock<IFgsTenantServiceSetupReadRepository>().Object,
            new Mock<IFgsTenantServiceAccountsSetupReadRepository>().Object,
            cache.Object,
            UnauthenticatedContext().Object);

        var response = await handler.Handle(new GetCompanyAggregateQuery(10, 1), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(404);
        response.Errors.Should().Contain("Tenant not found.");
    }

    [Fact]
    public async Task Handle_WhenCompanyMissing_ReturnsNotFound()
    {
        var cache = new Mock<ICacheService>();
        cache.Setup(c => c.GetAsync<CompanyAggregateDto>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((CompanyAggregateDto?)null);

        var tenantRepo = new Mock<IUserReadRepository<FgsTenant>>();
        tenantRepo.Setup(r => r.GetByIdAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FgsTenant
            {
                Id = 10,
                TenantGuid = Guid.NewGuid(),
                TenantCode = "ACME",
                Name = "Acme",
                FgsTenantStatusId = 3,
                IsActive = true
            });

        var companyQuery = new Mock<ICompanyDetailsReadQuery>();
        companyQuery.Setup(q => q.GetAsync(10, 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((CompanyDetailDto?)null);

        var handler = new GetCompanyAggregateQueryHandler(
            tenantRepo.Object,
            companyQuery.Object,
            new Mock<IFgsTenantServiceSetupReadRepository>().Object,
            new Mock<IFgsTenantServiceAccountsSetupReadRepository>().Object,
            cache.Object,
            UnauthenticatedContext().Object);

        var response = await handler.Handle(new GetCompanyAggregateQuery(10, 1), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(404);
        response.Errors.Should().Contain("Company not found.");
    }

    [Fact]
    public async Task Handle_WhenFound_ReturnsAggregateAndCaches()
    {
        var cache = new Mock<ICacheService>();
        cache.Setup(c => c.GetAsync<CompanyAggregateDto>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((CompanyAggregateDto?)null);

        var tenantGuid = Guid.NewGuid();
        var companyGuid = Guid.NewGuid();
        var tenantRepo = new Mock<IUserReadRepository<FgsTenant>>();
        tenantRepo.Setup(r => r.GetByIdAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FgsTenant
            {
                Id = 10,
                TenantGuid = tenantGuid,
                TenantCode = "ACME",
                Name = "Acme",
                FgsTenantStatusId = 3,
                StorageBucketName = "bucket",
                IsActive = true
            });

        var company = new CompanyDetailDto(
            5,
            10,
            1,
            companyGuid,
            "ACME",
            "Acme Co",
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            true,
            null,
            null);
        var companyQuery = new Mock<ICompanyDetailsReadQuery>();
        companyQuery.Setup(q => q.GetAsync(10, 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(company);

        var serviceSetup = new FgsTenantServiceSetupDetailDto(
            10,
            1,
            TimeCardOption.None,
            null,
            false,
            false,
            false,
            false,
            false,
            false,
            null,
            null,
            null,
            null,
            null,
            "ARRIVE",
            false,
            false,
            100,
            100,
            100,
            100,
            null,
            null,
            null,
            null,
            null,
            true);
        var serviceSetupRepo = new Mock<IFgsTenantServiceSetupReadRepository>();
        serviceSetupRepo.Setup(r => r.GetByTenantCompanyAsync(10, 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(serviceSetup);

        var serviceAccounts = new FgsTenantServiceAccountsSetupDetailDto(
            10, 1, null, null, null, null, null, null, null, null, null, null, true);
        var serviceAccountsRepo = new Mock<IFgsTenantServiceAccountsSetupReadRepository>();
        serviceAccountsRepo.Setup(r => r.GetByTenantCompanyAsync(10, 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(serviceAccounts);

        var handler = new GetCompanyAggregateQueryHandler(
            tenantRepo.Object,
            companyQuery.Object,
            serviceSetupRepo.Object,
            serviceAccountsRepo.Object,
            cache.Object,
            UnauthenticatedContext().Object);

        var response = await handler.Handle(new GetCompanyAggregateQuery(10, 1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Tenant.Id.Should().Be(10);
        response.Data.Tenant.Code.Should().Be("ACME");
        response.Data.Company.Should().BeEquivalentTo(company);
        response.Data.ServiceSetup.Should().BeEquivalentTo(serviceSetup);
        response.Data.ServiceAccountsSetup.Should().BeEquivalentTo(serviceAccounts);
        cache.Verify(
            c => c.SetAsync(
                It.IsAny<string>(),
                It.IsAny<CompanyAggregateDto>(),
                It.IsAny<TimeSpan?>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private static Mock<IFgsUserContext> UnauthenticatedContext()
    {
        var userContext = new Mock<IFgsUserContext>();
        userContext.SetupGet(c => c.IsAuthenticated).Returns(false);
        return userContext;
    }

    private static CompanyAggregateDto CreateAggregate() =>
        new(
            new(
                10,
                Guid.NewGuid(),
                "ACME",
                "Acme",
                null,
                null,
                null,
                null,
                null,
                null,
                3,
                null,
                true),
            new(
                5,
                10,
                1,
                Guid.NewGuid(),
                "ACME",
                "Acme Co",
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                true,
                null,
                null),
            null,
            null);
}
