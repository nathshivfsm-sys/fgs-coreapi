using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.JobTypeTasks;
using Fgs.Setup.Application.Features.JobTypeTasks.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.JobTypeTasks.Queries.LookupJobTypeTasks;

public sealed class LookupJobTypeTasksQueryHandler(
    IJobTypeTaskReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupJobTypeTasksQuery, ApiResponse<IReadOnlyList<JobTypeTaskLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<JobTypeTaskLookupDto>>> Handle(
        LookupJobTypeTasksQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "jobtypetask",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<JobTypeTaskLookupDto>>.Ok(result ?? Array.Empty<JobTypeTaskLookupDto>());
    }
}
