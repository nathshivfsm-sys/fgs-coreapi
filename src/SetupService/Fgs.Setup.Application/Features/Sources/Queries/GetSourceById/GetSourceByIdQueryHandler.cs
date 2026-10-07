using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Sources;
using Fgs.Setup.Application.Features.Sources.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Sources.Queries.GetSourceById;

public sealed class GetSourceByIdQueryHandler(
    ISourceReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetSourceByIdQuery, ApiResponse<SourceDetailDto>>
{
    public async Task<ApiResponse<SourceDetailDto>> Handle(
        GetSourceByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "source",
            request.Id.ToString());

        var cached = await cache.GetAsync<SourceDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<SourceDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<SourceDetailDto>.Fail(
                [$"source '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<SourceDetailDto>.Ok(result);
    }
}
