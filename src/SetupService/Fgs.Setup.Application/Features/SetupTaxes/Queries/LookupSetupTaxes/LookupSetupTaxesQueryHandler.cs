using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupTaxes;
using Fgs.Setup.Application.Features.SetupTaxes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxes.Queries.LookupSetupTaxes;

public sealed class LookupSetupTaxesQueryHandler(
    IFgsSetupTaxReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupSetupTaxesQuery, ApiResponse<IReadOnlyList<FgsSetupTaxLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsSetupTaxLookupDto>>> Handle(
        LookupSetupTaxesQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "taxes",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<FgsSetupTaxLookupDto>>.Ok(result ?? Array.Empty<FgsSetupTaxLookupDto>());
    }
}
