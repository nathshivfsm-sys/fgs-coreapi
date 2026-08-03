using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalMatrixSizeTiers;
using Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Queries.LookupUniversalMatrixSizeTiers;

public sealed class LookupUniversalMatrixSizeTiersQueryHandler(
    IFgsUniversalMatrixSizeTierReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupUniversalMatrixSizeTiersQuery, ApiResponse<IReadOnlyList<FgsUniversalMatrixSizeTierLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsUniversalMatrixSizeTierLookupDto>>> Handle(
        LookupUniversalMatrixSizeTiersQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "universalmatrixsizetier",
            $"{CacheKeys.LookupSegment(request.ActiveOnly)}:ups:{request.UniversalPricingServiceId}");

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, request.UniversalPricingServiceId, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<FgsUniversalMatrixSizeTierLookupDto>>.Ok(result ?? Array.Empty<FgsUniversalMatrixSizeTierLookupDto>());
    }
}
