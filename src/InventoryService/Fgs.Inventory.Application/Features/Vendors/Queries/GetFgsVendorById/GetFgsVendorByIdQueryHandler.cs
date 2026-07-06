using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Inventory.Application.Abstractions.Vendors;
using Fgs.Inventory.Application.Features.Vendors.Dtos;
using MediatR;

namespace Fgs.Inventory.Application.Features.Vendors.Queries.GetFgsVendorById;

public sealed class GetFgsVendorByIdQueryHandler(
    IFgsVendorReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsVendorByIdQuery, ApiResponse<FgsVendorDetailDto>>
{
    public async Task<ApiResponse<FgsVendorDetailDto>> Handle(
        GetFgsVendorByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "vendor",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsVendorDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsVendorDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsVendorDetailDto>.Fail(
                [$"Vendor '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsVendorDetailDto>.Ok(result);
    }
}
