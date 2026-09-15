using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Setup.Application.Abstractions.GloLookups;
using Fgs.Setup.Application.Features.GloLookups.Common;
using Fgs.Setup.Application.Features.GloLookups.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloUnitOfMeasures;

public sealed class LookupGloUnitOfMeasuresQueryHandler(
    IGloUnitOfMeasureReadRepository readRepository,
    ICacheService cache)
    : IRequestHandler<LookupGloUnitOfMeasuresQuery, ApiResponse<IReadOnlyList<GloUnitOfMeasureLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<GloUnitOfMeasureLookupDto>>> Handle(
        LookupGloUnitOfMeasuresQuery request,
        CancellationToken cancellationToken)
    {
        var unitType = string.IsNullOrWhiteSpace(request.UnitType)
            ? null
            : request.UnitType.Trim();
        var unitTypeSegment = unitType is null ? string.Empty : unitType.ToUpperInvariant();
        var cacheKey = CacheKeys.GlobalLookup(
            "unitofmeasure",
            $"lookup:unitType={unitTypeSegment}:{CacheKeys.LookupSegment(request.ActiveOnly)}");

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(unitType, request.ActiveOnly, cancellationToken),
            absoluteExpiration: GloLookupCache.Ttl,
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<GloUnitOfMeasureLookupDto>>.Ok(
            result ?? Array.Empty<GloUnitOfMeasureLookupDto>());
    }
}
