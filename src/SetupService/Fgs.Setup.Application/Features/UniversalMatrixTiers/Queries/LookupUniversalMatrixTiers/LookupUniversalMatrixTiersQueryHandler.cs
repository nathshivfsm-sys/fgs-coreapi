using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalMatrixTiers;
using Fgs.Setup.Application.Features.UniversalMatrixTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixTiers.Queries.LookupUniversalMatrixTiers;

public sealed class LookupUniversalMatrixTiersQueryHandler(
    IFgsUniversalMatrixTierReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupUniversalMatrixTiersQuery, ApiResponse<IReadOnlyList<FgsUniversalMatrixTierLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsUniversalMatrixTierLookupDto>>> Handle(
        LookupUniversalMatrixTiersQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "universalmatrixtier",
            $"{CacheKeys.LookupSegment(request.ActiveOnly)}:ups:{request.UniversalPricingServiceId}");

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, request.UniversalPricingServiceId, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<FgsUniversalMatrixTierLookupDto>>.Ok(result ?? Array.Empty<FgsUniversalMatrixTierLookupDto>());
    }
}
