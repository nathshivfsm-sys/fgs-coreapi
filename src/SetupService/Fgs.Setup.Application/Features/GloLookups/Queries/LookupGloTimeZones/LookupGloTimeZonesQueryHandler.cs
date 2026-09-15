using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Setup.Application.Abstractions.GloLookups;
using Fgs.Setup.Application.Features.GloLookups.Common;
using Fgs.Setup.Application.Features.GloLookups.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloTimeZones;

public sealed class LookupGloTimeZonesQueryHandler(
    IGloTimeZoneReadRepository readRepository,
    ICacheService cache)
    : IRequestHandler<LookupGloTimeZonesQuery, ApiResponse<IReadOnlyList<GloTimeZoneLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<GloTimeZoneLookupDto>>> Handle(
        LookupGloTimeZonesQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeys.GlobalLookup(
            "timezone",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            absoluteExpiration: GloLookupCache.Ttl,
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<GloTimeZoneLookupDto>>.Ok(result ?? Array.Empty<GloTimeZoneLookupDto>());
    }
}
