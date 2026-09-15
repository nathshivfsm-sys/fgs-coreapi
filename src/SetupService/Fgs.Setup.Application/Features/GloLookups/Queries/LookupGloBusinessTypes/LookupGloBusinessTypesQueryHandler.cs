using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Setup.Application.Abstractions.GloLookups;
using Fgs.Setup.Application.Features.GloLookups.Common;
using Fgs.Setup.Application.Features.GloLookups.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloBusinessTypes;

public sealed class LookupGloBusinessTypesQueryHandler(
    IGloBusinessTypeReadRepository readRepository,
    ICacheService cache)
    : IRequestHandler<LookupGloBusinessTypesQuery, ApiResponse<IReadOnlyList<GloBusinessTypeLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<GloBusinessTypeLookupDto>>> Handle(
        LookupGloBusinessTypesQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeys.GlobalLookup(
            "businesstype",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            absoluteExpiration: GloLookupCache.Ttl,
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<GloBusinessTypeLookupDto>>.Ok(result ?? Array.Empty<GloBusinessTypeLookupDto>());
    }
}
