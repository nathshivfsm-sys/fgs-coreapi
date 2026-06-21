using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.LeadSources;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.LeadSources.Dtos;
using Fgs.Setup.Application.Features.LeadSources.Queries.GetLeadSourceById;
using Fgs.Setup.Application.Features.LeadSources.Queries.ListLeadSources;
using Moq;

namespace Fgs.Setup.Tests.LeadSources;

public sealed class LeadSourceQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = new LeadSourceDetailDto(1, 10, 20, "TEST", "SourceName value", "Description value", true, DateTimeOffset.UtcNow, "seed", null, null);

        var readRepository = new Mock<ILeadSourceReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        var handler = new GetLeadSourceByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetLeadSourceByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<ILeadSourceReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((LeadSourceDetailDto?)null);

        var handler = new GetLeadSourceByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetLeadSourceByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<ILeadSourceReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(It.IsAny<SetupListQuery>(), It.IsAny<LeadSourceListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<LeadSourceSummaryDto>([], 1, 25, 0));

        var handler = new ListLeadSourcesQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListLeadSourcesQuery(new SetupListQuery(), new LeadSourceListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
    }
}
