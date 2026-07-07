using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalMatrixFrequencyDiscounts;
using Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixFrequencyDiscounts.Queries.LookupUniversalMatrixFrequencyDiscounts;

public sealed class LookupUniversalMatrixFrequencyDiscountsQueryHandler(
    IFgsUniversalMatrixFrequencyDiscountReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupUniversalMatrixFrequencyDiscountsQuery, ApiResponse<IReadOnlyList<FgsUniversalMatrixFrequencyDiscountLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsUniversalMatrixFrequencyDiscountLookupDto>>> Handle(
        LookupUniversalMatrixFrequencyDiscountsQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "universalmatrixfrequencydiscount",
            $"{CacheKeys.LookupSegment(request.ActiveOnly)}:universalPricingServiceId={request.UniversalPricingServiceId?.ToString() ?? "all"}");

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, request.UniversalPricingServiceId, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<FgsUniversalMatrixFrequencyDiscountLookupDto>>.Ok(result ?? Array.Empty<FgsUniversalMatrixFrequencyDiscountLookupDto>());
    }
}
