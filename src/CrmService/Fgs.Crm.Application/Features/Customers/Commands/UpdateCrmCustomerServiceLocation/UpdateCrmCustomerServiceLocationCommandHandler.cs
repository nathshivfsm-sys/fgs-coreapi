using Fgs.Contracts.Api;
using Fgs.Crm.Application.Abstractions.Customers;
using Fgs.Crm.Application.Features.Customers.Dtos;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Crm.Application.Features.Customers.Commands.UpdateCrmCustomerServiceLocation;

public sealed class UpdateCrmCustomerServiceLocationCommandHandler(
    ICrmCustomerWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<UpdateCrmCustomerServiceLocationCommandHandler> logger)
    : IRequestHandler<UpdateCrmCustomerServiceLocationCommand, ApiResponse<CrmServiceLocationDetailDto>>
{
    public async Task<ApiResponse<CrmServiceLocationDetailDto>> Handle(
        UpdateCrmCustomerServiceLocationCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.UpdateServiceLocationAsync(
            request.CustomerId,
            request.ServiceLocationId,
            request.Dto,
            cancellationToken);
        logger.LogInformation(
            "Updated service location {ServiceLocationId} for customer {CustomerId}",
            result.Id,
            result.CustomerId);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "customer"),
            cancellationToken);
        return ApiResponse<CrmServiceLocationDetailDto>.Ok(result);
    }
}
