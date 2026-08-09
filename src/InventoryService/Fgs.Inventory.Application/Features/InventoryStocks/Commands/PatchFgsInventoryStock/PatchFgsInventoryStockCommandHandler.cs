using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.InventoryStocks;
using Fgs.Inventory.Application.Features.InventoryStocks.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Inventory.Application.Features.InventoryStocks.Commands.PatchFgsInventoryStock;

public sealed class PatchFgsInventoryStockCommandHandler(
    IFgsInventoryStockWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchFgsInventoryStockCommandHandler> logger)
    : IRequestHandler<PatchFgsInventoryStockCommand, ApiResponse<FgsInventoryStockDetailDto>>
{
    public async Task<ApiResponse<FgsInventoryStockDetailDto>> Handle(
        PatchFgsInventoryStockCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patched inventory stock {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "inventorystock"),
            cancellationToken);
        return ApiResponse<FgsInventoryStockDetailDto>.Ok(result);
    }
}
