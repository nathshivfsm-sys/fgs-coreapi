using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.CatalogCrud;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.FgsBusinessTypes;
using Fgs.Setup.Application.Features.FgsBusinessTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.FgsBusinessTypes.Queries.GetFgsBusinessTypeById;

public sealed class GetFgsBusinessTypeByIdQueryHandler(
    IFgsBusinessTypeReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsBusinessTypeByIdQuery, ApiResponse<FgsBusinessTypeDetailDto>>
{
    public async Task<ApiResponse<FgsBusinessTypeDetailDto>> Handle(
        GetFgsBusinessTypeByIdQuery request,
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
                    "businesstypes",
                    request.Id.ToString());

                var cached = await cache.GetAsync<FgsBusinessTypeDetailDto>(cacheKey, cancellationToken);
                if (cached is not null)
                {
                    return ApiResponse<FgsBusinessTypeDetailDto>.Ok(cached);
                }

                var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
                if (result is null)
                {
                    return ApiResponse<FgsBusinessTypeDetailDto>.Fail(
                        [$"Business Type '{request.Id}' was not found."],
                        ApiStatusCodes.NotFound);
                }

                await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
                return ApiResponse<FgsBusinessTypeDetailDto>.Ok(result);
            }

            var uncached = await readRepository.GetByIdAsync(request.Id, cancellationToken);
            if (uncached is null)
            {
                return ApiResponse<FgsBusinessTypeDetailDto>.Fail(
                    [$"Business Type '{request.Id}' was not found."],
                    ApiStatusCodes.NotFound);
            }

            return ApiResponse<FgsBusinessTypeDetailDto>.Ok(uncached);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<FgsBusinessTypeDetailDto>(ex);
        }
    }
}
