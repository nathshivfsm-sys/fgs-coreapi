using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Inventory.Application.Abstractions.InventorySerials;
using Fgs.Inventory.Application.Features.InventorySerials.Dtos;
using Fgs.MultiTenancy;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventorySerials.Queries.LookupInventorySerials;

public sealed class LookupInventorySerialsQueryHandler(
    IFgsInventorySerialReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupInventorySerialsQuery, ApiResponse<IReadOnlyList<FgsInventorySerialLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsInventorySerialLookupDto>>> Handle(
        LookupInventorySerialsQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "inventoryserial",
            $"lookup:inventoryItemId={request.InventoryItemId?.ToString() ?? "all"}");

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.InventoryItemId, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<FgsInventorySerialLookupDto>>.Ok(result ?? Array.Empty<FgsInventorySerialLookupDto>());
    }
}
