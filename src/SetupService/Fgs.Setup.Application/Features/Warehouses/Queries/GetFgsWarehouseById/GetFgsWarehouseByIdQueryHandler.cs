using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Warehouses;
using Fgs.Setup.Application.Features.Warehouses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Warehouses.Queries.GetFgsWarehouseById;

public sealed class GetFgsWarehouseByIdQueryHandler(
    IFgsWarehouseReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsWarehouseByIdQuery, ApiResponse<FgsWarehouseDetailDto>>
{
    public async Task<ApiResponse<FgsWarehouseDetailDto>> Handle(
        GetFgsWarehouseByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "warehouses",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsWarehouseDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsWarehouseDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsWarehouseDetailDto>.Fail(
                [$"Warehouse '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsWarehouseDetailDto>.Ok(result);
    }
}
