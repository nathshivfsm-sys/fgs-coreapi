using Fgs.Contracts.Api;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Sources;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.Sources.Dtos;
using Fgs.Setup.Application.Features.Sources.Queries.GetSourceById;
using Fgs.Setup.Application.Features.Sources.Queries.ListSources;
using Moq;

namespace Fgs.Setup.Tests.Sources;

public sealed class SourceQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = new SourceDetailDto(1, "TEST", "SourceName", "Description", true);

        var readRepository = new Mock<ISourceReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new GetSourceByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetSourceByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
        readRepository.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<ISourceReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((SourceDetailDto?)null);

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new GetSourceByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetSourceByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<ISourceReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(It.IsAny<SetupListQuery>(), It.IsAny<SourceListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<SourcesummaryDto>([], 1, 25, 0));

        var handler = new ListSourcesQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListSourcesQuery(new SetupListQuery(), new SourceListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
    }
}
