using Fgs.Asset.Application.Abstractions.Assets;
using Fgs.Asset.Application.Features.Assets.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;

namespace Fgs.Asset.Application.Features.Assets.Queries.GetFgsAssetById;

public sealed class GetFgsAssetByIdQueryHandler(
    IFgsAssetReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsAssetByIdQuery, ApiResponse<FgsAssetDetailDto>>
{
    public async Task<ApiResponse<FgsAssetDetailDto>> Handle(
        GetFgsAssetByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "asset",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsAssetDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsAssetDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsAssetDetailDto>.Fail(
                [$"Asset '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsAssetDetailDto>.Ok(result);
    }
}
