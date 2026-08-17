using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.PriceBookItems;
using Fgs.Setup.Application.Features.PriceBookItems.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.PriceBookItems.Queries.LookupPriceBookItems;

public sealed class LookupPriceBookItemsQueryHandler(
    IFgsPriceBookItemReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupPriceBookItemsQuery, ApiResponse<IReadOnlyList<FgsPriceBookItemLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsPriceBookItemLookupDto>>> Handle(
        LookupPriceBookItemsQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "pricebookitem",
            $"lookup:pb:{request.PriceBookId}");

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.PriceBookId, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<FgsPriceBookItemLookupDto>>.Ok(
            result ?? Array.Empty<FgsPriceBookItemLookupDto>());
    }
}
