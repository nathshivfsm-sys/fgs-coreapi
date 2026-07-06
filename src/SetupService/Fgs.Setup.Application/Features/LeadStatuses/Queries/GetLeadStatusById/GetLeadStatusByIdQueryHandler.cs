using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.LeadStatuses;
using Fgs.Setup.Application.Features.LeadStatuses.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadStatuses.Queries.GetLeadStatusById;

public sealed class GetLeadStatusByIdQueryHandler(
    ILeadStatusReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetLeadStatusByIdQuery, ApiResponse<LeadStatusDetailDto>>
{
    public async Task<ApiResponse<LeadStatusDetailDto>> Handle(
        GetLeadStatusByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "leadstatus",
            request.Id.ToString());

        var cached = await cache.GetAsync<LeadStatusDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<LeadStatusDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<LeadStatusDetailDto>.Fail(
                [$"Lead Status '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<LeadStatusDetailDto>.Ok(result);
    }
}
