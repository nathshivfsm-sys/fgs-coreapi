using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.InventorySubCategories;
using Fgs.Inventory.Application.Features.InventorySubCategories.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Inventory.Application.Features.InventorySubCategories.Commands.UpdateFgsInventorySubCategory;

public sealed class UpdateFgsInventorySubCategoryCommandHandler(
    IFgsInventorySubCategoryWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateFgsInventorySubCategoryCommandHandler> logger)
    : IRequestHandler<UpdateFgsInventorySubCategoryCommand, ApiResponse<FgsInventorySubCategoryDetailDto>>
{
    public async Task<ApiResponse<FgsInventorySubCategoryDetailDto>> Handle(
        UpdateFgsInventorySubCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated inventory sub-category {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "inventorysubcategory"),
            cancellationToken);
        return ApiResponse<FgsInventorySubCategoryDetailDto>.Ok(result);
    }
}
