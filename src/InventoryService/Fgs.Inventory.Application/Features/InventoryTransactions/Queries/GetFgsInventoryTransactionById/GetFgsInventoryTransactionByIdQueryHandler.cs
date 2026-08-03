using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.InventoryTransactions;
using Fgs.Inventory.Application.Features.InventoryTransactions.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventoryTransactions.Queries.GetFgsInventoryTransactionById;

public sealed class GetFgsInventoryTransactionByIdQueryHandler(
    IFgsInventoryTransactionReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsInventoryTransactionByIdQuery, ApiResponse<FgsInventoryTransactionDetailDto>>
{
    public async Task<ApiResponse<FgsInventoryTransactionDetailDto>> Handle(
        GetFgsInventoryTransactionByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "inventorytransaction",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsInventoryTransactionDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsInventoryTransactionDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsInventoryTransactionDetailDto>.Fail(
                [$"Inventory transaction '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsInventoryTransactionDetailDto>.Ok(result);
    }
}
