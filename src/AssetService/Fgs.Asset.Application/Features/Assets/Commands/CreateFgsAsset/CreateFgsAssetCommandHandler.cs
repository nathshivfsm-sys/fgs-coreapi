using Fgs.Asset.Application.Abstractions.Assets;
using Fgs.Asset.Application.Features.Assets.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Asset.Application.Features.Assets.Commands.CreateFgsAsset;

public sealed class CreateFgsAssetCommandHandler(
    IFgsAssetWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsAssetCommandHandler> logger)
    : IRequestHandler<CreateFgsAssetCommand, ApiResponse<FgsAssetDetailDto>>
{
    public async Task<ApiResponse<FgsAssetDetailDto>> Handle(
        CreateFgsAssetCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "asset"),
            cancellationToken);
        return ApiResponse<FgsAssetDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
