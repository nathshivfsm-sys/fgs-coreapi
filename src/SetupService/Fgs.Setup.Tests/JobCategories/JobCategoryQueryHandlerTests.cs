using Fgs.Contracts.Api;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.JobCategories;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.JobCategories.Dtos;
using Fgs.Setup.Application.Features.JobCategories.Queries.GetJobCategoryById;
using Fgs.Setup.Application.Features.JobCategories.Queries.ListJobCategories;
using Moq;

namespace Fgs.Setup.Tests.JobCategories;

public sealed class JobCategoryQueryHandlerTests
{
    [Fact]
    public async Task GetById_WhenFound_ReturnsOk()
    {
        var detail = new JobCategoryDetailDto(1, "TEST", "Name", 1, true);

        var readRepository = new Mock<IJobCategoryReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new GetJobCategoryByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetJobCategoryByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
        readRepository.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<IJobCategoryReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((JobCategoryDetailDto?)null);

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new GetJobCategoryByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetJobCategoryByIdQuery(99), CancellationToken.None);

        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(ApiStatusCodes.NotFound);
    }

    [Fact]
    public async Task List_ReturnsPagedResult()
    {
        var readRepository = new Mock<IJobCategoryReadRepository>();
        readRepository
            .Setup(r => r.ListAsync(It.IsAny<SetupListQuery>(), It.IsAny<JobCategoryListFilters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<JobCategorySummaryDto>([], 1, 25, 0));

        var handler = new ListJobCategoriesQueryHandler(readRepository.Object);
        var response = await handler.Handle(
            new ListJobCategoriesQuery(new SetupListQuery(), new JobCategoryListFilters()),
            CancellationToken.None);

        response.Success.Should().BeTrue();
    }
}
