using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.TruckStockTemplateItems;
using Fgs.Inventory.Application.Features.TruckStockTemplateItems.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Inventory.Application.Features.TruckStockTemplateItems.Commands.CreateFgsTruckStockTemplateItem;

public sealed class CreateFgsTruckStockTemplateItemCommandHandler(
    IFgsTruckStockTemplateItemWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsTruckStockTemplateItemCommandHandler> logger)
    : IRequestHandler<CreateFgsTruckStockTemplateItemCommand, ApiResponse<FgsTruckStockTemplateItemDetailDto>>
{
    public async Task<ApiResponse<FgsTruckStockTemplateItemDetailDto>> Handle(
        CreateFgsTruckStockTemplateItemCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.TemplateId, request.Dto, cancellationToken);
        logger.LogInformation(
            "Created truck stock template item {Id} on template {TemplateId}",
            result.Id,
            result.TruckStockTemplateId);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "truck-stock-template"),
            cancellationToken);
        return ApiResponse<FgsTruckStockTemplateItemDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
