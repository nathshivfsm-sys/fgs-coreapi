using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.InventoryCategories;
using Fgs.Inventory.Application.Features.InventoryCategories.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Inventory.Application.Features.InventoryCategories.Commands.PatchFgsInventoryCategory;

public sealed class PatchFgsInventoryCategoryCommandHandler(
    IFgsInventoryCategoryWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchFgsInventoryCategoryCommandHandler> logger)
    : IRequestHandler<PatchFgsInventoryCategoryCommand, ApiResponse<FgsInventoryCategoryDetailDto>>
{
    public async Task<ApiResponse<FgsInventoryCategoryDetailDto>> Handle(
        PatchFgsInventoryCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patched inventory category {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "inventorycategory"),
            cancellationToken);
        return ApiResponse<FgsInventoryCategoryDetailDto>.Ok(result);
    }
}
