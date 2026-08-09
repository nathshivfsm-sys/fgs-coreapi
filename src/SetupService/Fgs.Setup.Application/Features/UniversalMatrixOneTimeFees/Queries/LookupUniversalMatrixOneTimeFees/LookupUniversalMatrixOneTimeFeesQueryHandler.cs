using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalMatrixOneTimeFees;
using Fgs.Setup.Application.Features.UniversalMatrixOneTimeFees.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixOneTimeFees.Queries.LookupUniversalMatrixOneTimeFees;

public sealed class LookupUniversalMatrixOneTimeFeesQueryHandler(
    IFgsUniversalMatrixOneTimeFeeReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupUniversalMatrixOneTimeFeesQuery, ApiResponse<IReadOnlyList<FgsUniversalMatrixOneTimeFeeLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsUniversalMatrixOneTimeFeeLookupDto>>> Handle(
        LookupUniversalMatrixOneTimeFeesQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "universalmatrixonetimefee",
            $"{CacheKeys.LookupSegment(request.ActiveOnly)}:ups:{request.UniversalPricingServiceId}");

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, request.UniversalPricingServiceId, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<FgsUniversalMatrixOneTimeFeeLookupDto>>.Ok(result ?? Array.Empty<FgsUniversalMatrixOneTimeFeeLookupDto>());
    }
}
