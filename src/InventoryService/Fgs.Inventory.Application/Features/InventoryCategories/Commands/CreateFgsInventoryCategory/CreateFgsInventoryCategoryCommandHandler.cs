using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.InventoryCategories;
using Fgs.Inventory.Application.Features.InventoryCategories.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Inventory.Application.Features.InventoryCategories.Commands.CreateFgsInventoryCategory;

public sealed class CreateFgsInventoryCategoryCommandHandler(
    IFgsInventoryCategoryWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsInventoryCategoryCommandHandler> logger)
    : IRequestHandler<CreateFgsInventoryCategoryCommand, ApiResponse<FgsInventoryCategoryDetailDto>>
{
    public async Task<ApiResponse<FgsInventoryCategoryDetailDto>> Handle(
        CreateFgsInventoryCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created inventory category {Id} with code {CategoryCode}", result.Id, result.CategoryCode);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "inventorycategory"),
            cancellationToken);
        return ApiResponse<FgsInventoryCategoryDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
