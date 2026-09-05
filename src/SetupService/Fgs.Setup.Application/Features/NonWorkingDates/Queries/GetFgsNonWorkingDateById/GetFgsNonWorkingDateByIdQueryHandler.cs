using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.NonWorkingDates;
using Fgs.Setup.Application.Features.NonWorkingDates.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.NonWorkingDates.Queries.GetFgsNonWorkingDateById;

public sealed class GetFgsNonWorkingDateByIdQueryHandler(
    IFgsNonWorkingDateReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsNonWorkingDateByIdQuery, ApiResponse<FgsNonWorkingDateDetailDto>>
{
    public async Task<ApiResponse<FgsNonWorkingDateDetailDto>> Handle(
        GetFgsNonWorkingDateByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "nonworkingdate",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsNonWorkingDateDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsNonWorkingDateDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsNonWorkingDateDetailDto>.Fail(
                [$"Non-working date '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsNonWorkingDateDetailDto>.Ok(result);
    }
}
