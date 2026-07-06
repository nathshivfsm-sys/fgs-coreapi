using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.JobTypeCategories;
using Fgs.Setup.Application.Features.JobTypeCategories.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Setup.Application.Features.JobTypeCategories.Commands.DeleteJobTypeCategory;

public sealed class DeleteJobTypeCategoryCommandHandler(
    IJobTypeCategoryWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<DeleteJobTypeCategoryCommandHandler> logger)
    : IRequestHandler<DeleteJobTypeCategoryCommand, ApiResponse<JobTypeCategoryDetailDto>>
{
    public async Task<ApiResponse<JobTypeCategoryDetailDto>> Handle(
        DeleteJobTypeCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.DeleteAsync(request.Id, cancellationToken);
        logger.LogInformation("Soft-deleted job type category {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
                CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "jobtypecategory"),
                cancellationToken);
        return ApiResponse<JobTypeCategoryDetailDto>.Ok(result);
    }
}
