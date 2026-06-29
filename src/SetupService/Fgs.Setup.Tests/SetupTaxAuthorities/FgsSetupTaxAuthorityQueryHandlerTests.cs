using Fgs.Contracts.Api;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupTaxAuthorities;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Dtos;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Queries.GetFgsSetupTaxAuthorityById;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Queries.ListSetupTaxAuthorities;
using Moq;

namespace Fgs.Setup.Tests.SetupTaxAuthorities;

public sealed class FgsSetupTaxAuthorityQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = new FgsSetupTaxAuthorityDetailDto(1, "TEST", "Name value", "TEST", false, 10.5m, "Description value", 0, true);

        var readRepository = new Mock<IFgsSetupTaxAuthorityReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new GetFgsSetupTaxAuthorityByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetFgsSetupTaxAuthorityByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
        readRepository.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<IFgsSetupTaxAuthorityReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((FgsSetupTaxAuthorityDetailDto?)null);

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new GetFgsSetupTaxAuthorityByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetFgsSetupTaxAuthorityByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<IFgsSetupTaxAuthorityReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(It.IsAny<SetupListQuery>(), It.IsAny<FgsSetupTaxAuthorityListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<FgsSetupTaxAuthoritySummaryDto>([], 1, 25, 0));

        var handler = new ListSetupTaxAuthoritiesQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListSetupTaxAuthoritiesQuery(new SetupListQuery(), new FgsSetupTaxAuthorityListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
    }
}
