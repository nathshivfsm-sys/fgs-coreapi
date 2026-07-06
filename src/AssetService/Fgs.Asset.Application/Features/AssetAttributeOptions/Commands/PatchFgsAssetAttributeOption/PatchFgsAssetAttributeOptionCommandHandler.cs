using Fgs.Asset.Application.Abstractions.AssetAttributeOptions;
using Fgs.Asset.Application.Features.AssetAttributeOptions.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Asset.Application.Features.AssetAttributeOptions.Commands.PatchFgsAssetAttributeOption;

public sealed class PatchFgsAssetAttributeOptionCommandHandler(
    IFgsAssetAttributeOptionWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchFgsAssetAttributeOptionCommandHandler> logger)
    : IRequestHandler<PatchFgsAssetAttributeOptionCommand, ApiResponse<FgsAssetAttributeOptionDetailDto>>
{
    public async Task<ApiResponse<FgsAssetAttributeOptionDetailDto>> Handle(
        PatchFgsAssetAttributeOptionCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "assetattributeoption"),
            cancellationToken);
        return ApiResponse<FgsAssetAttributeOptionDetailDto>.Ok(result);
    }
}
