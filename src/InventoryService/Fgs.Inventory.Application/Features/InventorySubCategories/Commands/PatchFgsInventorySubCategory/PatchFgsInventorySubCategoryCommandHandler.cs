using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.InventorySubCategories;
using Fgs.Inventory.Application.Features.InventorySubCategories.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Inventory.Application.Features.InventorySubCategories.Commands.PatchFgsInventorySubCategory;

public sealed class PatchFgsInventorySubCategoryCommandHandler(
    IFgsInventorySubCategoryWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchFgsInventorySubCategoryCommandHandler> logger)
    : IRequestHandler<PatchFgsInventorySubCategoryCommand, ApiResponse<FgsInventorySubCategoryDetailDto>>
{
    public async Task<ApiResponse<FgsInventorySubCategoryDetailDto>> Handle(
        PatchFgsInventorySubCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patched inventory sub-category {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "inventorysubcategory"),
            cancellationToken);
        return ApiResponse<FgsInventorySubCategoryDetailDto>.Ok(result);
    }
}
