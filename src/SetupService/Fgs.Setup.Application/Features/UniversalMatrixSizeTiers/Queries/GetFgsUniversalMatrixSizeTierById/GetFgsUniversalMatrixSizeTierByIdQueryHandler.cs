using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.UniversalMatrixSizeTiers;
using Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.UniversalMatrixSizeTiers.Queries.GetFgsUniversalMatrixSizeTierById;

public sealed class GetFgsUniversalMatrixSizeTierByIdQueryHandler(
    IFgsUniversalMatrixSizeTierReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsUniversalMatrixSizeTierByIdQuery, ApiResponse<FgsUniversalMatrixSizeTierDetailDto>>
{
    public async Task<ApiResponse<FgsUniversalMatrixSizeTierDetailDto>> Handle(
        GetFgsUniversalMatrixSizeTierByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "universalmatrixsizetier",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsUniversalMatrixSizeTierDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsUniversalMatrixSizeTierDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsUniversalMatrixSizeTierDetailDto>.Fail(
                [$"Universal Matrix Size Tier '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsUniversalMatrixSizeTierDetailDto>.Ok(result);
    }
}
