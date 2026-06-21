using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.SetupLaborRateTypes;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupLaborRateTypes.Dtos;
using Fgs.Setup.Application.Features.SetupLaborRateTypes.Queries.GetFgsSetupLaborRateTypeById;
using Fgs.Setup.Application.Features.SetupLaborRateTypes.Queries.ListSetupLaborRateTypes;
using Moq;

namespace Fgs.Setup.Tests.SetupLaborRateTypes;

public sealed class FgsSetupLaborRateTypeQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = new FgsSetupLaborRateTypeDetailDto(1, 10, 20, "Name value", "Description value", 1, false, true, DateTimeOffset.UtcNow, "seed", null, "seed");

        var readRepository = new Mock<IFgsSetupLaborRateTypeReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        var handler = new GetFgsSetupLaborRateTypeByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetFgsSetupLaborRateTypeByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<IFgsSetupLaborRateTypeReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((FgsSetupLaborRateTypeDetailDto?)null);

        var handler = new GetFgsSetupLaborRateTypeByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetFgsSetupLaborRateTypeByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<IFgsSetupLaborRateTypeReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(It.IsAny<SetupListQuery>(), It.IsAny<FgsSetupLaborRateTypeListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<FgsSetupLaborRateTypeSummaryDto>([], 1, 25, 0));

        var handler = new ListSetupLaborRateTypesQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListSetupLaborRateTypesQuery(new SetupListQuery(), new FgsSetupLaborRateTypeListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
    }
}
