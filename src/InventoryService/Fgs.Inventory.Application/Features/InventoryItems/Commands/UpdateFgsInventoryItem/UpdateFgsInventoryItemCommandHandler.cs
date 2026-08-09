using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.InventoryItems;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Inventory.Application.Features.InventoryItems.Commands.UpdateFgsInventoryItem;

public sealed class UpdateFgsInventoryItemCommandHandler(
    IFgsInventoryItemWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateFgsInventoryItemCommandHandler> logger)
    : IRequestHandler<UpdateFgsInventoryItemCommand, ApiResponse<FgsInventoryItemDetailDto>>
{
    public async Task<ApiResponse<FgsInventoryItemDetailDto>> Handle(
        UpdateFgsInventoryItemCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated inventory item {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "inventoryitem"),
            cancellationToken);
        return ApiResponse<FgsInventoryItemDetailDto>.Ok(result);
    }
}
