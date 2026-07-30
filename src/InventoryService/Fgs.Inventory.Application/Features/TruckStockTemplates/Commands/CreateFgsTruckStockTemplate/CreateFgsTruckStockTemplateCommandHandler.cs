using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.TruckStockTemplates;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Inventory.Application.Features.TruckStockTemplates.Commands.CreateFgsTruckStockTemplate;

public sealed class CreateFgsTruckStockTemplateCommandHandler(
    IFgsTruckStockTemplateWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsTruckStockTemplateCommandHandler> logger)
    : IRequestHandler<CreateFgsTruckStockTemplateCommand, ApiResponse<FgsTruckStockTemplateDetailDto>>
{
    public async Task<ApiResponse<FgsTruckStockTemplateDetailDto>> Handle(
        CreateFgsTruckStockTemplateCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created truck stock template {Id} with code {TemplateCode}", result.Id, result.TemplateCode);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "truck-stock-template"),
            cancellationToken);
        return ApiResponse<FgsTruckStockTemplateDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
