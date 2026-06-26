using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupTimeSlots;
using Fgs.Setup.Application.Features.SetupTimeSlots.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTimeSlots.Queries.LookupSetupTimeSlots;

public sealed class LookupSetupTimeSlotsQueryHandler(
    IFgsSetupTimeSlotReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupSetupTimeSlotsQuery, ApiResponse<IReadOnlyList<FgsSetupTimeSlotLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsSetupTimeSlotLookupDto>>> Handle(
        LookupSetupTimeSlotsQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "timeslots",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<FgsSetupTimeSlotLookupDto>>.Ok(result ?? Array.Empty<FgsSetupTimeSlotLookupDto>());
    }
}
