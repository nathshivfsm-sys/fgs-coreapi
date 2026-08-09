using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.JobCategories;
using Fgs.Setup.Application.Features.JobCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobCategories.Queries.GetJobCategoryById;

public sealed class GetJobCategoryByIdQueryHandler(
    IJobCategoryReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetJobCategoryByIdQuery, ApiResponse<JobCategoryDetailDto>>
{
    public async Task<ApiResponse<JobCategoryDetailDto>> Handle(
        GetJobCategoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "jobcategory",
            request.Id.ToString());

        var cached = await cache.GetAsync<JobCategoryDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<JobCategoryDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<JobCategoryDetailDto>.Fail(
                [$"Job Category '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<JobCategoryDetailDto>.Ok(result);
    }
}
