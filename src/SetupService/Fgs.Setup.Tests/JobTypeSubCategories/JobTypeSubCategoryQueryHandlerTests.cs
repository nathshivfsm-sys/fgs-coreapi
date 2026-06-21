using Fgs.Contracts.Api;
using Fgs.Foundation.CatalogCrud;
using Fgs.Setup.Application.Abstractions.JobTypeSubCategories;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.JobTypeSubCategories.Dtos;
using Fgs.Setup.Application.Features.JobTypeSubCategories.Queries.GetJobTypeSubCategoryById;
using Fgs.Setup.Application.Features.JobTypeSubCategories.Queries.ListJobTypeSubCategories;
using Moq;

namespace Fgs.Setup.Tests.JobTypeSubCategories;

public sealed class JobTypeSubCategoryQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = new JobTypeSubCategoryDetailDto(1, 10, 20, "TEST", "Name value", "Description value", 1, true, DateTimeOffset.UtcNow, "seed", null, null);

        var readRepository = new Mock<IJobTypeSubCategoryReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        var handler = new GetJobTypeSubCategoryByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetJobTypeSubCategoryByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<IJobTypeSubCategoryReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((JobTypeSubCategoryDetailDto?)null);

        var handler = new GetJobTypeSubCategoryByIdQueryHandler(readRepository.Object);
        var response = await handler.Handle(new GetJobTypeSubCategoryByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<IJobTypeSubCategoryReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(It.IsAny<SetupListQuery>(), It.IsAny<JobTypeSubCategoryListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<JobTypeSubCategorySummaryDto>([], 1, 25, 0));

        var handler = new ListJobTypeSubCategoriesQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListJobTypeSubCategoriesQuery(new SetupListQuery(), new JobTypeSubCategoryListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
    }
}
