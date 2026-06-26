using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.SetupPaymentMethods;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPaymentMethods.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupPaymentMethods.Queries.ListActiveSetupPaymentMethods;

public sealed class ListActiveSetupPaymentMethodsQueryHandler(
    IFgsSetupPaymentMethodReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<ListActiveSetupPaymentMethodsQuery, ApiResponse<PagedResult<FgsSetupPaymentMethodSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsSetupPaymentMethodSummaryDto>>> Handle(
        ListActiveSetupPaymentMethodsQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var segment = CacheKeys.ListActiveSegment(
        request.Page,
        request.PageSize,
        request.SortBy,
        request.SortDirection.ToString(),
        request.Search,
        CacheKeys.Fingerprint(request.Filters));

        var cacheKey = CacheKeys.Build(
        tenantScope.TenantId,
        tenantScope.CompanyId,
        "paymentmethods",
        segment);

        var cached = await cache.GetOrSetAsync(
        cacheKey,
        async () =>
        {
            var query = new SetupListQuery(
                request.Page,
                request.PageSize,
                request.SortBy,
                request.SortDirection,
                request.Search,
                IsActive: true);

            return await readRepository.ListAsync(
                query,
                request.Filters ?? new FgsSetupPaymentMethodListFilters(),
                cancellationToken);
        },
        cancellationToken: cancellationToken);

        return ApiResponse<PagedResult<FgsSetupPaymentMethodSummaryDto>>.Ok(cached!);
    }
}
