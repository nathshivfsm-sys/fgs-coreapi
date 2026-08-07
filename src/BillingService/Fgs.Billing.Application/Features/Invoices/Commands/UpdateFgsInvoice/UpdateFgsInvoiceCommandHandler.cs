using Fgs.Contracts.Api;
using Fgs.Billing.Application.Abstractions.Invoices;
using Fgs.Billing.Application.Features.Invoices.Dtos;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Billing.Application.Features.Invoices.Commands.UpdateFgsInvoice;

public sealed class UpdateFgsInvoiceCommandHandler(
    IFgsInvoiceWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateFgsInvoiceCommandHandler> logger)
    : IRequestHandler<UpdateFgsInvoiceCommand, ApiResponse<FgsInvoiceDetailDto>>
{
    public async Task<ApiResponse<FgsInvoiceDetailDto>> Handle(
        UpdateFgsInvoiceCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Updated invoice {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "invoice"),
            cancellationToken);
        return ApiResponse<FgsInvoiceDetailDto>.Ok(result);
    }
}
