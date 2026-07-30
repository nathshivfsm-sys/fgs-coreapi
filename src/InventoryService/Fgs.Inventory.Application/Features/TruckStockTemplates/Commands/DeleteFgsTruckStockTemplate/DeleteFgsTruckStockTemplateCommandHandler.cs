using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.TruckStockTemplates;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Inventory.Application.Features.TruckStockTemplates.Commands.DeleteFgsTruckStockTemplate;

public sealed class DeleteFgsTruckStockTemplateCommandHandler(
    IFgsTruckStockTemplateWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<DeleteFgsTruckStockTemplateCommandHandler> logger)
    : IRequestHandler<DeleteFgsTruckStockTemplateCommand, ApiResponse<FgsTruckStockTemplateDetailDto>>
{
    public async Task<ApiResponse<FgsTruckStockTemplateDetailDto>> Handle(
        DeleteFgsTruckStockTemplateCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.DeleteAsync(request.Id, cancellationToken);
        logger.LogInformation("Soft-deleted truck stock template {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "truck-stock-template"),
            cancellationToken);
        return ApiResponse<FgsTruckStockTemplateDetailDto>.Ok(result);
    }
}
