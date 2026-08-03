using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Inventory.Application.Abstractions.InventorySerials;
using Fgs.Inventory.Application.Features.InventorySerials.Dtos;
using Fgs.MultiTenancy;
using MediatR;

namespace Fgs.Inventory.Application.Features.InventorySerials.Queries.GetFgsInventorySerialById;

public sealed class GetFgsInventorySerialByIdQueryHandler(
    IFgsInventorySerialReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsInventorySerialByIdQuery, ApiResponse<FgsInventorySerialDetailDto>>
{
    public async Task<ApiResponse<FgsInventorySerialDetailDto>> Handle(
        GetFgsInventorySerialByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "inventoryserial",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsInventorySerialDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsInventorySerialDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsInventorySerialDetailDto>.Fail(
                [$"Inventory serial '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsInventorySerialDetailDto>.Ok(result);
    }
}
