using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.Tags;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.Tags.Dtos;
using Fgs.Setup.Application.Features.Tags.Queries.GetFgsTagById;
using Fgs.Setup.Application.Features.Tags.Queries.ListTags;
using Moq;

namespace Fgs.Setup.Tests.Tags;

public sealed class FgsTagQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = new FgsTagDetailDto(1, 10, 20, "TEST", "Name", "Description value", "BackgroundColor", "TextColor", null, 1, true, DateTimeOffset.UtcNow, "seed", null, "seed");

        var readRepository = new Mock<IFgsTagReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        var handler = new GetFgsTagByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetFgsTagByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<IFgsTagReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((FgsTagDetailDto?)null);

        var handler = new GetFgsTagByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetFgsTagByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<IFgsTagReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(It.IsAny<SetupListQuery>(), It.IsAny<FgsTagListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<FgsTagSummaryDto>([], 1, 25, 0));

        var handler = new ListTagsQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListTagsQuery(new SetupListQuery(), new FgsTagListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
    }
}
