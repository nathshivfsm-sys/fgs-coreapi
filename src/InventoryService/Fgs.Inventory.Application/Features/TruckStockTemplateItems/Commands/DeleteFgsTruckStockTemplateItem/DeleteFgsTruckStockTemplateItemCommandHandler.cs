using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.TruckStockTemplateItems;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Inventory.Application.Features.TruckStockTemplateItems.Commands.DeleteFgsTruckStockTemplateItem;

public sealed class DeleteFgsTruckStockTemplateItemCommandHandler(
    IFgsTruckStockTemplateItemWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<DeleteFgsTruckStockTemplateItemCommandHandler> logger)
    : IRequestHandler<DeleteFgsTruckStockTemplateItemCommand, ApiResponse<object>>
{
    public async Task<ApiResponse<object>> Handle(
        DeleteFgsTruckStockTemplateItemCommand request,
        CancellationToken cancellationToken)
    {
        await writeService.DeleteAsync(request.TemplateId, request.ItemId, cancellationToken);
        logger.LogInformation(
            "Deleted truck stock template item {ItemId} from template {TemplateId}",
            request.ItemId,
            request.TemplateId);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "truck-stock-template"),
            cancellationToken);
        return ApiResponse<object>.Ok(new object());
    }
}
