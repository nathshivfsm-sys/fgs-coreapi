using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupTaxAuthorities;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxAuthorities.Queries.LookupSetupTaxAuthorities;

public sealed class LookupSetupTaxAuthoritiesQueryHandler(
    IFgsSetupTaxAuthorityReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupSetupTaxAuthoritiesQuery, ApiResponse<IReadOnlyList<FgsSetupTaxAuthorityLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsSetupTaxAuthorityLookupDto>>> Handle(
        LookupSetupTaxAuthoritiesQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "taxauthority",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<FgsSetupTaxAuthorityLookupDto>>.Ok(result ?? Array.Empty<FgsSetupTaxAuthorityLookupDto>());
    }
}
