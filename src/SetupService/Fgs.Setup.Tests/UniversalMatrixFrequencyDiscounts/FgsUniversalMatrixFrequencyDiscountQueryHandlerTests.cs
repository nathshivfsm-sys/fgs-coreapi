using Fgs.Contracts.Api;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalMatrixFrequencyDiscounts;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Dtos;
using Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Queries.GetFgsUniversalMatrixFrequencyDiscountById;
using Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Queries.ListUniversalMatrixFrequencyDiscounts;
using Moq;

namespace Fgs.Setup.Tests.UniversalMatrixFrequencyDiscounts;

public sealed class FgsUniversalMatrixFrequencyDiscountQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = new FgsUniversalMatrixFrequencyDiscountDetailDto(1, 1, "Name", 10.5m, 5, true);

        var readRepository = new Mock<IFgsUniversalMatrixFrequencyDiscountReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new GetFgsUniversalMatrixFrequencyDiscountByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetFgsUniversalMatrixFrequencyDiscountByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
        readRepository.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<IFgsUniversalMatrixFrequencyDiscountReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((FgsUniversalMatrixFrequencyDiscountDetailDto?)null);

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new GetFgsUniversalMatrixFrequencyDiscountByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetFgsUniversalMatrixFrequencyDiscountByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<IFgsUniversalMatrixFrequencyDiscountReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(It.IsAny<SetupListQuery>(), It.IsAny<FgsUniversalMatrixFrequencyDiscountListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<FgsUniversalMatrixFrequencyDiscountSummaryDto>([], 1, 25, 0));

        var handler = new ListUniversalMatrixFrequencyDiscountsQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListUniversalMatrixFrequencyDiscountsQuery(new SetupListQuery(), new FgsUniversalMatrixFrequencyDiscountListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
    }
}
