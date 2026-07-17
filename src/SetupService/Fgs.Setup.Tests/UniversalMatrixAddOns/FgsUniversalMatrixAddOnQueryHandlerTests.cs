using Fgs.Contracts.Api;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalMatrixAddOns;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Dtos;
using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Queries.GetFgsUniversalMatrixAddOnById;
using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Queries.ListUniversalMatrixAddOns;
using Moq;

namespace Fgs.Setup.Tests.UniversalMatrixAddOns;

public sealed class FgsUniversalMatrixAddOnQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = new FgsUniversalMatrixAddOnDetailDto(1, 1, "Name", "UnitType", 10.5m, 5, true);

        var readRepository = new Mock<IFgsUniversalMatrixAddOnReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new GetFgsUniversalMatrixAddOnByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetFgsUniversalMatrixAddOnByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
        readRepository.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<IFgsUniversalMatrixAddOnReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((FgsUniversalMatrixAddOnDetailDto?)null);

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new GetFgsUniversalMatrixAddOnByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetFgsUniversalMatrixAddOnByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<IFgsUniversalMatrixAddOnReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(It.IsAny<SetupListQuery>(), It.IsAny<FgsUniversalMatrixAddOnListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<FgsUniversalMatrixAddOnSummaryDto>([], 1, 25, 0));

        var handler = new ListUniversalMatrixAddOnsQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListUniversalMatrixAddOnsQuery(new SetupListQuery(), new FgsUniversalMatrixAddOnListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
    }
}
