using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.TruckStockTemplates;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Inventory.Application.Features.TruckStockTemplates.Commands.UpdateFgsTruckStockTemplate;

public sealed class UpdateFgsTruckStockTemplateCommandHandler(
    IFgsTruckStockTemplateWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateFgsTruckStockTemplateCommandHandler> logger)
    : IRequestHandler<UpdateFgsTruckStockTemplateCommand, ApiResponse<FgsTruckStockTemplateDetailDto>>
{
    public async Task<ApiResponse<FgsTruckStockTemplateDetailDto>> Handle(
        UpdateFgsTruckStockTemplateCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated truck stock template {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "truckstocktemplate"),
            cancellationToken);
        return ApiResponse<FgsTruckStockTemplateDetailDto>.Ok(result);
    }
}
