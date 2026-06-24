using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.CatalogCrud;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.JobTypeSubCategories;
using Fgs.Setup.Application.Features.JobTypeSubCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypeSubCategories.Queries.GetJobTypeSubCategoryById;

public sealed class GetJobTypeSubCategoryByIdQueryHandler(
    IJobTypeSubCategoryReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetJobTypeSubCategoryByIdQuery, ApiResponse<JobTypeSubCategoryDetailDto>>
{
    public async Task<ApiResponse<JobTypeSubCategoryDetailDto>> Handle(
        GetJobTypeSubCategoryByIdQuery request,
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
                    "jobtypesubcategories",
                    request.Id.ToString());

                var cached = await cache.GetAsync<JobTypeSubCategoryDetailDto>(cacheKey, cancellationToken);
                if (cached is not null)
                {
                    return ApiResponse<JobTypeSubCategoryDetailDto>.Ok(cached);
                }

                var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
                if (result is null)
                {
                    return ApiResponse<JobTypeSubCategoryDetailDto>.Fail(
                        [$"Job Type Subcategory '{request.Id}' was not found."],
                        ApiStatusCodes.NotFound);
                }

                await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
                return ApiResponse<JobTypeSubCategoryDetailDto>.Ok(result);
            }

            var uncached = await readRepository.GetByIdAsync(request.Id, cancellationToken);
            if (uncached is null)
            {
                return ApiResponse<JobTypeSubCategoryDetailDto>.Fail(
                    [$"Job Type Subcategory '{request.Id}' was not found."],
                    ApiStatusCodes.NotFound);
            }

            return ApiResponse<JobTypeSubCategoryDetailDto>.Ok(uncached);
        }
        catch (Exception ex)
        {
            return CatalogCrudExceptionMapper.MapException<JobTypeSubCategoryDetailDto>(ex);
        }
    }
}
