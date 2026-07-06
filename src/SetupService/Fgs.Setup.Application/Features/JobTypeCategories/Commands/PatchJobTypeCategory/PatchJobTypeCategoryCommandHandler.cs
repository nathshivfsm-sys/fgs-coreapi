using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.JobTypeCategories;
using Fgs.Setup.Application.Features.JobTypeCategories.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.JobTypeCategories.Commands.PatchJobTypeCategory;

public sealed class PatchJobTypeCategoryCommandHandler(
    IJobTypeCategoryWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchJobTypeCategoryCommandHandler> logger)
    : IRequestHandler<PatchJobTypeCategoryCommand, ApiResponse<JobTypeCategoryDetailDto>>
{
    public async Task<ApiResponse<JobTypeCategoryDetailDto>> Handle(
        PatchJobTypeCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patchd job type category {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "jobtypecategory"),
                cancellationToken);
        return ApiResponse<JobTypeCategoryDetailDto>.Ok(result);
    }
}
