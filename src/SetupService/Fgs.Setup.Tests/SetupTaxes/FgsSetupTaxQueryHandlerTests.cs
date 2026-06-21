using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupTaxes;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupTaxes.Dtos;
using Fgs.Setup.Application.Features.SetupTaxes.Queries.GetFgsSetupTaxById;
using Fgs.Setup.Application.Features.SetupTaxes.Queries.ListSetupTaxes;
using Moq;

namespace Fgs.Setup.Tests.SetupTaxes;

public sealed class FgsSetupTaxQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = new FgsSetupTaxDetailDto(1, 10, 20, "TEST", "Name value", false, "ExternalSystemId", "SyncToken", false, "Description value", true, DateTimeOffset.UtcNow, "seed", null, "seed");

        var readRepository = new Mock<IFgsSetupTaxReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        var handler = new GetFgsSetupTaxByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetFgsSetupTaxByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<IFgsSetupTaxReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((FgsSetupTaxDetailDto?)null);

        var handler = new GetFgsSetupTaxByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetFgsSetupTaxByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<IFgsSetupTaxReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(It.IsAny<SetupListQuery>(), It.IsAny<FgsSetupTaxListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<FgsSetupTaxSummaryDto>([], 1, 25, 0));

        var handler = new ListSetupTaxesQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListSetupTaxesQuery(new SetupListQuery(), new FgsSetupTaxListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
    }
}
