using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrices;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrices.Queries.LookupSetupPricingMatrices;

public sealed class LookupSetupPricingMatricesQueryHandler(
    IFgsSetupPricingMatrixReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupSetupPricingMatricesQuery, ApiResponse<IReadOnlyList<FgsSetupPricingMatrixLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsSetupPricingMatrixLookupDto>>> Handle(
        LookupSetupPricingMatricesQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "pricingmatrix",
            $"lookup:{request.ActiveOnly}");

        var cached = await cache.GetAsync<IReadOnlyList<FgsSetupPricingMatrixLookupDto>>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<IReadOnlyList<FgsSetupPricingMatrixLookupDto>>.Ok(cached);
        }

        var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<IReadOnlyList<FgsSetupPricingMatrixLookupDto>>.Ok(result);
    }
}
