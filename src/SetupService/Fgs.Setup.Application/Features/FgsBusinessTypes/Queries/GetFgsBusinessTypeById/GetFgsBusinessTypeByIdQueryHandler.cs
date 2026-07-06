using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.FgsBusinessTypes;
using Fgs.Setup.Application.Features.FgsBusinessTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.FgsBusinessTypes.Queries.GetFgsBusinessTypeById;

public sealed class GetFgsBusinessTypeByIdQueryHandler(
    IFgsBusinessTypeReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsBusinessTypeByIdQuery, ApiResponse<FgsBusinessTypeDetailDto>>
{
    public async Task<ApiResponse<FgsBusinessTypeDetailDto>> Handle(
        GetFgsBusinessTypeByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "businesstype",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsBusinessTypeDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsBusinessTypeDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsBusinessTypeDetailDto>.Fail(
                [$"Business Type '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsBusinessTypeDetailDto>.Ok(result);
    }
}
