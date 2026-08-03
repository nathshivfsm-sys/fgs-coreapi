using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.VendorInventoryItems;
using Fgs.Inventory.Application.Features.VendorInventoryItems.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.VendorInventoryItems.Queries.GetFgsVendorInventoryItemById;

public sealed class GetFgsVendorInventoryItemByIdQueryHandler(
    IFgsVendorInventoryItemReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsVendorInventoryItemByIdQuery, ApiResponse<FgsVendorInventoryItemDetailDto>>
{
    public async Task<ApiResponse<FgsVendorInventoryItemDetailDto>> Handle(
        GetFgsVendorInventoryItemByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "vendorinventoryitem",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsVendorInventoryItemDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsVendorInventoryItemDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsVendorInventoryItemDetailDto>.Fail(
                [$"Vendor inventory item '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsVendorInventoryItemDetailDto>.Ok(result);
    }
}
