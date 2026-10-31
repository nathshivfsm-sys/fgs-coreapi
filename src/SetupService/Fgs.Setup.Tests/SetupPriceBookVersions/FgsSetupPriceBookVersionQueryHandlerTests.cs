using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Abstractions.SetupPriceBookVersions;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPriceBookVersions.Dtos;
using Fgs.Setup.Application.Features.SetupPriceBookVersions.Queries.GetFgsSetupPriceBookVersionById;
using Fgs.Setup.Application.Features.SetupPriceBookVersions.Queries.ListSetupPriceBookVersions;
using Fgs.Setup.Application.Features.SetupPriceBookVersions.Queries.LookupSetupPriceBookVersions;
using Moq;

namespace Fgs.Setup.Tests.SetupPriceBookVersions;

public sealed class FgsSetupPriceBookVersionQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = new FgsSetupPriceBookVersionDetailDto(
            1, "2026-Q1", new DateOnly(2026, 1, 1), new DateOnly(2026, 3, 31), true);

        var readRepository = new Mock<IFgsSetupPriceBookVersionReadRepository>();
        readRepository
            .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(detail);

        var handler = new GetFgsSetupPriceBookVersionByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetFgsSetupPriceBookVersionByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
        response.Data!.Version.Should().Be("2026-Q1");
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<IFgsSetupPriceBookVersionReadRepository>();
        readRepository
            .Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync((FgsSetupPriceBookVersionDetailDto?)null);

        var handler = new GetFgsSetupPriceBookVersionByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetFgsSetupPriceBookVersionByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var items = new List<FgsSetupPriceBookVersionSummaryDto>
        {
            new(1, "2026-Q1", new DateOnly(2026, 1, 1), null, true)
        };

        var readRepository = new Mock<IFgsSetupPriceBookVersionReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(
                It.IsAny<SetupListQuery>(),
                It.IsAny<FgsSetupPriceBookVersionListFilters>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<FgsSetupPriceBookVersionSummaryDto>(items, 1, 25, 1));

        var handler = new ListSetupPriceBookVersionsQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListSetupPriceBookVersionsQuery(new SetupListQuery(), new FgsSetupPriceBookVersionListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Items.Should().HaveCount(1);
        response.Data.TotalCount.Should().Be(1);
    }

    [Fact]
    public async Task Lookup_ReturnsItems()
    {
        var items = new List<FgsSetupPriceBookVersionLookupDto>
        {
            new(1, "2026-Q1", new DateOnly(2026, 1, 1), null)
        };

        var readRepository = new Mock<IFgsSetupPriceBookVersionReadRepository>();
        readRepository
            .Setup(r => r.LookupAsync(true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(items);

        var handler = new LookupSetupPriceBookVersionsQueryHandler(readRepository.Object);
        var response = await handler.Handle(new LookupSetupPriceBookVersionsQuery(), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().HaveCount(1);
    }
}
