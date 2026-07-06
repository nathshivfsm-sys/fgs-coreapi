using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.BillingCategories;
using Fgs.Setup.Application.Features.BillingCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.BillingCategories.Queries.GetBillingCategoryById;

public sealed class GetBillingCategoryByIdQueryHandler(
    IBillingCategoryReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetBillingCategoryByIdQuery, ApiResponse<BillingCategoryDetailDto>>
{
    public async Task<ApiResponse<BillingCategoryDetailDto>> Handle(
        GetBillingCategoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "billingcategory",
            request.Id.ToString());

        var cached = await cache.GetAsync<BillingCategoryDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<BillingCategoryDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<BillingCategoryDetailDto>.Fail(
                [$"Billing Category '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<BillingCategoryDetailDto>.Ok(result);
    }
}
