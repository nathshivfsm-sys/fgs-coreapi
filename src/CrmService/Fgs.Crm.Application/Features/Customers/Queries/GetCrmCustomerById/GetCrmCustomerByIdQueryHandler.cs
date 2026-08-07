using Fgs.Contracts.Api;
using Fgs.Crm.Application.Abstractions.Customers;
using Fgs.Crm.Application.Features.Customers.Dtos;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;

namespace Fgs.Crm.Application.Features.Customers.Queries.GetCrmCustomerById;

public sealed class GetCrmCustomerByIdQueryHandler(
    ICrmCustomerReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetCrmCustomerByIdQuery, ApiResponse<CrmCustomerDetailDto>>
{
    public async Task<ApiResponse<CrmCustomerDetailDto>> Handle(
        GetCrmCustomerByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "customer",
            request.Id.ToString());

        var cached = await cache.GetAsync<CrmCustomerDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<CrmCustomerDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<CrmCustomerDetailDto>.Fail(
                [$"Customer '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<CrmCustomerDetailDto>.Ok(result);
    }
}
