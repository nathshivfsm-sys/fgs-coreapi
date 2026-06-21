using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupPostalCodes;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPostalCodes.Dtos;
using Fgs.Setup.Application.Features.SetupPostalCodes.Queries.GetFgsSetupPostalCodeById;
using Fgs.Setup.Application.Features.SetupPostalCodes.Queries.ListSetupPostalCodes;
using Moq;

namespace Fgs.Setup.Tests.SetupPostalCodes;

public sealed class FgsSetupPostalCodeQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = new FgsSetupPostalCodeDetailDto(1, 10, 20, "PostalCode value", null, null, true, DateTimeOffset.UtcNow, "seed", null, "seed");

        var readRepository = new Mock<IFgsSetupPostalCodeReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        var handler = new GetFgsSetupPostalCodeByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetFgsSetupPostalCodeByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<IFgsSetupPostalCodeReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((FgsSetupPostalCodeDetailDto?)null);

        var handler = new GetFgsSetupPostalCodeByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetFgsSetupPostalCodeByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<IFgsSetupPostalCodeReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(It.IsAny<SetupListQuery>(), It.IsAny<FgsSetupPostalCodeListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<FgsSetupPostalCodeSummaryDto>([], 1, 25, 0));

        var handler = new ListSetupPostalCodesQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListSetupPostalCodesQuery(new SetupListQuery(), new FgsSetupPostalCodeListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
    }
}
