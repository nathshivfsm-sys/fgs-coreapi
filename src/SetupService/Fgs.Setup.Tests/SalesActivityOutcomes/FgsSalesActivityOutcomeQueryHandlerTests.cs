using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SalesActivityOutcomes;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SalesActivityOutcomes.Dtos;
using Fgs.Setup.Application.Features.SalesActivityOutcomes.Queries.GetFgsSalesActivityOutcomeById;
using Fgs.Setup.Application.Features.SalesActivityOutcomes.Queries.ListSalesActivityOutcomes;
using Moq;

namespace Fgs.Setup.Tests.SalesActivityOutcomes;

public sealed class FgsSalesActivityOutcomeQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = new FgsSalesActivityOutcomeDetailDto(1, 10, 20, "TEST", "OutcomeName", "Description", 5, false, true, true, null, false, false, true, true, DateTimeOffset.UtcNow, "seed", null, "seed");

        var readRepository = new Mock<IFgsSalesActivityOutcomeReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        var handler = new GetFgsSalesActivityOutcomeByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetFgsSalesActivityOutcomeByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<IFgsSalesActivityOutcomeReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((FgsSalesActivityOutcomeDetailDto?)null);

        var handler = new GetFgsSalesActivityOutcomeByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetFgsSalesActivityOutcomeByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<IFgsSalesActivityOutcomeReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(It.IsAny<SetupListQuery>(), It.IsAny<FgsSalesActivityOutcomeListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<FgsSalesActivityOutcomeSummaryDto>([], 1, 25, 0));

        var handler = new ListSalesActivityOutcomesQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListSalesActivityOutcomesQuery(new SetupListQuery(), new FgsSalesActivityOutcomeListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
    }
}
