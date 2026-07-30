using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.TruckStockTemplates;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.TruckStockTemplates.Queries.LookupTruckStockTemplates;

public sealed class LookupTruckStockTemplatesQueryHandler(
    IFgsTruckStockTemplateReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupTruckStockTemplatesQuery, ApiResponse<IReadOnlyList<FgsTruckStockTemplateLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsTruckStockTemplateLookupDto>>> Handle(
        LookupTruckStockTemplatesQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "truck-stock-template",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<FgsTruckStockTemplateLookupDto>>.Ok(
            result ?? Array.Empty<FgsTruckStockTemplateLookupDto>());
    }
}
