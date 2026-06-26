using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.VehicleMaintenances;
using Fgs.Setup.Application.Features.VehicleMaintenances.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.VehicleMaintenances.Queries.LookupVehicleMaintenances;

public sealed class LookupVehicleMaintenancesQueryHandler(
    IFgsVehicleMaintenanceReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupVehicleMaintenancesQuery, ApiResponse<IReadOnlyList<FgsVehicleMaintenanceLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsVehicleMaintenanceLookupDto>>> Handle(
        LookupVehicleMaintenancesQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "vehiclemaintenances",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<FgsVehicleMaintenanceLookupDto>>.Ok(result ?? Array.Empty<FgsVehicleMaintenanceLookupDto>());
    }
}
