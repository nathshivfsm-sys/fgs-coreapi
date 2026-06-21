using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SalesDispositionReasons;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SalesDispositionReasons.Dtos;
using Fgs.Setup.Application.Features.SalesDispositionReasons.Queries.GetFgsSalesDispositionReasonById;
using Fgs.Setup.Application.Features.SalesDispositionReasons.Queries.ListSalesDispositionReasons;
using Moq;

namespace Fgs.Setup.Tests.SalesDispositionReasons;

public sealed class FgsSalesDispositionReasonQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = new FgsSalesDispositionReasonDetailDto(1, 10, 20, "TEST", "DispositionReasonName", "Description", 5, false, true, false, false, true, true, true, DateTimeOffset.UtcNow, "seed", null, "seed");

        var readRepository = new Mock<IFgsSalesDispositionReasonReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        var handler = new GetFgsSalesDispositionReasonByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetFgsSalesDispositionReasonByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<IFgsSalesDispositionReasonReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((FgsSalesDispositionReasonDetailDto?)null);

        var handler = new GetFgsSalesDispositionReasonByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetFgsSalesDispositionReasonByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<IFgsSalesDispositionReasonReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(It.IsAny<SetupListQuery>(), It.IsAny<FgsSalesDispositionReasonListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<FgsSalesDispositionReasonSummaryDto>([], 1, 25, 0));

        var handler = new ListSalesDispositionReasonsQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListSalesDispositionReasonsQuery(new SetupListQuery(), new FgsSalesDispositionReasonListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
    }
}
