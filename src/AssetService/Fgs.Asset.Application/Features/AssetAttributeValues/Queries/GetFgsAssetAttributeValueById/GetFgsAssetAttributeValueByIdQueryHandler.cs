using Fgs.Asset.Application.Abstractions.AssetAttributeValues;
using Fgs.Asset.Application.Features.AssetAttributeValues.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetAttributeValues.Queries.GetFgsAssetAttributeValueById;

public sealed class GetFgsAssetAttributeValueByIdQueryHandler(
    IFgsAssetAttributeValueReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsAssetAttributeValueByIdQuery, ApiResponse<FgsAssetAttributeValueDetailDto>>
{
    public async Task<ApiResponse<FgsAssetAttributeValueDetailDto>> Handle(
        GetFgsAssetAttributeValueByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "assetattributevalue",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsAssetAttributeValueDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsAssetAttributeValueDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsAssetAttributeValueDetailDto>.Fail(
                [$"Asset Attribute Value '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsAssetAttributeValueDetailDto>.Ok(result);
    }
}
