using Dapper;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.User.Application.Abstractions.DataAccessScopes;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.DataAccessScopes.Dtos;
using Fgs.User.Infrastructure.Common;

namespace Fgs.User.Infrastructure.Entities.DataAccessScopes;

internal sealed class FgsDataAccessScopeReadRepository(
    IUserReadConnectionFactory connectionFactory,
    ITenantContextAccessor tenantContextAccessor) : IFgsDataAccessScopeReadRepository
{
    public async Task<FgsDataAccessScopeDetailDto?> GetByIdAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var sql = $"""
            SELECT {FgsDataAccessScopeSql.SelectDetailColumns}
            FROM {FgsDataAccessScopeSql.Table}
            WHERE "Id" = @Id
              AND "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var row = await connection.QueryFirstOrDefaultAsync<FgsDataAccessScopeDetailRow>(
            new CommandDefinition(sql, new { Id = id, TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return row?.ToDto();
    }

    public async Task<PagedResult<FgsDataAccessScopeSummaryDto>> ListAsync(
        IdentityListQuery query,
        FgsDataAccessScopeListFilters filters,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var paging = query.ToPagedQuery();
        var page = Math.Max(1, paging.Page);
        var pageSize = Math.Clamp(paging.PageSize, 1, 200);
        var offset = (page - 1) * pageSize;

        var where = new List<string>
        {
            "\"TenantId\" = @TenantId",
            "\"CompanyId\" = @CompanyId"
        };

        if (filters.FgsDataAccessId.HasValue)
        {
            where.Add("\"FgsDataAccessId\" = @FgsDataAccessId");
        }

        if (!string.IsNullOrWhiteSpace(filters.ScopeType))
        {
            where.Add("\"ScopeType\" = @ScopeType");
        }

        if (!string.IsNullOrWhiteSpace(paging.Search))
        {
            where.Add("(\"ScopeType\" ILIKE @Search OR \"Operator\" ILIKE @Search OR \"ScopeValue\" ILIKE @Search)");
        }

        var whereClause = string.Join(" AND ", where);
        var orderBy = FgsDataAccessScopeSql.ResolveOrderBy(paging.SortBy, paging.SortDirection);

        var sql = $"""
            SELECT {FgsDataAccessScopeSql.SelectSummaryColumns}
            FROM {FgsDataAccessScopeSql.Table}
            WHERE {whereClause}
            {orderBy}
            LIMIT @PageSize OFFSET @Offset;

            SELECT COUNT(*)
            FROM {FgsDataAccessScopeSql.Table}
            WHERE {whereClause};
            """;

        var parameters = new
        {
            TenantId = tenantId,
            CompanyId = companyId,
            FgsDataAccessId = filters.FgsDataAccessId,
            ScopeType = filters.ScopeType?.Trim(),
            Search = paging.Search is null ? null : $"%{paging.Search.Trim()}%",
            PageSize = pageSize,
            Offset = offset
        };

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

        var items = (await multi.ReadAsync<FgsDataAccessScopeSummaryRow>()).Select(row => row.ToDto()).ToList();
        var totalCount = await multi.ReadSingleAsync<int>();

        return new PagedResult<FgsDataAccessScopeSummaryDto>(items, page, pageSize, totalCount);
    }
}
