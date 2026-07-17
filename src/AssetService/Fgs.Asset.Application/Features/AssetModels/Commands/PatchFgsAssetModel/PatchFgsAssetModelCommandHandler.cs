using Fgs.Asset.Application.Abstractions.AssetModels;
using Fgs.Asset.Application.Features.AssetModels.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;

namespace Fgs.Asset.Application.Features.AssetModels.Commands.PatchFgsAssetModel;

public sealed class PatchFgsAssetModelCommandHandler(
    IFgsAssetModelWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<PatchFgsAssetModelCommand, ApiResponse<FgsAssetModelDetailDto>>
{
    public async Task<ApiResponse<FgsAssetModelDetailDto>> Handle(
        PatchFgsAssetModelCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "assetmodel"),
            cancellationToken);
        return ApiResponse<FgsAssetModelDetailDto>.Ok(result);
    }
}
