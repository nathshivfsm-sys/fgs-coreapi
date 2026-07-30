using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.TruckStockTemplates;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.TruckStockTemplates.Queries.GetFgsTruckStockTemplateById;

public sealed class GetFgsTruckStockTemplateByIdQueryHandler(
    IFgsTruckStockTemplateReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsTruckStockTemplateByIdQuery, ApiResponse<FgsTruckStockTemplateDetailDto>>
{
    public async Task<ApiResponse<FgsTruckStockTemplateDetailDto>> Handle(
        GetFgsTruckStockTemplateByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "truck-stock-template",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsTruckStockTemplateDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsTruckStockTemplateDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsTruckStockTemplateDetailDto>.Fail(
                [$"Truck stock template '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsTruckStockTemplateDetailDto>.Ok(result);
    }
}
