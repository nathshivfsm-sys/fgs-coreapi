using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.PurchaseOrders;
using Fgs.Inventory.Application.Features.PurchaseOrders.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Inventory.Application.Features.PurchaseOrders.Commands.CreateFgsPurchaseOrder;

public sealed class CreateFgsPurchaseOrderCommandHandler(
    IFgsPurchaseOrderWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsPurchaseOrderCommandHandler> logger)
    : IRequestHandler<CreateFgsPurchaseOrderCommand, ApiResponse<FgsPurchaseOrderDetailDto>>
{
    public async Task<ApiResponse<FgsPurchaseOrderDetailDto>> Handle(
        CreateFgsPurchaseOrderCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation(
            "Created purchase order {Id} with number {PurchaseOrderNumber}",
            result.Id,
            result.PurchaseOrderNumber);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "purchaseorder"),
            cancellationToken);
        return ApiResponse<FgsPurchaseOrderDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
