using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.TruckStockTemplateItems;
using Fgs.Inventory.Application.Features.TruckStockTemplateItems.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Inventory.Application.Features.TruckStockTemplateItems.Commands.PatchFgsTruckStockTemplateItem;

public sealed class PatchFgsTruckStockTemplateItemCommandHandler(
    IFgsTruckStockTemplateItemWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchFgsTruckStockTemplateItemCommandHandler> logger)
    : IRequestHandler<PatchFgsTruckStockTemplateItemCommand, ApiResponse<FgsTruckStockTemplateItemDetailDto>>
{
    public async Task<ApiResponse<FgsTruckStockTemplateItemDetailDto>> Handle(
        PatchFgsTruckStockTemplateItemCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.TemplateId, request.ItemId, request.Dto, cancellationToken);
        logger.LogInformation("Patched truck stock template item {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "truck-stock-template"),
            cancellationToken);
        return ApiResponse<FgsTruckStockTemplateItemDetailDto>.Ok(result);
    }
}
