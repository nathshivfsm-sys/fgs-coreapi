using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Setup.Application.Abstractions.GloLookups;
using Fgs.Setup.Application.Features.GloLookups.Common;
using Fgs.Setup.Application.Features.GloLookups.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloInventoryTransactionTypes;

public sealed class LookupGloInventoryTransactionTypesQueryHandler(
    IGloInventoryTransactionTypeReadRepository readRepository,
    ICacheService cache)
    : IRequestHandler<LookupGloInventoryTransactionTypesQuery, ApiResponse<IReadOnlyList<GloInventoryTransactionTypeLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<GloInventoryTransactionTypeLookupDto>>> Handle(
        LookupGloInventoryTransactionTypesQuery request,
        CancellationToken cancellationToken)
    {
        var sourceSegment = request.SourceTypeId?.ToString() ?? string.Empty;
        var cacheKey = CacheKeys.GlobalLookup(
            "inventorytransactiontype",
            $"lookup:sourceTypeId={sourceSegment}:{CacheKeys.LookupSegment(request.ActiveOnly)}");

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.SourceTypeId, request.ActiveOnly, cancellationToken),
            absoluteExpiration: GloLookupCache.Ttl,
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<GloInventoryTransactionTypeLookupDto>>.Ok(
            result ?? Array.Empty<GloInventoryTransactionTypeLookupDto>());
    }
}
