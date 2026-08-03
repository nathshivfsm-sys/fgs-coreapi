using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.InventoryItems;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Inventory.Application.Features.InventoryItems.Commands.PatchFgsInventoryItem;

public sealed class PatchFgsInventoryItemCommandHandler(
    IFgsInventoryItemWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchFgsInventoryItemCommandHandler> logger)
    : IRequestHandler<PatchFgsInventoryItemCommand, ApiResponse<FgsInventoryItemDetailDto>>
{
    public async Task<ApiResponse<FgsInventoryItemDetailDto>> Handle(
        PatchFgsInventoryItemCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patched inventory item {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "inventoryitem"),
            cancellationToken);
        return ApiResponse<FgsInventoryItemDetailDto>.Ok(result);
    }
}
