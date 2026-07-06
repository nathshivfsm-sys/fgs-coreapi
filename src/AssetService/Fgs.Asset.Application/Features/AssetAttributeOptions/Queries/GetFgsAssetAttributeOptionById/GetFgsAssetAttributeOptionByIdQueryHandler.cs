using Fgs.Asset.Application.Abstractions.AssetAttributeOptions;
using Fgs.Asset.Application.Features.AssetAttributeOptions.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetAttributeOptions.Queries.GetFgsAssetAttributeOptionById;

public sealed class GetFgsAssetAttributeOptionByIdQueryHandler(
    IFgsAssetAttributeOptionReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsAssetAttributeOptionByIdQuery, ApiResponse<FgsAssetAttributeOptionDetailDto>>
{
    public async Task<ApiResponse<FgsAssetAttributeOptionDetailDto>> Handle(
        GetFgsAssetAttributeOptionByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "assetattributeoption",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsAssetAttributeOptionDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsAssetAttributeOptionDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsAssetAttributeOptionDetailDto>.Fail(
                [$"Asset Attribute Option '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsAssetAttributeOptionDetailDto>.Ok(result);
    }
}
