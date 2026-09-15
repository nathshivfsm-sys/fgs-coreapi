using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Setup.Application.Abstractions.GloLookups;
using Fgs.Setup.Application.Features.GloLookups.Common;
using Fgs.Setup.Application.Features.GloLookups.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloStateProvinces;

public sealed class LookupGloStateProvincesQueryHandler(
    IGloStateProvinceReadRepository readRepository,
    ICacheService cache)
    : IRequestHandler<LookupGloStateProvincesQuery, ApiResponse<IReadOnlyList<GloStateProvinceLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<GloStateProvinceLookupDto>>> Handle(
        LookupGloStateProvincesQuery request,
        CancellationToken cancellationToken)
    {
        var countryCode = request.CountryCode.Trim().ToUpperInvariant();
        var cacheKey = CacheKeys.GlobalLookup(
            "stateprovince",
            $"lookup:countryCode={countryCode}:{CacheKeys.LookupSegment(request.ActiveOnly)}");

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(countryCode, request.ActiveOnly, cancellationToken),
            absoluteExpiration: GloLookupCache.Ttl,
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<GloStateProvinceLookupDto>>.Ok(
            result ?? Array.Empty<GloStateProvinceLookupDto>());
    }
}
