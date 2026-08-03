using Fgs.Contracts.Api;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalPricingServices;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.UniversalPricingServices.Dtos;
using Fgs.Setup.Application.Features.UniversalPricingServices.Queries.GetFgsUniversalPricingServiceById;
using Fgs.Setup.Application.Features.UniversalPricingServices.Queries.ListUniversalPricingServices;
using Moq;

namespace Fgs.Setup.Tests.UniversalPricingServices;

public sealed class FgsUniversalPricingServiceQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = new FgsUniversalPricingServiceDetailDto(1, "TEST", 5, true);

        var readRepository = new Mock<IFgsUniversalPricingServiceReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new GetFgsUniversalPricingServiceByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetFgsUniversalPricingServiceByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
        response.Data!.UniversalPricingServiceCode.Should().Be("TEST");
        readRepository.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<IFgsUniversalPricingServiceReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((FgsUniversalPricingServiceDetailDto?)null);

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new GetFgsUniversalPricingServiceByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetFgsUniversalPricingServiceByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<IFgsUniversalPricingServiceReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(It.IsAny<SetupListQuery>(), It.IsAny<FgsUniversalPricingServiceListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<FgsUniversalPricingServiceSummaryDto>([], 1, 25, 0));

        var handler = new ListUniversalPricingServicesQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListUniversalPricingServicesQuery(new SetupListQuery(), new FgsUniversalPricingServiceListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
    }
}
