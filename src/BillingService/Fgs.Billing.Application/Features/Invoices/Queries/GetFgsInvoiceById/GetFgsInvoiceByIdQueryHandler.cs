using Fgs.Contracts.Api;
using Fgs.Billing.Application.Abstractions.Invoices;
using Fgs.Billing.Application.Features.Invoices.Dtos;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;

namespace Fgs.Billing.Application.Features.Invoices.Queries.GetFgsInvoiceById;

public sealed class GetFgsInvoiceByIdQueryHandler(
    IFgsInvoiceReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsInvoiceByIdQuery, ApiResponse<FgsInvoiceDetailDto>>
{
    public async Task<ApiResponse<FgsInvoiceDetailDto>> Handle(
        GetFgsInvoiceByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "invoice",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsInvoiceDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsInvoiceDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsInvoiceDetailDto>.Fail(
                [$"Invoice '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsInvoiceDetailDto>.Ok(result);
    }
}
