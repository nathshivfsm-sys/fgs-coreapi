using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.JobTypeSubCategories;
using Fgs.Setup.Application.Features.JobTypeSubCategories.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.JobTypeSubCategories.Commands.UpdateJobTypeSubCategory;

public sealed class UpdateJobTypeSubCategoryCommandHandler(
    IJobTypeSubCategoryWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateJobTypeSubCategoryCommandHandler> logger)
    : IRequestHandler<UpdateJobTypeSubCategoryCommand, ApiResponse<JobTypeSubCategoryDetailDto>>
{
    public async Task<ApiResponse<JobTypeSubCategoryDetailDto>> Handle(
        UpdateJobTypeSubCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated job type subcategory {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "jobtypesubcategory"),
                cancellationToken);
        return ApiResponse<JobTypeSubCategoryDetailDto>.Ok(result);
    }
}
