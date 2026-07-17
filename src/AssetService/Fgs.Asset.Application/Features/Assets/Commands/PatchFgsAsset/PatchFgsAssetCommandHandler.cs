using Fgs.Asset.Application.Abstractions.Assets;
using Fgs.Asset.Application.Features.Assets.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;

namespace Fgs.Asset.Application.Features.Assets.Commands.PatchFgsAsset;

public sealed class PatchFgsAssetCommandHandler(
    IFgsAssetWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<PatchFgsAssetCommand, ApiResponse<FgsAssetDetailDto>>
{
    public async Task<ApiResponse<FgsAssetDetailDto>> Handle(
        PatchFgsAssetCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "asset"),
            cancellationToken);
        return ApiResponse<FgsAssetDetailDto>.Ok(result);
    }
}
