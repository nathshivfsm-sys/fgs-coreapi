using Fgs.Contracts.Api;
using Fgs.Crm.Application.Abstractions.Customers;
using Fgs.Crm.Application.Features.Customers.Dtos;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Crm.Application.Features.Customers.Commands.CreateCrmCustomer;

public sealed class CreateCrmCustomerCommandHandler(
    ICrmCustomerWriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<CreateCrmCustomerCommandHandler> logger)
    : IRequestHandler<CreateCrmCustomerCommand, ApiResponse<CrmCustomerCreateResultDto>>
{
    public async Task<ApiResponse<CrmCustomerCreateResultDto>> Handle(
        CreateCrmCustomerCommand request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.CreateAsync(request.Dto, cancellationToken);
        logger.LogInformation(
            "Created customer {Id} with number {CustomerNumber}",
            result.Customer.Id,
            result.Customer.CustomerNumber);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "customer"),
            cancellationToken);
        return ApiResponse<CrmCustomerCreateResultDto>.Ok(result, ApiStatusCodes.Created);
    }
}
