using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTaxDetails;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupTaxDetails.Dtos;
using Fgs.Setup.Application.Features.SetupTaxDetails.Queries.GetFgsSetupTaxDetailById;
using Fgs.Setup.Application.Features.SetupTaxDetails.Queries.ListSetupTaxDetails;
using Moq;

namespace Fgs.Setup.Tests.SetupTaxDetails;

public sealed class FgsSetupTaxDetailQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = new FgsSetupTaxDetailDetailDto(1, 10, 20, 1, 1, DateOnly.FromDateTime(DateTime.UtcNow), null, 10.5m, false, true, DateTimeOffset.UtcNow, "seed", null, "seed");

        var readRepository = new Mock<IFgsSetupTaxDetailReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        var handler = new GetFgsSetupTaxDetailByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetFgsSetupTaxDetailByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<IFgsSetupTaxDetailReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((FgsSetupTaxDetailDetailDto?)null);

        var handler = new GetFgsSetupTaxDetailByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetFgsSetupTaxDetailByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<IFgsSetupTaxDetailReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(It.IsAny<SetupListQuery>(), It.IsAny<FgsSetupTaxDetailListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<FgsSetupTaxDetailSummaryDto>([], 1, 25, 0));

        var handler = new ListSetupTaxDetailsQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListSetupTaxDetailsQuery(new SetupListQuery(), new FgsSetupTaxDetailListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
    }
}
