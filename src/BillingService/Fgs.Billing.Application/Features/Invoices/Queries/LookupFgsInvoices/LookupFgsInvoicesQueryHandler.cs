using Fgs.Contracts.Api;
using Fgs.Billing.Application.Abstractions.Invoices;
using Fgs.Billing.Application.Features.Invoices.Dtos;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;

namespace Fgs.Billing.Application.Features.Invoices.Queries.LookupFgsInvoices;

public sealed class LookupFgsInvoicesQueryHandler(
    IFgsInvoiceReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupFgsInvoicesQuery, ApiResponse<IReadOnlyList<FgsInvoiceLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsInvoiceLookupDto>>> Handle(
        LookupFgsInvoicesQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "invoice",
            "lookup");

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<FgsInvoiceLookupDto>>.Ok(result ?? Array.Empty<FgsInvoiceLookupDto>());
    }
}
