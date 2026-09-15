using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Setup.Application.Abstractions.GloLookups;
using Fgs.Setup.Application.Features.GloLookups.Common;
using Fgs.Setup.Application.Features.GloLookups.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloVehicleMaintenanceTypes;

public sealed class LookupGloVehicleMaintenanceTypesQueryHandler(
    IGloVehicleMaintenanceTypeReadRepository readRepository,
    ICacheService cache)
    : IRequestHandler<LookupGloVehicleMaintenanceTypesQuery, ApiResponse<IReadOnlyList<GloVehicleMaintenanceTypeLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<GloVehicleMaintenanceTypeLookupDto>>> Handle(
        LookupGloVehicleMaintenanceTypesQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeys.GlobalLookup(
            "vehiclemaintenancetype",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            absoluteExpiration: GloLookupCache.Ttl,
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<GloVehicleMaintenanceTypeLookupDto>>.Ok(result ?? Array.Empty<GloVehicleMaintenanceTypeLookupDto>());
    }
}
