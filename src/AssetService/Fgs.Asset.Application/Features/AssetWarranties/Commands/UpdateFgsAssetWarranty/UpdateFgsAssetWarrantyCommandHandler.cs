using Fgs.Asset.Application.Abstractions.AssetWarranties;
using Fgs.Asset.Application.Features.AssetWarranties.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Asset.Application.Features.AssetWarranties.Commands.UpdateFgsAssetWarranty;

public sealed class UpdateFgsAssetWarrantyCommandHandler(
    IFgsAssetWarrantyWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateFgsAssetWarrantyCommandHandler> logger)
    : IRequestHandler<UpdateFgsAssetWarrantyCommand, ApiResponse<FgsAssetWarrantyDetailDto>>
{
    public async Task<ApiResponse<FgsAssetWarrantyDetailDto>> Handle(
        UpdateFgsAssetWarrantyCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "assetwarranty"),
            cancellationToken);
        return ApiResponse<FgsAssetWarrantyDetailDto>.Ok(result);
    }
}
