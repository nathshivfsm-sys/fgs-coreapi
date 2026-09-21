using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Employees;
using Fgs.Setup.Application.Features.Employees.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Employees.Queries.GetFgsEmployeeById;

public sealed class GetFgsEmployeeByIdQueryHandler(
    IFgsEmployeeReadRepository readRepository,
    IUserInternalUsersClient userInternalUsersClient,
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
        FgsEmployeeDetailDto? result;
        if (cached is not null)
        {
            result = cached;
        }
        else
        {
            result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
            if (result is null)
            {
                return ApiResponse<FgsEmployeeDetailDto>.Fail(
                    [$"Employee '{request.Id}' was not found."],
                    ApiStatusCodes.NotFound);
            }

            // Cache DB shape only; Role/LastLogin enriched below so LastLogin stays fresh.
            await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        }

        result = await EnrichAsync(result, tenantScope, cancellationToken);
        return ApiResponse<FgsEmployeeDetailDto>.Ok(result);
    }

    private async Task<FgsEmployeeDetailDto> EnrichAsync(
        FgsEmployeeDetailDto detail,
        ITenantContext tenantScope,
        CancellationToken cancellationToken)
    {
        if (detail.UserId is not Guid userId)
        {
            return detail;
        }

        var enrichmentResponse = await userInternalUsersClient.GetListEnrichmentAsync(
            [userId],
            tenantScope.TenantId.ToString(),
            tenantScope.CompanyId.ToString(),
            cancellationToken);

        if (!enrichmentResponse.Success
            || enrichmentResponse.Data is null
            || enrichmentResponse.Data.Count == 0)
        {
            return detail;
        }

        var enrichment = enrichmentResponse.Data[0];
        return detail with
        {
            RoleId = enrichment.RoleId,
            RoleName = enrichment.RoleName,
            LastLoginOn = enrichment.LastLoginOn
        };
    }
}
