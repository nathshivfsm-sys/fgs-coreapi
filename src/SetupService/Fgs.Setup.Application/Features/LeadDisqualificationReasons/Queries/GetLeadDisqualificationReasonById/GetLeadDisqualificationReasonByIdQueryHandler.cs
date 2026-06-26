using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.LeadDisqualificationReasons;
using Fgs.Setup.Application.Features.LeadDisqualificationReasons.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.LeadDisqualificationReasons.Queries.GetLeadDisqualificationReasonById;

public sealed class GetLeadDisqualificationReasonByIdQueryHandler(
    ILeadDisqualificationReasonReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetLeadDisqualificationReasonByIdQuery, ApiResponse<LeadDisqualificationReasonDetailDto>>
{
    public async Task<ApiResponse<LeadDisqualificationReasonDetailDto>> Handle(
        GetLeadDisqualificationReasonByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "leaddisqualificationreasons",
            request.Id.ToString());

        var cached = await cache.GetAsync<LeadDisqualificationReasonDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<LeadDisqualificationReasonDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<LeadDisqualificationReasonDetailDto>.Fail(
                [$"Lead Disqualification Reason '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<LeadDisqualificationReasonDetailDto>.Ok(result);
    }
}
