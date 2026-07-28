using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.JobCategories;
using Fgs.Setup.Application.Features.JobCategories.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.JobCategories.Commands.CreateJobCategory;

public sealed class CreateJobCategoryCommandHandler(
    IJobCategoryWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateJobCategoryCommandHandler> logger)
    : IRequestHandler<CreateJobCategoryCommand, ApiResponse<JobCategoryDetailDto>>
{
    public async Task<ApiResponse<JobCategoryDetailDto>> Handle(
        CreateJobCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation("Created job category {Id} with code {CategoryCode}", result.Id, result.CategoryCode);
            var tenantScope = tenantContextAccessor.Current!;
            await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "jobcategory"),
                cancellationToken);
        return ApiResponse<JobCategoryDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
