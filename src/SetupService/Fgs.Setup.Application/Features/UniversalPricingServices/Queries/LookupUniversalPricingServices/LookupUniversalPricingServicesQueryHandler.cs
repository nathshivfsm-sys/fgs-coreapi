using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalPricingServices;
using Fgs.Setup.Application.Features.UniversalPricingServices.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalPricingServices.Queries.LookupUniversalPricingServices;

public sealed class LookupUniversalPricingServicesQueryHandler(
    IFgsUniversalPricingServiceReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupUniversalPricingServicesQuery, ApiResponse<IReadOnlyList<FgsUniversalPricingServiceLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsUniversalPricingServiceLookupDto>>> Handle(
        LookupUniversalPricingServicesQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "universalpricingservice",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<FgsUniversalPricingServiceLookupDto>>.Ok(result ?? Array.Empty<FgsUniversalPricingServiceLookupDto>());
    }
}
