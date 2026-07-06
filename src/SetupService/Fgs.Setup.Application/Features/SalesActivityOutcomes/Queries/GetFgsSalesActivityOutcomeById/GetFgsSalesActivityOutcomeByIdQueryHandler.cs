using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SalesActivityOutcomes;
using Fgs.Setup.Application.Features.SalesActivityOutcomes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SalesActivityOutcomes.Queries.GetFgsSalesActivityOutcomeById;

public sealed class GetFgsSalesActivityOutcomeByIdQueryHandler(
    IFgsSalesActivityOutcomeReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsSalesActivityOutcomeByIdQuery, ApiResponse<FgsSalesActivityOutcomeDetailDto>>
{
    public async Task<ApiResponse<FgsSalesActivityOutcomeDetailDto>> Handle(
        GetFgsSalesActivityOutcomeByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "salesactivityoutcome",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsSalesActivityOutcomeDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsSalesActivityOutcomeDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsSalesActivityOutcomeDetailDto>.Fail(
                [$"Sales Activity Outcome '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsSalesActivityOutcomeDetailDto>.Ok(result);
    }
}
