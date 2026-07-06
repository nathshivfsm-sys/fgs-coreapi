using Fgs.Asset.Application.Abstractions.AssetTypes;
using Fgs.Asset.Application.Features.AssetTypes.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetTypes.Queries.GetFgsAssetTypeById;

public sealed class GetFgsAssetTypeByIdQueryHandler(
    IFgsAssetTypeReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsAssetTypeByIdQuery, ApiResponse<FgsAssetTypeDetailDto>>
{
    public async Task<ApiResponse<FgsAssetTypeDetailDto>> Handle(
        GetFgsAssetTypeByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "assettype",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsAssetTypeDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsAssetTypeDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsAssetTypeDetailDto>.Fail(
                [$"Asset Type '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsAssetTypeDetailDto>.Ok(result);
    }
}
