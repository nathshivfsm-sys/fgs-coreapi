using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SalesActivityTypes;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SalesActivityTypes.Dtos;
using Fgs.Setup.Application.Features.SalesActivityTypes.Queries.GetFgsSalesActivityTypeById;
using Fgs.Setup.Application.Features.SalesActivityTypes.Queries.ListSalesActivityTypes;
using Moq;

namespace Fgs.Setup.Tests.SalesActivityTypes;

public sealed class FgsSalesActivityTypeQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = new FgsSalesActivityTypeDetailDto(1, 10, 20, "TEST", "ActivityTypeName", "Description", 5, false, true, true, true, true, DateTimeOffset.UtcNow, "seed", null, "seed");

        var readRepository = new Mock<IFgsSalesActivityTypeReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        var handler = new GetFgsSalesActivityTypeByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetFgsSalesActivityTypeByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<IFgsSalesActivityTypeReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((FgsSalesActivityTypeDetailDto?)null);

        var handler = new GetFgsSalesActivityTypeByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetFgsSalesActivityTypeByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<IFgsSalesActivityTypeReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(It.IsAny<SetupListQuery>(), It.IsAny<FgsSalesActivityTypeListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<FgsSalesActivityTypeSummaryDto>([], 1, 25, 0));

        var handler = new ListSalesActivityTypesQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListSalesActivityTypesQuery(new SetupListQuery(), new FgsSalesActivityTypeListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
    }
}
