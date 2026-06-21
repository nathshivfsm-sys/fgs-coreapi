using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.JobTypeCategories;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.JobTypeCategories.Dtos;
using Fgs.Setup.Application.Features.JobTypeCategories.Queries.GetJobTypeCategoryById;
using Fgs.Setup.Application.Features.JobTypeCategories.Queries.ListJobTypeCategories;
using Moq;

namespace Fgs.Setup.Tests.JobTypeCategories;

public sealed class JobTypeCategoryQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = new JobTypeCategoryDetailDto(1, 10, 20, "TEST", "Name value", "Description value", 1, true, DateTimeOffset.UtcNow, "seed", null, null);

        var readRepository = new Mock<IJobTypeCategoryReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        var handler = new GetJobTypeCategoryByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetJobTypeCategoryByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<IJobTypeCategoryReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((JobTypeCategoryDetailDto?)null);

        var handler = new GetJobTypeCategoryByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetJobTypeCategoryByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<IJobTypeCategoryReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(It.IsAny<SetupListQuery>(), It.IsAny<JobTypeCategoryListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<JobTypeCategorySummaryDto>([], 1, 25, 0));

        var handler = new ListJobTypeCategoriesQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListJobTypeCategoriesQuery(new SetupListQuery(), new JobTypeCategoryListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
    }
}
