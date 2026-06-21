using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupPaymentMethods;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPaymentMethods.Dtos;
using Fgs.Setup.Application.Features.SetupPaymentMethods.Queries.GetFgsSetupPaymentMethodById;
using Fgs.Setup.Application.Features.SetupPaymentMethods.Queries.ListSetupPaymentMethods;
using Moq;

namespace Fgs.Setup.Tests.SetupPaymentMethods;

public sealed class FgsSetupPaymentMethodQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = new FgsSetupPaymentMethodDetailDto(1, 10, 20, "DisplayName value", 1, true, true, true, DateTimeOffset.UtcNow, "seed", null, "seed");

        var readRepository = new Mock<IFgsSetupPaymentMethodReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        var handler = new GetFgsSetupPaymentMethodByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetFgsSetupPaymentMethodByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<IFgsSetupPaymentMethodReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((FgsSetupPaymentMethodDetailDto?)null);

        var handler = new GetFgsSetupPaymentMethodByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetFgsSetupPaymentMethodByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<IFgsSetupPaymentMethodReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(It.IsAny<SetupListQuery>(), It.IsAny<FgsSetupPaymentMethodListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<FgsSetupPaymentMethodSummaryDto>([], 1, 25, 0));

        var handler = new ListSetupPaymentMethodsQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListSetupPaymentMethodsQuery(new SetupListQuery(), new FgsSetupPaymentMethodListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
    }
}
