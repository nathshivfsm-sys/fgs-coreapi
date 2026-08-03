using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.PurchaseOrders;
using Fgs.Inventory.Application.Features.PurchaseOrders.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Inventory.Application.Features.PurchaseOrders.Commands.UpdateFgsPurchaseOrder;

public sealed class UpdateFgsPurchaseOrderCommandHandler(
    IFgsPurchaseOrderWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateFgsPurchaseOrderCommandHandler> logger)
    : IRequestHandler<UpdateFgsPurchaseOrderCommand, ApiResponse<FgsPurchaseOrderDetailDto>>
{
    public async Task<ApiResponse<FgsPurchaseOrderDetailDto>> Handle(
        UpdateFgsPurchaseOrderCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated purchase order {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "purchaseorder"),
            cancellationToken);
        return ApiResponse<FgsPurchaseOrderDetailDto>.Ok(result);
    }
}
