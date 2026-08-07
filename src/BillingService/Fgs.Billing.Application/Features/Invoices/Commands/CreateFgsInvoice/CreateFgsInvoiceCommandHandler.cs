using Fgs.Contracts.Api;
using Fgs.Billing.Application.Abstractions.Invoices;
using Fgs.Billing.Application.Features.Invoices.Dtos;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Billing.Application.Features.Invoices.Commands.CreateFgsInvoice;

public sealed class CreateFgsInvoiceCommandHandler(
    IFgsInvoiceWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateFgsInvoiceCommandHandler> logger)
    : IRequestHandler<CreateFgsInvoiceCommand, ApiResponse<FgsInvoiceDetailDto>>
{
    public async Task<ApiResponse<FgsInvoiceDetailDto>> Handle(
        CreateFgsInvoiceCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation(
            "Created invoice {Id} with number {InvoiceNumber}",
            result.Id,
            result.InvoiceNumber);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "invoice"),
            cancellationToken);
        return ApiResponse<FgsInvoiceDetailDto>.Ok(result, ApiStatusCodes.Created);
    }
}
