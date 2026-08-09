using Fgs.Contracts.Api;
using Fgs.Billing.Application.Abstractions.Invoices;
using Fgs.Billing.Application.Features.Invoices.Dtos;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Billing.Application.Features.Invoices.Commands.PatchFgsInvoice;

public sealed class PatchFgsInvoiceCommandHandler(
    IFgsInvoiceWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchFgsInvoiceCommandHandler> logger)
    : IRequestHandler<PatchFgsInvoiceCommand, ApiResponse<FgsInvoiceDetailDto>>
{
    public async Task<ApiResponse<FgsInvoiceDetailDto>> Handle(
        PatchFgsInvoiceCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patched invoice {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "invoice"),
            cancellationToken);
        return ApiResponse<FgsInvoiceDetailDto>.Ok(result);
    }
}
