using Fgs.Contracts.Api;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.CatalogCrud;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.LeadStatuses;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.LeadStatuses.Dtos;
using Fgs.Setup.Application.Features.LeadStatuses.Queries.GetLeadStatusById;
using Fgs.Setup.Application.Features.LeadStatuses.Queries.ListLeadStatuses;
using Moq;

namespace Fgs.Setup.Tests.LeadStatuses;

public sealed class LeadStatusQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = new LeadStatusDetailDto(1, "TEST", "StatusName", "Description", 1, false, true);

        var readRepository = new Mock<ILeadStatusReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new GetLeadStatusByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetLeadStatusByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
        readRepository.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<ILeadStatusReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((LeadStatusDetailDto?)null);

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new GetLeadStatusByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetLeadStatusByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<ILeadStatusReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(It.IsAny<SetupListQuery>(), It.IsAny<LeadStatusListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<LeadStatusSummaryDto>([], 1, 25, 0));

        var handler = new ListLeadStatusesQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListLeadStatusesQuery(new SetupListQuery(), new LeadStatusListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
    }
}
