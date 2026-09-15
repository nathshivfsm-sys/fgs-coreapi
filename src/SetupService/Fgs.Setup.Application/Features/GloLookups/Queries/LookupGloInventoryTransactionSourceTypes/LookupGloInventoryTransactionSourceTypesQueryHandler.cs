using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Setup.Application.Abstractions.GloLookups;
using Fgs.Setup.Application.Features.GloLookups.Common;
using Fgs.Setup.Application.Features.GloLookups.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloInventoryTransactionSourceTypes;

public sealed class LookupGloInventoryTransactionSourceTypesQueryHandler(
    IGloInventoryTransactionSourceTypeReadRepository readRepository,
    ICacheService cache)
    : IRequestHandler<LookupGloInventoryTransactionSourceTypesQuery, ApiResponse<IReadOnlyList<GloInventoryTransactionSourceTypeLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<GloInventoryTransactionSourceTypeLookupDto>>> Handle(
        LookupGloInventoryTransactionSourceTypesQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeys.GlobalLookup(
            "inventorytransactionsource",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            absoluteExpiration: GloLookupCache.Ttl,
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<GloInventoryTransactionSourceTypeLookupDto>>.Ok(result ?? Array.Empty<GloInventoryTransactionSourceTypeLookupDto>());
    }
}
