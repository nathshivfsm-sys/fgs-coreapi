using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Setup.Application.Abstractions.GloLookups;
using Fgs.Setup.Application.Features.GloLookups.Common;
using Fgs.Setup.Application.Features.GloLookups.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloCountries;

public sealed class LookupGloCountriesQueryHandler(
    IGloCountryReadRepository readRepository,
    ICacheService cache)
    : IRequestHandler<LookupGloCountriesQuery, ApiResponse<IReadOnlyList<GloCountryLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<GloCountryLookupDto>>> Handle(
        LookupGloCountriesQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeys.GlobalLookup(
            "country",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            absoluteExpiration: GloLookupCache.Ttl,
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<GloCountryLookupDto>>.Ok(result ?? Array.Empty<GloCountryLookupDto>());
    }
}
