using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.EntityDefaultTermsConditions;
using Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Queries.GetFgsEntityDefaultTermsConditionById;

public sealed class GetFgsEntityDefaultTermsConditionByIdQueryHandler(
    IFgsEntityDefaultTermsConditionReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsEntityDefaultTermsConditionByIdQuery, ApiResponse<FgsEntityDefaultTermsConditionDetailDto>>
{
    public async Task<ApiResponse<FgsEntityDefaultTermsConditionDetailDto>> Handle(
        GetFgsEntityDefaultTermsConditionByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "entitydefaulttermscondition",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsEntityDefaultTermsConditionDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsEntityDefaultTermsConditionDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsEntityDefaultTermsConditionDetailDto>.Fail(
                [$"Entity default terms condition '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsEntityDefaultTermsConditionDetailDto>.Ok(result);
    }
}
