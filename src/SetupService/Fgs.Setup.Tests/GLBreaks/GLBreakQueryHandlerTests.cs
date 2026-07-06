using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Abstractions.GLBreaks;
using Fgs.Setup.Application.Common.Locations;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.GLBreaks.Dtos;
using Fgs.Setup.Application.Features.GLBreaks.Queries.GetGLBreakById;
using Fgs.Setup.Application.Features.GLBreaks.Queries.ListGLBreaks;
using Moq;

namespace Fgs.Setup.Tests.GLBreaks;

public sealed class GLBreakQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsAddressAndTrades()
    {
        var address = new GLBreakAddressDetailDto(
            Guid.NewGuid(),
            "123 Main St",
            null,
            null,
            null,
            "Dallas",
            "TX",
            "US",
            "75201",
            null,
            null,
            null);

        var trades = new List<GLBreakTradeDto>
        {
            new(1, "HVAC")
        };

        var detail = new GLBreakDetailDto(
            5, "HVAC", "HVAC Division", null, 1, null, address, trades, true);

        var readRepository = new Mock<IGLBreakReadRepository>();
        readRepository
            .Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(detail);

        var handler = new GetGLBreakByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetGLBreakByIdQuery(5), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Address!.City.Should().Be("Dallas");
        response.Data.Trades[0].TradeCode.Should().Be("HVAC");
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<IGLBreakReadRepository>();
        readRepository
            .Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync((GLBreakDetailDto?)null);

        var handler = new GetGLBreakByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetGLBreakByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var items = new List<GLBreakSummaryDto>
        {
            new(1, "HVAC", "HVAC Division", null, 1, null, true)
        };

        var readRepository = new Mock<IGLBreakReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(It.IsAny<SetupListQuery>(), It.IsAny<GLBreakListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<GLBreakSummaryDto>(items, 1, 25, 1));

        var handler = new ListGLBreaksQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListGLBreaksQuery(new SetupListQuery(), new GLBreakListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Items.Should().HaveCount(1);
    }
}
