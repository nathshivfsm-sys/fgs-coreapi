using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.InventorySubCategories;
using Fgs.Inventory.Application.Features.InventorySubCategories.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Inventory.Application.Features.InventorySubCategories.Commands.CreateFgsInventorySubCategory;

public sealed class CreateFgsInventorySubCategoryCommandHandler(
    IFgsInventorySubCategoryWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsInventorySubCategoryCommandHandler> logger)
    : IRequestHandler<CreateFgsInventorySubCategoryCommand, ApiResponse<FgsInventorySubCategoryDetailDto>>
{
    public async Task<ApiResponse<FgsInventorySubCategoryDetailDto>> Handle(
        CreateFgsInventorySubCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created inventory sub-category {Id} with code {SubCategoryCode}", result.Id, result.SubCategoryCode);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "inventorysubcategory"),
            cancellationToken);
        return ApiResponse<FgsInventorySubCategoryDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
