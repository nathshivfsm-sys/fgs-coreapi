using Fgs.Contracts.Api;
using Fgs.Crm.Application.Abstractions.Customers;
using Fgs.Crm.Application.Features.Customers.Dtos;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Crm.Application.Features.Customers.Commands.PatchCrmCustomer;

public sealed class PatchCrmCustomerCommandHandler(
    ICrmCustomerWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<PatchCrmCustomerCommandHandler> logger)
    : IRequestHandler<PatchCrmCustomerCommand, ApiResponse<CrmCustomerDetailDto>>
{
    public async Task<ApiResponse<CrmCustomerDetailDto>> Handle(
        PatchCrmCustomerCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.PatchAsync(request.Id, request.Dto, cancellationToken);
        logger.LogInformation("Patched customer {Id}", result.Id);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "customer"),
            cancellationToken);
        return ApiResponse<CrmCustomerDetailDto>.Ok(result);
    }
}
