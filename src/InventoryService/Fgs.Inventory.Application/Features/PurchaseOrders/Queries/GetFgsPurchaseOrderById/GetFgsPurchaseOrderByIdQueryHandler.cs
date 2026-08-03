using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.PurchaseOrders;
using Fgs.Inventory.Application.Features.PurchaseOrders.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.PurchaseOrders.Queries.GetFgsPurchaseOrderById;

public sealed class GetFgsPurchaseOrderByIdQueryHandler(
    IFgsPurchaseOrderReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsPurchaseOrderByIdQuery, ApiResponse<FgsPurchaseOrderDetailDto>>
{
    public async Task<ApiResponse<FgsPurchaseOrderDetailDto>> Handle(
        GetFgsPurchaseOrderByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "purchaseorder",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsPurchaseOrderDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsPurchaseOrderDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsPurchaseOrderDetailDto>.Fail(
                [$"Purchase order '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsPurchaseOrderDetailDto>.Ok(result);
    }
}
