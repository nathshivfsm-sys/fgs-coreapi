using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPostalCodes;
using Fgs.Setup.Application.Features.SetupPostalCodes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPostalCodes.Queries.LookupPostalCodeCities;

public sealed class LookupPostalCodeCitiesQueryHandler(
    IFgsSetupPostalCodeReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupPostalCodeCitiesQuery, ApiResponse<IReadOnlyList<PostalCodeCityLookupDto>>>
{
    private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(30);

    public async Task<ApiResponse<IReadOnlyList<PostalCodeCityLookupDto>>> Handle(
        LookupPostalCodeCitiesQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var countryCode = string.IsNullOrWhiteSpace(request.CountryCode)
            ? string.Empty
            : request.CountryCode.Trim().ToUpperInvariant();
        var stateProvinceCode = string.IsNullOrWhiteSpace(request.StateProvinceCode)
            ? string.Empty
            : request.StateProvinceCode.Trim().ToUpperInvariant();

        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "postalcode-city",
            $"lookup:countryCode={countryCode}:stateProvinceCode={stateProvinceCode}:{CacheKeys.LookupSegment(request.ActiveOnly)}");

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupDistinctCitiesAsync(
                string.IsNullOrEmpty(countryCode) ? null : countryCode,
                string.IsNullOrEmpty(stateProvinceCode) ? null : stateProvinceCode,
                request.ActiveOnly,
                cancellationToken),
            absoluteExpiration: CacheTtl,
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<PostalCodeCityLookupDto>>.Ok(
            result ?? Array.Empty<PostalCodeCityLookupDto>());
    }
}
