using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalMatrixTiers;
using Fgs.Setup.Application.Features.UniversalMatrixTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixTiers.Queries.GetFgsUniversalMatrixTierById;

public sealed class GetFgsUniversalMatrixTierByIdQueryHandler(
    IFgsUniversalMatrixTierReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsUniversalMatrixTierByIdQuery, ApiResponse<FgsUniversalMatrixTierDetailDto>>
{
    public async Task<ApiResponse<FgsUniversalMatrixTierDetailDto>> Handle(
        GetFgsUniversalMatrixTierByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "universalmatrixtier",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsUniversalMatrixTierDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsUniversalMatrixTierDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsUniversalMatrixTierDetailDto>.Fail(
                [$"Universal Matrix Tier '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsUniversalMatrixTierDetailDto>.Ok(result);
    }
}
