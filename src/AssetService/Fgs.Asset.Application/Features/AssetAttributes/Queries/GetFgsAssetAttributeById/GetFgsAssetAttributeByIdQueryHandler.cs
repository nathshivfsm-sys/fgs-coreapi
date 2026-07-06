using Fgs.Asset.Application.Abstractions.AssetAttributes;
using Fgs.Asset.Application.Features.AssetAttributes.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetAttributes.Queries.GetFgsAssetAttributeById;

public sealed class GetFgsAssetAttributeByIdQueryHandler(
    IFgsAssetAttributeReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsAssetAttributeByIdQuery, ApiResponse<FgsAssetAttributeDetailDto>>
{
    public async Task<ApiResponse<FgsAssetAttributeDetailDto>> Handle(
        GetFgsAssetAttributeByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "assetattribute",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsAssetAttributeDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsAssetAttributeDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsAssetAttributeDetailDto>.Fail(
                [$"Asset Attribute '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsAssetAttributeDetailDto>.Ok(result);
    }
}
