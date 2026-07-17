using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrices;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPricingMatrices.Queries.GetFgsSetupPricingMatrixById;

public sealed class GetFgsSetupPricingMatrixByIdQueryHandler(
    IFgsSetupPricingMatrixReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsSetupPricingMatrixByIdQuery, ApiResponse<FgsSetupPricingMatrixDetailDto>>
{
    public async Task<ApiResponse<FgsSetupPricingMatrixDetailDto>> Handle(
        GetFgsSetupPricingMatrixByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "pricingmatrix",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsSetupPricingMatrixDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsSetupPricingMatrixDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsSetupPricingMatrixDetailDto>.Fail(
                [$"Pricing matrix '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsSetupPricingMatrixDetailDto>.Ok(result);
    }
}
