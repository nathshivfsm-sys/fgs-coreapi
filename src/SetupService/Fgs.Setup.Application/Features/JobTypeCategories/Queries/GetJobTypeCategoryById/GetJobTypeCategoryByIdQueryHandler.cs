using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.JobTypeCategories;
using Fgs.Setup.Application.Features.JobTypeCategories.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypeCategories.Queries.GetJobTypeCategoryById;

public sealed class GetJobTypeCategoryByIdQueryHandler(
    IJobTypeCategoryReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetJobTypeCategoryByIdQuery, ApiResponse<JobTypeCategoryDetailDto>>
{
    public async Task<ApiResponse<JobTypeCategoryDetailDto>> Handle(
        GetJobTypeCategoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "jobtypecategory",
            request.Id.ToString());

        var cached = await cache.GetAsync<JobTypeCategoryDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<JobTypeCategoryDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<JobTypeCategoryDetailDto>.Fail(
                [$"Job Type Category '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<JobTypeCategoryDetailDto>.Ok(result);
    }
}
