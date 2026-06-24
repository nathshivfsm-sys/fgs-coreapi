using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.CatalogCrud;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Vehicles;
using Fgs.Setup.Application.Features.Vehicles.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Vehicles.Queries.GetFgsVehicleById;

public sealed class GetFgsVehicleByIdQueryHandler(
    IFgsVehicleReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsVehicleByIdQuery, ApiResponse<FgsVehicleDetailDto>>
{
    public async Task<ApiResponse<FgsVehicleDetailDto>> Handle(
        GetFgsVehicleByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var tenantScope = tenantContextAccessor.Current;
            if (tenantScope?.IsResolved == true)
            {
                var cacheKey = CacheKeys.Build(
                    tenantScope.TenantId,
                    tenantScope.CompanyId,
                    "vehicles",
                    request.Id.ToString());

                var cached = await cache.GetAsync<FgsVehicleDetailDto>(cacheKey, cancellationToken);
                if (cached is not null)
                {
                    return ApiResponse<FgsVehicleDetailDto>.Ok(cached);
                }

                var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
                if (result is null)
                {
                    return ApiResponse<FgsVehicleDetailDto>.Fail(
                        [$"Vehicle '{request.Id}' was not found."],
                        ApiStatusCodes.NotFound);
                }

                await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
                return ApiResponse<FgsVehicleDetailDto>.Ok(result);
            }

            var uncached = await readRepository.GetByIdAsync(request.Id, cancellationToken);
            if (uncached is null)
            {
                return ApiResponse<FgsVehicleDetailDto>.Fail(
                    [$"Vehicle '{request.Id}' was not found."],
                    ApiStatusCodes.NotFound);
            }

            return ApiResponse<FgsVehicleDetailDto>.Ok(uncached);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<FgsVehicleDetailDto>(ex);
        }
    }
}
