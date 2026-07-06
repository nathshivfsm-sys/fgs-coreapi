using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
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
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "jobtypesubcategory",
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
}
