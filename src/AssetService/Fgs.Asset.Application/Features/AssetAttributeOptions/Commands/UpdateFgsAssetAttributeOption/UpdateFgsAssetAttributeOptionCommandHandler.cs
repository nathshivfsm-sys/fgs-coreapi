using Fgs.Asset.Application.Abstractions.AssetAttributeOptions;
using Fgs.Asset.Application.Features.AssetAttributeOptions.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Asset.Application.Features.AssetAttributeOptions.Commands.UpdateFgsAssetAttributeOption;

public sealed class UpdateFgsAssetAttributeOptionCommandHandler(
    IFgsAssetAttributeOptionWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateFgsAssetAttributeOptionCommandHandler> logger)
    : IRequestHandler<UpdateFgsAssetAttributeOptionCommand, ApiResponse<FgsAssetAttributeOptionDetailDto>>
{
    public async Task<ApiResponse<FgsAssetAttributeOptionDetailDto>> Handle(
        UpdateFgsAssetAttributeOptionCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "assetattributeoption"),
            cancellationToken);
        return ApiResponse<FgsAssetAttributeOptionDetailDto>.Ok(result);
    }
}
