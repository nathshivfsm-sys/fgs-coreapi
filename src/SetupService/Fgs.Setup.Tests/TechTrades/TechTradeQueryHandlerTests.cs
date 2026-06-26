using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Abstractions.TechTrades;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.TechTrades.Dtos;
using Fgs.Setup.Application.Features.TechTrades.Queries.GetTechTradeById;
using Fgs.Setup.Application.Features.TechTrades.Queries.ListTechTrades;
using Moq;

namespace Fgs.Setup.Tests.TechTrades;

public sealed class TechTradeQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = new TechTradeDetailDto(1, "HVAC", "HVAC", null, 1, true);

        var readRepository = new Mock<ITechTradeReadRepository>();
        readRepository
            .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(detail);

        var handler = new GetTechTradeByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetTechTradeByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
        response.Data!.TradeCode.Should().Be("HVAC");
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<ITechTradeReadRepository>();
        readRepository
            .Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TechTradeDetailDto?)null);

        var handler = new GetTechTradeByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetTechTradeByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var items = new List<TechTradeSummaryDto>
        {
            new(1, "HVAC", "HVAC", 1, true)
        };

        var readRepository = new Mock<ITechTradeReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(It.IsAny<SetupListQuery>(), It.IsAny<TechTradeListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<TechTradeSummaryDto>(items, 1, 25, 1));

        var handler = new ListTechTradesQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListTechTradesQuery(new SetupListQuery(), new TechTradeListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Items.Should().HaveCount(1);
        response.Data.TotalCount.Should().Be(1);
    }
}
