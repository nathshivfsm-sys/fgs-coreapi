using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupZones;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupZones.Dtos;
using Fgs.Setup.Application.Features.SetupZones.Queries.GetFgsSetupZoneById;
using Fgs.Setup.Application.Features.SetupZones.Queries.ListSetupZones;
using Moq;

namespace Fgs.Setup.Tests.SetupZones;

public sealed class FgsSetupZoneQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = new FgsSetupZoneDetailDto(1, 10, 20, "TEST", "Name value", "Description value", true, DateTimeOffset.UtcNow, "seed", null, "seed");

        var readRepository = new Mock<IFgsSetupZoneReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        var handler = new GetFgsSetupZoneByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetFgsSetupZoneByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<IFgsSetupZoneReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((FgsSetupZoneDetailDto?)null);

        var handler = new GetFgsSetupZoneByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetFgsSetupZoneByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<IFgsSetupZoneReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(It.IsAny<SetupListQuery>(), It.IsAny<FgsSetupZoneListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<FgsSetupZoneSummaryDto>([], 1, 25, 0));

        var handler = new ListSetupZonesQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListSetupZonesQuery(new SetupListQuery(), new FgsSetupZoneListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
    }
}
