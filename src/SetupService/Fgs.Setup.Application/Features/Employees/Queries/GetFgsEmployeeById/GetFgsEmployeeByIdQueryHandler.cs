using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Employees;
using Fgs.Setup.Application.Features.Employees.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Employees.Queries.GetFgsEmployeeById;

public sealed class GetFgsEmployeeByIdQueryHandler(
    IFgsEmployeeReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<GetFgsEmployeeByIdQuery, ApiResponse<FgsEmployeeDetailDto>>
{
    public async Task<ApiResponse<FgsEmployeeDetailDto>> Handle(
        GetFgsEmployeeByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "employees",
            request.Id.ToString());

        var cached = await cache.GetAsync<FgsEmployeeDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<FgsEmployeeDetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<FgsEmployeeDetailDto>.Fail(
                [$"Employee '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<FgsEmployeeDetailDto>.Ok(result);
    }
}
