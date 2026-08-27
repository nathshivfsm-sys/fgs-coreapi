using Fgs.Contracts.Clients;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Security.Abstractions;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Features.Companies.Dtos;
using Fgs.User.Application.Features.Companies.Queries.GetCompany;
using Fgs.User.Application.Features.Companies.Queries.ListCompanies;
using Fgs.User.Domain.Entities;
using Moq;

namespace Fgs.User.Tests.Application;

public sealed class CompanyQueryHandlerTests
{
    private static readonly CompanyDetailDto Company = new(
        5, 10, 1, Guid.NewGuid(), "ACME", "Acme Co", null, null, null, null, null, null, null, true, null, null);

    [Fact]
    public async Task GetCompany_WhenCached_DoesNotCallRepository()
    {
        var cache = new Mock<ICacheService>();
        cache.Setup(c => c.GetAsync<CompanyDetailDto>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Company);

        var details = new Mock<ICompanyDetailsReadQuery>();
        var handler = new GetCompanyQueryHandler(details.Object, cache.Object, UnauthenticatedContext().Object);
        var response = await handler.Handle(new GetCompanyQuery(10, 1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Name.Should().Be("Acme Co");
        details.Verify(q => q.GetAsync(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetCompany_WhenMissing_ReturnsNotFound()
    {
        var cache = new Mock<ICacheService>();
        cache.Setup(c => c.GetAsync<CompanyDetailDto>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((CompanyDetailDto?)null);

        var details = new Mock<ICompanyDetailsReadQuery>();
        details.Setup(q => q.GetAsync(10, 1, It.IsAny<CancellationToken>())).ReturnsAsync((CompanyDetailDto?)null);

        var handler = new GetCompanyQueryHandler(details.Object, cache.Object, UnauthenticatedContext().Object);
        var response = await handler.Handle(new GetCompanyQuery(10, 1), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task GetCompany_WhenFound_CachesAndReturns()
    {
        var cache = new Mock<ICacheService>();
        cache.Setup(c => c.GetAsync<CompanyDetailDto>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((CompanyDetailDto?)null);

        var details = new Mock<ICompanyDetailsReadQuery>();
        details.Setup(q => q.GetAsync(10, 1, It.IsAny<CancellationToken>())).ReturnsAsync(Company);

        var handler = new GetCompanyQueryHandler(details.Object, cache.Object, UnauthenticatedContext().Object);
        var response = await handler.Handle(new GetCompanyQuery(10, 1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Code.Should().Be("ACME");
        cache.Verify(
            c => c.SetAsync(It.IsAny<string>(), Company, It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task ListCompanies_ReturnsCompanies()
    {
        var companies = new List<FgsTenantCompany>
        {
            new()
            {
                Id = 5,
                TenantId = 10,
                CompanyNumber = 1,
                CompanyGuid = Guid.NewGuid(),
                Code = "ACME",
                Name = "Acme Co",
                IsActive = true
            }
        };

        var repository = new Mock<IUserReadRepository<FgsTenantCompany>>();
        repository.Setup(r => r.ListAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(companies);

        var cache = new Mock<ICacheService>();
        cache.Setup(c => c.GetOrSetAsync(
                It.IsAny<string>(),
                It.IsAny<Func<Task<IReadOnlyList<TenantCompanyDto>>>>(),
                It.IsAny<TimeSpan?>(),
                It.IsAny<CancellationToken>()))
            .Returns<string, Func<Task<IReadOnlyList<TenantCompanyDto>>>, TimeSpan?, CancellationToken>(
                async (_, factory, _, _) => await factory());

        var handler = new ListCompaniesQueryHandler(repository.Object, cache.Object, UnauthenticatedContext().Object);
        var response = await handler.Handle(new ListCompaniesQuery(10), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().ContainSingle(c => c.Code == "ACME");
    }

    private static Mock<IFgsUserContext> UnauthenticatedContext()
    {
        var userContext = new Mock<IFgsUserContext>();
        userContext.SetupGet(c => c.IsAuthenticated).Returns(false);
        return userContext;
    }
}
