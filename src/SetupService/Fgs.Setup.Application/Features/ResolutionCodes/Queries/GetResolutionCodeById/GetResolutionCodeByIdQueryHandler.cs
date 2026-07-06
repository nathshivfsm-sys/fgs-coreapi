using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.ResolutionCodes;
using Fgs.Setup.Application.Features.ResolutionCodes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.ResolutionCodes.Queries.GetResolutionCodeById;

public sealed class GetResolutionCodeByIdQueryHandler(
    IResolutionCodeReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetResolutionCodeByIdQuery, ApiResponse<ResolutionCodeDetailDto>>
{
    public async Task<ApiResponse<ResolutionCodeDetailDto>> Handle(
        GetResolutionCodeByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "resolutioncode",
            request.Id.ToString());

        var cached = await cache.GetAsync<ResolutionCodeDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<ResolutionCodeDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<ResolutionCodeDetailDto>.Fail(
                [$"Resolution Code '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<ResolutionCodeDetailDto>.Ok(result);
    }
}
