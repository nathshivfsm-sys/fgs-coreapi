using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.JobCategories;
using Fgs.Setup.Application.Features.JobCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobCategories.Queries.LookupJobCategories;

public sealed class LookupJobCategoriesQueryHandler(
    IJobCategoryReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupJobCategoriesQuery, ApiResponse<IReadOnlyList<JobCategoryLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<JobCategoryLookupDto>>> Handle(
        LookupJobCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "jobcategory",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<JobCategoryLookupDto>>.Ok(result ?? Array.Empty<JobCategoryLookupDto>());
    }
}
