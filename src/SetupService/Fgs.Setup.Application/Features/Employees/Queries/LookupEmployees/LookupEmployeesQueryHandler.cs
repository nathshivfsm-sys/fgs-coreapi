using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Employees;
using Fgs.Setup.Application.Features.Employees.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Employees.Queries.LookupEmployees;

public sealed class LookupEmployeesQueryHandler(
    IFgsEmployeeReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<LookupEmployeesQuery, ApiResponse<IReadOnlyList<FgsEmployeeLookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<FgsEmployeeLookupDto>>> Handle(
        LookupEmployeesQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "employees",
            CacheKeys.LookupSegment(request.ActiveOnly));

        var result = await cache.GetOrSetAsync(
            cacheKey,
            () => readRepository.LookupAsync(request.ActiveOnly, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<FgsEmployeeLookupDto>>.Ok(result ?? Array.Empty<FgsEmployeeLookupDto>());
    }
}
