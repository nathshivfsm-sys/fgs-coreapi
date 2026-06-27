using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Setup.Application.Abstractions.TitlesOfCourtesy;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Dtos;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Queries.GetTitleOfCourtesyById;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Queries.ListTitlesOfCourtesy;
using Moq;

namespace Fgs.Setup.Tests.TitlesOfCourtesy;

public sealed class TitleOfCourtesyQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = new TitleOfCourtesyDetailDto(1, "MR", "Mr.", 1, true);

        var readRepository = new Mock<ITitleOfCourtesyReadRepository>();
        readRepository
            .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(detail);

        var handler = new GetTitleOfCourtesyByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetTitleOfCourtesyByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
        response.Data!.Code.Should().Be("MR");
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<ITitleOfCourtesyReadRepository>();
        readRepository
            .Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TitleOfCourtesyDetailDto?)null);

        var handler = new GetTitleOfCourtesyByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetTitleOfCourtesyByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var items = new List<TitleOfCourtesySummaryDto>
        {
            new(1, "MR", "Mr.", 1, true)
        };

        var readRepository = new Mock<ITitleOfCourtesyReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(It.IsAny<SetupListQuery>(), It.IsAny<TitleOfCourtesyListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<TitleOfCourtesySummaryDto>(items, 1, 25, 1));

        var handler = new ListTitlesOfCourtesyQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListTitlesOfCourtesyQuery(new SetupListQuery(), new TitleOfCourtesyListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data!.Items.Should().HaveCount(1);
        response.Data.TotalCount.Should().Be(1);
    }
}
