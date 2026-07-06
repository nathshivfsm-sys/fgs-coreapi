using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Tags;
using Fgs.Setup.Application.Features.Tags.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Tags.Queries.GetFgsTagById;

public sealed class GetFgsTagByIdQueryHandler(
    IFgsTagReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsTagByIdQuery, ApiResponse<FgsTagDetailDto>>
{
    public async Task<ApiResponse<FgsTagDetailDto>> Handle(
        GetFgsTagByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "tag",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsTagDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsTagDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsTagDetailDto>.Fail(
                [$"Tag '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsTagDetailDto>.Ok(result);
    }
}
