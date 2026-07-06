using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.BillingCategories;
using Fgs.Setup.Application.Features.BillingCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.BillingCategories.Queries.LookupBillingCategories;

public sealed class LookupBillingCategoriesQueryHandler(
    IBillingCategoryReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupBillingCategoriesQuery, ApiResponse<IReadOnlyList<BillingCategoryLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<BillingCategoryLookupDto>>> Handle(
        LookupBillingCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "billingcategory",
            $"{CacheKeys.LookupSegment(request.ActiveOnly)}:showToFieldTech={request.ShowToFieldTech?.ToString() ?? "all"}:allowToPick={request.AllowToPick?.ToString() ?? "all"}");

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, request.ShowToFieldTech, request.AllowToPick, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<BillingCategoryLookupDto>>.Ok(result ?? Array.Empty<BillingCategoryLookupDto>());
    }
}
