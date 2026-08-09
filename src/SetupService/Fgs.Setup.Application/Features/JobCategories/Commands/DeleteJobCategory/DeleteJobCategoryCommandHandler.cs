using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.JobCategories;
using Fgs.Setup.Application.Features.JobCategories.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.JobCategories.Commands.DeleteJobCategory;

public sealed class DeleteJobCategoryCommandHandler(
    IJobCategoryWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<DeleteJobCategoryCommandHandler> logger)
    : IRequestHandler<DeleteJobCategoryCommand, ApiResponse<JobCategoryDetailDto>>
{
    public async Task<ApiResponse<JobCategoryDetailDto>> Handle(
        DeleteJobCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.DeleteAsync(request.Id, cancellationToken);
        logger.LogInformation("Soft-deleted job category {Id}", result.Id);
            var tenantScope = tenantContextAccessor.Current!;
            await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "jobcategory"),
                cancellationToken);
        return ApiResponse<JobCategoryDetailDto>.Ok(result);
    }
}
