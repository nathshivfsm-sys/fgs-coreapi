using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.JobCategories;
using Fgs.Setup.Application.Features.JobCategories.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.JobCategories.Commands.PatchJobCategory;

public sealed class PatchJobCategoryCommandHandler(
    IJobCategoryWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchJobCategoryCommandHandler> logger)
    : IRequestHandler<PatchJobCategoryCommand, ApiResponse<JobCategoryDetailDto>>
{
    public async Task<ApiResponse<JobCategoryDetailDto>> Handle(
        PatchJobCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patchd job category {Id}", result.Id);
            var tenantScope = tenantContextAccessor.Current!;
            await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "jobcategory"),
                cancellationToken);
        return ApiResponse<JobCategoryDetailDto>.Ok(result);
    }
}
