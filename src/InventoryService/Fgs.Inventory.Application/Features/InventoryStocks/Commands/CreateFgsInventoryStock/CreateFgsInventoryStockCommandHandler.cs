using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.InventoryStocks;
using Fgs.Inventory.Application.Features.InventoryStocks.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Inventory.Application.Features.InventoryStocks.Commands.CreateFgsInventoryStock;

public sealed class CreateFgsInventoryStockCommandHandler(
    IFgsInventoryStockWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsInventoryStockCommandHandler> logger)
    : IRequestHandler<CreateFgsInventoryStockCommand, ApiResponse<FgsInventoryStockDetailDto>>
{
    public async Task<ApiResponse<FgsInventoryStockDetailDto>> Handle(
        CreateFgsInventoryStockCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created inventory stock {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "inventorystock"),
            cancellationToken);
        return ApiResponse<FgsInventoryStockDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
