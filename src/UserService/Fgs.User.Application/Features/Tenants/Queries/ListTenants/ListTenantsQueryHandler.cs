using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.Security.Abstractions;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Features.Tenants.Dtos;
using Fgs.User.Domain.Entities;
using MediatR;

namespace Fgs.User.Application.Features.Tenants.Queries.ListTenants;

public sealed class ListTenantsQueryHandler(
    IUserReadRepository<FgsTenant> tenantReadRepository,
    IFgsUserContext userContext)
    : IRequestHandler<ListTenantsQuery, ApiResponse<PagedResult<TenantSummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<TenantSummaryDto>>> Handle(
        ListTenantsQuery request,
        CancellationToken cancellationToken)
    {
        var paging = request.Query.ToPagedQuery();
        var page = Math.Max(1, paging.Page);
        var pageSize = Math.Clamp(paging.PageSize, 1, 200);
        var offset = (page - 1) * pageSize;

        var where = new List<string> { "1 = 1" };
        if (userContext.IsAuthenticated && userContext.TenantId is long scopedTenantId)
        {
            where.Add("\"Id\" = @ScopedTenantId");
        }

        if (paging.IsActive.HasValue)
        {
            where.Add("\"IsActive\" = @IsActive");
        }

        if (!string.IsNullOrWhiteSpace(paging.Search))
        {
            where.Add("(\"Name\" ILIKE @Search OR \"TenantCode\" ILIKE @Search)");
        }

        var whereClause = string.Join(" AND ", where);
        var orderBy = ResolveOrderBy(paging.SortBy, paging.SortDirection);

        var parameters = new
        {
            ScopedTenantId = userContext.TenantId,
            IsActive = paging.IsActive,
            Search = string.IsNullOrWhiteSpace(paging.Search) ? null : $"%{paging.Search.Trim()}%",
            PageSize = pageSize,
            Offset = offset
        };

        var itemsSql = $"""
            SELECT *
            FROM tenant."FgsTenant"
            WHERE {whereClause}
            {orderBy}
            LIMIT @PageSize OFFSET @Offset
            """;

        var countSql = $"""
            SELECT COUNT(*)
            FROM tenant."FgsTenant"
            WHERE {whereClause}
            """;

        var entities = await tenantReadRepository.QueryListAsync<FgsTenant>(
            itemsSql,
            parameters,
            cancellationToken);
        var total = await tenantReadRepository.QueryFirstAsync<int>(
            countSql,
            parameters,
            cancellationToken);

        var items = entities
            .Select(t => new TenantSummaryDto(
                t.Id,
                t.TenantGuid,
                t.TenantCode,
                t.Name,
                t.FgsTenantStatusId,
                t.IsActive))
            .ToList();

        return ApiResponse<PagedResult<TenantSummaryDto>>.Ok(
            new PagedResult<TenantSummaryDto>(items, page, pageSize, total));
    }

    private static string ResolveOrderBy(string? sortBy, SortDirection sortDirection)
    {
        var direction = sortDirection == SortDirection.Desc ? "DESC" : "ASC";
        var column = sortBy?.Trim().ToLowerInvariant() switch
        {
            "code" or "tenantcode" => "\"TenantCode\"",
            "id" => "\"Id\"",
            "status" or "fgstenantstatusid" => "\"FgsTenantStatusId\"",
            "isactive" => "\"IsActive\"",
            _ => "\"Name\""
        };
        return $"ORDER BY {column} {direction}";
    }
}
