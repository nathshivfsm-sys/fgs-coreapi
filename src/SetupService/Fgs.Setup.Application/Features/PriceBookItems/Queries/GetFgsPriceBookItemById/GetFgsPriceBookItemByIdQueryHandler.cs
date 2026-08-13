using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.PriceBookItems;
using Fgs.Setup.Application.Features.PriceBookItems.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.PriceBookItems.Queries.GetFgsPriceBookItemById;

public sealed class GetFgsPriceBookItemByIdQueryHandler(
    IFgsPriceBookItemReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsPriceBookItemByIdQuery, ApiResponse<FgsPriceBookItemDetailDto>>
{
    public async Task<ApiResponse<FgsPriceBookItemDetailDto>> Handle(
        GetFgsPriceBookItemByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "pricebookitem",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsPriceBookItemDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsPriceBookItemDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsPriceBookItemDetailDto>.Fail(
                [$"Price book item '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsPriceBookItemDetailDto>.Ok(result);
    }
}
