using Dapper;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.Tenants.Dtos;

namespace Fgs.User.Infrastructure.Persistence.Read;

internal sealed class TenantCatalogReadRepository(IUserReadConnectionFactory connectionFactory)
    : ITenantCatalogReadRepository
{
    public async Task<PagedResult<TenantSummaryDto>> ListTenantsAsync(
        IdentityListQuery query,
        long? scopedTenantId,
        CancellationToken cancellationToken = default)
    {
        var paging = query.ToPagedQuery();
        var page = Math.Max(1, paging.Page);
        var pageSize = Math.Clamp(paging.PageSize, 1, 200);
        var offset = (page - 1) * pageSize;

        var where = new List<string> { "1 = 1" };
        if (scopedTenantId is not null)
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
            ScopedTenantId = scopedTenantId,
            IsActive = paging.IsActive,
            Search = string.IsNullOrWhiteSpace(paging.Search) ? null : $"%{paging.Search.Trim()}%",
            PageSize = pageSize,
            Offset = offset
        };

        var itemsSql = $"""
            SELECT "Id", "TenantGuid", "TenantCode" AS "Code", "Name", "FgsTenantStatusId", "IsActive"
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

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var items = (await connection.QueryAsync<TenantSummaryDto>(
            new CommandDefinition(itemsSql, parameters, cancellationToken: cancellationToken))).ToList();
        var total = await connection.ExecuteScalarAsync<int>(
            new CommandDefinition(countSql, parameters, cancellationToken: cancellationToken));

        return new PagedResult<TenantSummaryDto>(items, page, pageSize, total);
    }

    public async Task<long> GetMaxCompanyNumberAsync(long tenantId, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT COALESCE(MAX("CompanyNumber"), 0)
            FROM tenant."FgsTenantCompany"
            WHERE "TenantId" = @TenantId
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<long>(
            new CommandDefinition(sql, new { TenantId = tenantId }, cancellationToken: cancellationToken));
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
