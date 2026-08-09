using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.InventoryCategories;
using Fgs.Inventory.Application.Features.InventoryCategories.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Inventory.Application.Features.InventoryCategories.Commands.UpdateFgsInventoryCategory;

public sealed class UpdateFgsInventoryCategoryCommandHandler(
    IFgsInventoryCategoryWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateFgsInventoryCategoryCommandHandler> logger)
    : IRequestHandler<UpdateFgsInventoryCategoryCommand, ApiResponse<FgsInventoryCategoryDetailDto>>
{
    public async Task<ApiResponse<FgsInventoryCategoryDetailDto>> Handle(
        UpdateFgsInventoryCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated inventory category {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "inventorycategory"),
            cancellationToken);
        return ApiResponse<FgsInventoryCategoryDetailDto>.Ok(result);
    }
}
