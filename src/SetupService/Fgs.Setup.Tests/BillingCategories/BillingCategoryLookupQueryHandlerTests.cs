using Fgs.Contracts.Api;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.BillingCategories;
using Fgs.Setup.Application.Features.BillingCategories.Dtos;
using Fgs.Setup.Application.Features.BillingCategories.Queries.LookupBillingCategories;
using Moq;

namespace Fgs.Setup.Tests.BillingCategories;

public sealed class BillingCategoryLookupQueryHandlerTests
{
    [Fact]
    public async Task Lookup_PassesFiltersToRepository()
    {
        var readRepository = new Mock<IBillingCategoryReadRepository>();
        readRepository
            .Setup(r => r.LookupAsync(true, true, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<BillingCategoryLookupDto> { new(1, "LABOR", "Labor", 1) });

        var cache = new Mock<ICacheService>();
        cache
            .Setup(c => c.GetOrSetAsync(
                It.IsAny<string>(),
                It.IsAny<Func<Task<IReadOnlyList<BillingCategoryLookupDto>?>>>(),
                It.IsAny<TimeSpan?>(),
                It.IsAny<CancellationToken>()))
            .Returns((string _, Func<Task<IReadOnlyList<BillingCategoryLookupDto>?>> factory, TimeSpan? _, CancellationToken __) => factory());

        var tenantAccessor = new Mock<ITenantContextAccessor>();
        tenantAccessor.Setup(t => t.Current).Returns(new TenantContext { TenantId = 10, CompanyId = 20 });

        var handler = new LookupBillingCategoriesQueryHandler(
            readRepository.Object,
            cache.Object,
            tenantAccessor.Object);

        var response = await handler.Handle(
            new LookupBillingCategoriesQuery(ActiveOnly: true, ShowToFieldTech: true, AllowToPick: true),
            CancellationToken.None);

        response.Success.Should().BeTrue();
        response.Data.Should().ContainSingle();
        readRepository.Verify(r => r.LookupAsync(true, true, true, It.IsAny<CancellationToken>()), Times.Once);
    }
}
