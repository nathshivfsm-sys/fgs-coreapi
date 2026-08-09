using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.InventoryStocks;
using Fgs.Inventory.Application.Features.InventoryStocks.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Inventory.Application.Features.InventoryStocks.Commands.UpdateFgsInventoryStock;

public sealed class UpdateFgsInventoryStockCommandHandler(
    IFgsInventoryStockWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateFgsInventoryStockCommandHandler> logger)
    : IRequestHandler<UpdateFgsInventoryStockCommand, ApiResponse<FgsInventoryStockDetailDto>>
{
    public async Task<ApiResponse<FgsInventoryStockDetailDto>> Handle(
        UpdateFgsInventoryStockCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated inventory stock {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "inventorystock"),
            cancellationToken);
        return ApiResponse<FgsInventoryStockDetailDto>.Ok(result);
    }
}
