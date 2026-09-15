using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Setup.Application.Abstractions.GloLookups;
using Fgs.Setup.Application.Features.GloLookups.Common;
using Fgs.Setup.Application.Features.GloLookups.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloAppointmentAssignmentEventTypes;

public sealed class LookupGloAppointmentAssignmentEventTypesQueryHandler(
    IGloAppointmentAssignmentEventTypeReadRepository readRepository,
    ICacheService cache)
    : IRequestHandler<LookupGloAppointmentAssignmentEventTypesQuery, ApiResponse<IReadOnlyList<GloAppointmentAssignmentEventTypeLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<GloAppointmentAssignmentEventTypeLookupDto>>> Handle(
        LookupGloAppointmentAssignmentEventTypesQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeys.GlobalLookup(
            "appointmentassignmenteventtype",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            absoluteExpiration: GloLookupCache.Ttl,
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<GloAppointmentAssignmentEventTypeLookupDto>>.Ok(result ?? Array.Empty<GloAppointmentAssignmentEventTypeLookupDto>());
    }
}
