using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.JobTypeCategories;
using Fgs.Setup.Application.Features.JobTypeCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypeCategories.Queries.LookupJobTypeCategories;

public sealed class LookupJobTypeCategoriesQueryHandler(
    IJobTypeCategoryReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupJobTypeCategoriesQuery, ApiResponse<IReadOnlyList<JobTypeCategoryLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<JobTypeCategoryLookupDto>>> Handle(
        LookupJobTypeCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "jobtypecategory",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<JobTypeCategoryLookupDto>>.Ok(result ?? Array.Empty<JobTypeCategoryLookupDto>());
    }
}
