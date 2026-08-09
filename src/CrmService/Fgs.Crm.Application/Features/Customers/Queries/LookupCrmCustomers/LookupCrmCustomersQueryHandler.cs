using Fgs.Contracts.Api;
using Fgs.Crm.Application.Abstractions.Customers;
using Fgs.Crm.Application.Features.Customers.Dtos;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;

namespace Fgs.Crm.Application.Features.Customers.Queries.LookupCrmCustomers;

public sealed class LookupCrmCustomersQueryHandler(
    ICrmCustomerReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupCrmCustomersQuery, ApiResponse<IReadOnlyList<CrmCustomerLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<CrmCustomerLookupDto>>> Handle(
        LookupCrmCustomersQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "customer",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<CrmCustomerLookupDto>>.Ok(result ?? Array.Empty<CrmCustomerLookupDto>());
    }
}
