using Fgs.Asset.Application.Abstractions.AssetTypes;
using Fgs.Asset.Application.Features.AssetTypes.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Asset.Application.Features.AssetTypes.Commands.PatchFgsAssetType;

public sealed class PatchFgsAssetTypeCommandHandler(
    IFgsAssetTypeWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchFgsAssetTypeCommandHandler> logger)
    : IRequestHandler<PatchFgsAssetTypeCommand, ApiResponse<FgsAssetTypeDetailDto>>
{
    public async Task<ApiResponse<FgsAssetTypeDetailDto>> Handle(
        PatchFgsAssetTypeCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "assettype"),
            cancellationToken);
        return ApiResponse<FgsAssetTypeDetailDto>.Ok(result);
    }
}
