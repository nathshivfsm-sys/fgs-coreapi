using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.JobCategories;
using Fgs.Setup.Application.Features.JobCategories.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.JobCategories.Commands.UpdateJobCategory;

public sealed class UpdateJobCategoryCommandHandler(
    IJobCategoryWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateJobCategoryCommandHandler> logger)
    : IRequestHandler<UpdateJobCategoryCommand, ApiResponse<JobCategoryDetailDto>>
{
    public async Task<ApiResponse<JobCategoryDetailDto>> Handle(
        UpdateJobCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated job category {Id}", result.Id);
            var tenantScope = tenantContextAccessor.Current!;
            await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "jobcategory"),
                cancellationToken);
        return ApiResponse<JobCategoryDetailDto>.Ok(result);
    }
}
