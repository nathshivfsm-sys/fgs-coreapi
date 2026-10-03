using Fgs.Contracts.Api;
using Fgs.Crm.Application.Abstractions.Customers;
using Fgs.Crm.Application.Features.Customers.Dtos;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Crm.Application.Features.Customers.Commands.AddCrmCustomerServiceLocation;

public sealed class AddCrmCustomerServiceLocationCommandHandler(
    ICrmCustomerWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<AddCrmCustomerServiceLocationCommandHandler> logger)
    : IRequestHandler<AddCrmCustomerServiceLocationCommand, ApiResponse<CrmServiceLocationCreatedDto>>
{
    public async Task<ApiResponse<CrmServiceLocationCreatedDto>> Handle(
        AddCrmCustomerServiceLocationCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.AddServiceLocationAsync(request.CustomerId, request.Dto, cancellationToken);
        logger.LogInformation(
            "Created service location {ServiceLocationId} for customer {CustomerId}",
            result.ServiceLocationId,
            result.CustomerId);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "customer"),
            cancellationToken);
        return ApiResponse<CrmServiceLocationCreatedDto>.Ok(result, ApiStatusCodes.Created);
    }
}
