using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Setup.Application.Abstractions.GloLookups;
using Fgs.Setup.Application.Features.GloLookups.Common;
using Fgs.Setup.Application.Features.GloLookups.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloLocationTypes;

public sealed class LookupGloLocationTypesQueryHandler(
    IGloLocationTypeReadRepository readRepository,
    ICacheService cache)
    : IRequestHandler<LookupGloLocationTypesQuery, ApiResponse<IReadOnlyList<GloLocationTypeLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<GloLocationTypeLookupDto>>> Handle(
        LookupGloLocationTypesQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeys.GlobalLookup(
            "locationtype",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            absoluteExpiration: GloLookupCache.Ttl,
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<GloLocationTypeLookupDto>>.Ok(result ?? Array.Empty<GloLocationTypeLookupDto>());
    }
}
