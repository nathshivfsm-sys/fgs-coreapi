using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.TermsConditions;
using Fgs.Setup.Application.Features.TermsConditions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.TermsConditions.Queries.GetFgsTermsConditionById;

public sealed class GetFgsTermsConditionByIdQueryHandler(
    IFgsTermsConditionReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsTermsConditionByIdQuery, ApiResponse<FgsTermsConditionDetailDto>>
{
    public async Task<ApiResponse<FgsTermsConditionDetailDto>> Handle(
        GetFgsTermsConditionByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "termscondition",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsTermsConditionDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsTermsConditionDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsTermsConditionDetailDto>.Fail(
                [$"Terms condition '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsTermsConditionDetailDto>.Ok(result);
    }
}
