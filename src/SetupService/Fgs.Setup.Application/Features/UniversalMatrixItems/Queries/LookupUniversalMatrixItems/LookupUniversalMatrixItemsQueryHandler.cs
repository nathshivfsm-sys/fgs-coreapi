using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalMatrixItems;
using Fgs.Setup.Application.Features.UniversalMatrixItems.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixItems.Queries.LookupUniversalMatrixItems;

public sealed class LookupUniversalMatrixItemsQueryHandler(
    IFgsUniversalMatrixItemReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupUniversalMatrixItemsQuery, ApiResponse<IReadOnlyList<FgsUniversalMatrixItemLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsUniversalMatrixItemLookupDto>>> Handle(
        LookupUniversalMatrixItemsQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "universalmatrixitem",
            $"{CacheKeys.LookupSegment(request.ActiveOnly)}:ups:{request.UniversalPricingServiceId}");

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, request.UniversalPricingServiceId, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<FgsUniversalMatrixItemLookupDto>>.Ok(result ?? Array.Empty<FgsUniversalMatrixItemLookupDto>());
    }
}
