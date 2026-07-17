using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalMatrixAddOns;
using Fgs.Setup.Application.Features.UniversalMatrixAddOns.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixAddOns.Queries.LookupUniversalMatrixAddOns;

public sealed class LookupUniversalMatrixAddOnsQueryHandler(
    IFgsUniversalMatrixAddOnReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupUniversalMatrixAddOnsQuery, ApiResponse<IReadOnlyList<FgsUniversalMatrixAddOnLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsUniversalMatrixAddOnLookupDto>>> Handle(
        LookupUniversalMatrixAddOnsQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "universalmatrixaddon",
            $"{CacheKeys.LookupSegment(request.ActiveOnly)}:universalPricingServiceId={request.UniversalPricingServiceId?.ToString() ?? "all"}");

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, request.UniversalPricingServiceId, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<FgsUniversalMatrixAddOnLookupDto>>.Ok(result ?? Array.Empty<FgsUniversalMatrixAddOnLookupDto>());
    }
}
