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
    : IRequestHandler<ListEmployeesQuery, ApiResponse<FgsEmployeeListResultDto>>
{
    private static readonly FgsEmployeeListSummaryDto EmptySummary = new(0, 0, 0);

    public async Task<ApiResponse<FgsEmployeeListResultDto>> Handle(
        ListEmployeesQuery request,
        CancellationToken cancellationToken)
    {
        var filters = request.Filters;
        var tenantContext = tenantContextAccessor.Current;

        if (filters.RoleIds is { Count: > 0 })
        {
            if (tenantContext is null)
            {
                return ApiResponse<FgsEmployeeListResultDto>.Fail(
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
                return ApiResponse<FgsEmployeeListResultDto>.Fail(
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
                var summary = request.IncludeSummary
                    ? await readRepository.GetListSummaryAsync(cancellationToken)
                    : EmptySummary;
                return ApiResponse<FgsEmployeeListResultDto>.Ok(
                    new FgsEmployeeListResultDto([], page, pageSize, 0, summary));
            }

            filters = filters with { UserIds = userIdsResponse.Data };
        }

        var result = await readRepository.ListAsync(
            request.Query,
            filters,
            request.IncludeSummary,
            cancellationToken);

        var enrichedItems = await EnrichItemsAsync(result.Items, tenantContext, cancellationToken);
        enrichedItems = ApplyEnrichmentSort(
            enrichedItems,
            request.Query.SortBy,
            request.Query.SortDirection);

        return ApiResponse<FgsEmployeeListResultDto>.Ok(
            result with { Items = enrichedItems });
    }

    private async Task<IReadOnlyList<FgsEmployeeSummaryDto>> EnrichItemsAsync(
        IReadOnlyList<FgsEmployeeSummaryDto> items,
        ITenantContext? tenantContext,
        CancellationToken cancellationToken)
    {
        var userIds = items
            .Where(i => i.UserId.HasValue)
            .Select(i => i.UserId!.Value)
            .Distinct()
            .ToList();

        if (userIds.Count == 0 || tenantContext is null)
        {
            return items;
        }

        var enrichmentResponse = await userInternalUsersClient.GetListEnrichmentAsync(
            userIds,
            tenantContext.TenantId.ToString(),
            tenantContext.CompanyId.ToString(),
            cancellationToken);

        if (!enrichmentResponse.Success || enrichmentResponse.Data is null || enrichmentResponse.Data.Count == 0)
        {
            return items;
        }

        var byUserId = enrichmentResponse.Data.ToDictionary(e => e.UserId);
        return items
            .Select(item =>
            {
                if (item.UserId is not Guid userId || !byUserId.TryGetValue(userId, out var enrichment))
                {
                    return item;
                }

                return item with
                {
                    RoleId = enrichment.RoleId,
                    RoleName = enrichment.RoleName,
                    LastLoginOn = enrichment.LastLoginOn
                };
            })
            .ToList();
    }

    /// <summary>
    /// Best-effort: RoleName/LastLoginOn are not in Setup SQL, so only the current page is reordered.
    /// </summary>
    private static IReadOnlyList<FgsEmployeeSummaryDto> ApplyEnrichmentSort(
        IReadOnlyList<FgsEmployeeSummaryDto> items,
        string? sortBy,
        SortDirection sortDirection)
    {
        if (string.IsNullOrWhiteSpace(sortBy) || items.Count <= 1)
        {
            return items;
        }

        var desc = sortDirection == SortDirection.Desc;
        if (sortBy.Equals("LastLoginOn", StringComparison.OrdinalIgnoreCase))
        {
            return desc
                ? items.OrderByDescending(i => i.LastLoginOn).ToList()
                : items.OrderBy(i => i.LastLoginOn).ToList();
        }

        if (sortBy.Equals("RoleName", StringComparison.OrdinalIgnoreCase)
            || sortBy.Equals("Role", StringComparison.OrdinalIgnoreCase))
        {
            return desc
                ? items.OrderByDescending(i => i.RoleName).ToList()
                : items.OrderBy(i => i.RoleName).ToList();
        }

        return items;
    }
}
