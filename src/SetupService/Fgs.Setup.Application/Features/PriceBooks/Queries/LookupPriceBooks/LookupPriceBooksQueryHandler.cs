using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.PriceBooks;
using Fgs.Setup.Application.Features.PriceBooks.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.PriceBooks.Queries.LookupPriceBooks;

public sealed class LookupPriceBooksQueryHandler(
    IFgsPriceBookReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupPriceBooksQuery, ApiResponse<IReadOnlyList<FgsPriceBookLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsPriceBookLookupDto>>> Handle(
        LookupPriceBooksQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "pricebook",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<FgsPriceBookLookupDto>>.Ok(
            result ?? Array.Empty<FgsPriceBookLookupDto>());
    }
}
