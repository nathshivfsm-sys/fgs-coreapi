using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalPricingServices;
using Fgs.Setup.Application.Features.UniversalPricingServices.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalPricingServices.Queries.GetFgsUniversalPricingServiceById;

public sealed class GetFgsUniversalPricingServiceByIdQueryHandler(
    IFgsUniversalPricingServiceReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsUniversalPricingServiceByIdQuery, ApiResponse<FgsUniversalPricingServiceDetailDto>>
{
    public async Task<ApiResponse<FgsUniversalPricingServiceDetailDto>> Handle(
        GetFgsUniversalPricingServiceByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "universalpricingservice",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsUniversalPricingServiceDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsUniversalPricingServiceDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsUniversalPricingServiceDetailDto>.Fail(
                [$"Universal Pricing Service '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsUniversalPricingServiceDetailDto>.Ok(result);
    }
}
