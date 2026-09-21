using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Employees;
using Fgs.Setup.Application.Features.Employees.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Employees.Queries.ListEmployees;

public sealed class ListEmployeesQueryHandler(
    IFgsEmployeeReadRepository readRepository,
    IUserInternalUsersClient userInternalUsersClient,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<ListEmployeesQuery, ApiResponse<PagedResult<FgsEmployeeSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<FgsEmployeeSummaryDto>>> Handle(
        ListEmployeesQuery request,
        CancellationToken cancellationToken)
    {
        var filters = request.Filters;

        if (filters.RoleIds is { Count: > 0 })
        {
            var tenantContext = tenantContextAccessor.Current;
            if (tenantContext is null)
            {
                return ApiResponse<PagedResult<FgsEmployeeSummaryDto>>.Fail(
                    ["Tenant context is required."],
                    ApiStatusCodes.BadRequest);
            }

            var userIdsResponse = await userInternalUsersClient.GetUserIdsByRolesAsync(
                filters.RoleIds,
                tenantContext.TenantId.ToString(),
                tenantContext.CompanyId.ToString(),
                cancellationToken);

            if (!userIdsResponse.Success || userIdsResponse.Data is null)
            {
                return ApiResponse<PagedResult<FgsEmployeeSummaryDto>>.Fail(
                    userIdsResponse.Errors is { Count: > 0 }
                        ? userIdsResponse.Errors
                        : ["Failed to resolve users by role."],
                    userIdsResponse.StatusCode == 0
                        ? ApiStatusCodes.InternalServerError
                        : userIdsResponse.StatusCode);
            }

            if (userIdsResponse.Data.Count == 0)
            {
                var paging = request.Query.ToPagedQuery();
                var page = Math.Max(1, paging.Page);
                var pageSize = Math.Clamp(paging.PageSize, 1, 200);
                return ApiResponse<PagedResult<FgsEmployeeSummaryDto>>.Ok(
                    new PagedResult<FgsEmployeeSummaryDto>([], page, pageSize, 0));
            }

            filters = filters with { UserIds = userIdsResponse.Data };
        }

        var result = await readRepository.ListAsync(request.Query, filters, cancellationToken);
        return ApiResponse<PagedResult<FgsEmployeeSummaryDto>>.Ok(result);
    }
}
