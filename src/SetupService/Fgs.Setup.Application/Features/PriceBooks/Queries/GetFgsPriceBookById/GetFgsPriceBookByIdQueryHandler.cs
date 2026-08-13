using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.PriceBooks;
using Fgs.Setup.Application.Features.PriceBooks.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.PriceBooks.Queries.GetFgsPriceBookById;

public sealed class GetFgsPriceBookByIdQueryHandler(
    IFgsPriceBookReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsPriceBookByIdQuery, ApiResponse<FgsPriceBookDetailDto>>
{
    public async Task<ApiResponse<FgsPriceBookDetailDto>> Handle(
        GetFgsPriceBookByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "pricebook",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsPriceBookDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsPriceBookDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsPriceBookDetailDto>.Fail(
                [$"Price book '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsPriceBookDetailDto>.Ok(result);
    }
}
