using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupDescriptions;
using Fgs.Setup.Application.Features.SetupDescriptions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupDescriptions.Queries.LookupSetupDescriptions;

public sealed class LookupSetupDescriptionsQueryHandler(
    IFgsSetupDescriptionReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupSetupDescriptionsQuery, ApiResponse<IReadOnlyList<FgsSetupDescriptionLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsSetupDescriptionLookupDto>>> Handle(
        LookupSetupDescriptionsQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "setupdescription",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<FgsSetupDescriptionLookupDto>>.Ok(result ?? Array.Empty<FgsSetupDescriptionLookupDto>());
    }
}
