using Fgs.Asset.Application.Abstractions.AssetModels;
using Fgs.Asset.Application.Features.AssetModels.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Asset.Application.Features.AssetModels.Commands.CreateFgsAssetModel;

public sealed class CreateFgsAssetModelCommandHandler(
    IFgsAssetModelWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsAssetModelCommandHandler> logger)
    : IRequestHandler<CreateFgsAssetModelCommand, ApiResponse<FgsAssetModelDetailDto>>
{
    public async Task<ApiResponse<FgsAssetModelDetailDto>> Handle(
        CreateFgsAssetModelCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "assetmodel"),
            cancellationToken);
        return ApiResponse<FgsAssetModelDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
