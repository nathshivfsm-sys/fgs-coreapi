using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.PurchaseOrders;
using Fgs.Inventory.Application.Features.PurchaseOrders.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Inventory.Application.Features.PurchaseOrders.Commands.PatchFgsPurchaseOrder;

public sealed class PatchFgsPurchaseOrderCommandHandler(
    IFgsPurchaseOrderWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchFgsPurchaseOrderCommandHandler> logger)
    : IRequestHandler<PatchFgsPurchaseOrderCommand, ApiResponse<FgsPurchaseOrderDetailDto>>
{
    public async Task<ApiResponse<FgsPurchaseOrderDetailDto>> Handle(
        PatchFgsPurchaseOrderCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patched purchase order {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "purchaseorder"),
            cancellationToken);
        return ApiResponse<FgsPurchaseOrderDetailDto>.Ok(result);
    }
}
