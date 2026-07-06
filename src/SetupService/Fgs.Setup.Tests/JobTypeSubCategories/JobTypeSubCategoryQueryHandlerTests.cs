using Fgs.Contracts.Api;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
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
        var detail = new JobTypeSubCategoryDetailDto(1, "TEST", "Name", "Description value", 1);

        var readRepository = new Mock<IJobTypeSubCategoryReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(detail);

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new GetJobTypeSubCategoryByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
        var response = await handler.Handle(new GetJobTypeSubCategoryByIdQuery(1), CancellationToken.None);

        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(ApiStatusCodes.Ok);
        readRepository.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_WhenMissing_ReturnsNotFound()
    {
        var readRepository = new Mock<IJobTypeSubCategoryReadRepository>();
        readRepository.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((JobTypeSubCategoryDetailDto?)null);

        var cache = new Mock<ICacheService>();
        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new GetJobTypeSubCategoryByIdQueryHandler(readRepository.Object, cache.Object, tenantAccessor.Object);
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
