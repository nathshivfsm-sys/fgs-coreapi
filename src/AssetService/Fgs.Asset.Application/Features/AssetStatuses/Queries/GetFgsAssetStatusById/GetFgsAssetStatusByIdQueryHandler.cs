using Fgs.Asset.Application.Abstractions.AssetStatuses;
using Fgs.Asset.Application.Features.AssetStatuses.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetStatuses.Queries.GetFgsAssetStatusById;

public sealed class GetFgsAssetStatusByIdQueryHandler(
    IFgsAssetStatusReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsAssetStatusByIdQuery, ApiResponse<FgsAssetStatusDetailDto>>
{
    public async Task<ApiResponse<FgsAssetStatusDetailDto>> Handle(
        GetFgsAssetStatusByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "assetstatus",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsAssetStatusDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsAssetStatusDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsAssetStatusDetailDto>.Fail(
                [$"Asset Status '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsAssetStatusDetailDto>.Ok(result);
    }
}
