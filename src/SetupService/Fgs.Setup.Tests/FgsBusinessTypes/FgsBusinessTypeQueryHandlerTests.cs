using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.FgsBusinessTypes;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.FgsBusinessTypes.Dtos;
using Fgs.Setup.Application.Features.FgsBusinessTypes.Queries.GetFgsBusinessTypeById;
using Fgs.Setup.Application.Features.FgsBusinessTypes.Queries.ListFgsBusinessTypes;
using Moq;

namespace Fgs.Setup.Tests.FgsBusinessTypes;

public sealed class FgsBusinessTypeQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = new FgsBusinessTypeDetailDto(1, 10, 20, "TEST", "Name value", "Description value", 1, true, DateTimeOffset.UtcNow, "seed", null, null);

        var readRepository = new Mock<IFgsBusinessTypeReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        var handler = new GetFgsBusinessTypeByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetFgsBusinessTypeByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<IFgsBusinessTypeReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((FgsBusinessTypeDetailDto?)null);

        var handler = new GetFgsBusinessTypeByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetFgsBusinessTypeByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<IFgsBusinessTypeReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(It.IsAny<SetupListQuery>(), It.IsAny<FgsBusinessTypeListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<FgsBusinessTypeSummaryDto>([], 1, 25, 0));

        var handler = new ListFgsBusinessTypesQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListFgsBusinessTypesQuery(new SetupListQuery(), new FgsBusinessTypeListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
    }
}
