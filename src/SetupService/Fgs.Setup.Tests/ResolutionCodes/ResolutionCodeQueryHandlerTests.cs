using Fgs.Contracts.Api;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.CatalogCrud;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.ResolutionCodes;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.ResolutionCodes.Dtos;
using Fgs.Setup.Application.Features.ResolutionCodes.Queries.GetResolutionCodeById;
using Fgs.Setup.Application.Features.ResolutionCodes.Queries.ListResolutionCodes;
using Moq;

namespace Fgs.Setup.Tests.ResolutionCodes;

public sealed class ResolutionCodeQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = new ResolutionCodeDetailDto(1, 1, "TEST", "ResolutionName", true, true);

        var readRepository = new Mock<IResolutionCodeReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new GetResolutionCodeByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetResolutionCodeByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
        readRepository.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<IResolutionCodeReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((ResolutionCodeDetailDto?)null);

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new GetResolutionCodeByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetResolutionCodeByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<IResolutionCodeReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(It.IsAny<SetupListQuery>(), It.IsAny<ResolutionCodeListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<ResolutionCodeSummaryDto>([], 1, 25, 0));

        var handler = new ListResolutionCodesQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListResolutionCodesQuery(new SetupListQuery(), new ResolutionCodeListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
    }
}
