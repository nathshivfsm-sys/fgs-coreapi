using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupLaborRateTypes;
using Fgs.Setup.Application.Features.SetupLaborRateTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupLaborRateTypes.Queries.LookupSetupLaborRateTypes;

public sealed class LookupSetupLaborRateTypesQueryHandler(
    IFgsSetupLaborRateTypeReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupSetupLaborRateTypesQuery, ApiResponse<IReadOnlyList<FgsSetupLaborRateTypeLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsSetupLaborRateTypeLookupDto>>> Handle(
        LookupSetupLaborRateTypesQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "laborratetype",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<FgsSetupLaborRateTypeLookupDto>>.Ok(result ?? Array.Empty<FgsSetupLaborRateTypeLookupDto>());
    }
}
