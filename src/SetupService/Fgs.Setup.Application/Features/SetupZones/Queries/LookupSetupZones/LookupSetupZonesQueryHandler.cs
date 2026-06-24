using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.CatalogCrud;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupZones;
using Fgs.Setup.Application.Features.SetupZones.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupZones.Queries.LookupSetupZones;

public sealed class LookupSetupZonesQueryHandler(
    IFgsSetupZoneReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupSetupZonesQuery, ApiResponse<IReadOnlyList<FgsSetupZoneLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsSetupZoneLookupDto>>> Handle(
        LookupSetupZonesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var tenantScope = tenantContextAccessor.Current;
            if (tenantScope?.IsResolved == true)
            {
                var cacheKey = CacheKeys.Build(
                    tenantScope.TenantId,
                    tenantScope.CompanyId,
                    "zones",
                    CacheKeys.LookupSegment(request.ActiveOnly));

                var result = await cache.GetOrSetAsync(
                    cacheKey,
                    () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
                    cancellationToken: cancellationToken);

                return ApiResponse<IReadOnlyList<FgsSetupZoneLookupDto>>.Ok(result ?? Array.Empty<FgsSetupZoneLookupDto>());
            }

            var uncached = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
            return ApiResponse<IReadOnlyList<FgsSetupZoneLookupDto>>.Ok(uncached);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<IReadOnlyList<FgsSetupZoneLookupDto>>(ex);
        }
    }
}
