using Dapper;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Abstractions.Permissions;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.Permissions.Dtos;

namespace Fgs.User.Infrastructure.Entities.Permissions;

internal sealed class FgsPermissionReadRepository(
    IUserReadConnectionFactory connectionFactory) : IFgsPermissionReadRepository
{
    public async Task<FgsPermissionDetailDto?> GetByIdAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        var sql = $"""
            SELECT {FgsPermissionSql.SelectDetailColumns}
            FROM {FgsPermissionSql.Table}
            WHERE "Id" = @Id
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var row = await connection.QueryFirstOrDefaultAsync<FgsPermissionDetailRow>(
            new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));

        return row?.ToDto();
    }

    public async Task<PagedResult<FgsPermissionSummaryDto>> ListAsync(
        IdentityListQuery query,
        FgsPermissionListFilters filters,
        CancellationToken cancellationToken = default)
    {
        var paging = query.ToPagedQuery();
        var page = Math.Max(1, paging.Page);
        var pageSize = Math.Clamp(paging.PageSize, 1, 200);
        var offset = (page - 1) * pageSize;

        var where = new List<string>();

        if (paging.IsActive.HasValue)
        {
            where.Add("\"IsActive\" = @IsActive");
        }

        if (!string.IsNullOrWhiteSpace(filters.PermissionCode))
        {
            where.Add("\"PermissionCode\" = @PermissionCode");
        }

        if (!string.IsNullOrWhiteSpace(filters.Module))
        {
            where.Add("\"Module\" = @Module");
        }

        if (!string.IsNullOrWhiteSpace(filters.Resource))
        {
            where.Add("\"Resource\" = @Resource");
        }

        if (!string.IsNullOrWhiteSpace(filters.Action))
        {
            where.Add("\"Action\" = @Action");
        }

        if (!string.IsNullOrWhiteSpace(paging.Search))
        {
            where.Add(
                """
                ("PermissionCode" ILIKE @Search
                 OR "Module" ILIKE @Search
                 OR "Resource" ILIKE @Search
                 OR "Action" ILIKE @Search
                 OR "Name" ILIKE @Search
                 OR "Description" ILIKE @Search)
                """);
        }

        var whereClause = where.Count > 0 ? string.Join(" AND ", where) : "TRUE";
        var orderBy = FgsPermissionSql.ResolveOrderBy(paging.SortBy, paging.SortDirection);

        var sql = $"""
            SELECT {FgsPermissionSql.SelectSummaryColumns}
            FROM {FgsPermissionSql.Table}
            WHERE {whereClause}
            {orderBy}
            LIMIT @PageSize OFFSET @Offset;

            SELECT COUNT(*)
            FROM {FgsPermissionSql.Table}
            WHERE {whereClause};
            """;

        var parameters = new
        {
            IsActive = paging.IsActive,
            PermissionCode = filters.PermissionCode?.Trim().ToUpperInvariant(),
            Module = filters.Module?.Trim(),
            Resource = filters.Resource?.Trim(),
            Action = filters.Action?.Trim(),
            Search = paging.Search is null ? null : $"%{paging.Search.Trim()}%",
            PageSize = pageSize,
            Offset = offset
        };

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

        var items = (await multi.ReadAsync<FgsPermissionSummaryRow>()).Select(row => row.ToDto()).ToList();
        var totalCount = await multi.ReadSingleAsync<int>();

        return new PagedResult<FgsPermissionSummaryDto>(items, page, pageSize, totalCount);
    }

    public async Task<IReadOnlyList<FgsPermissionLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var where = new List<string>();

        if (activeOnly)
        {
            where.Add("\"IsActive\" = true");
        }

        var whereClause = where.Count > 0 ? string.Join(" AND ", where) : "TRUE";

        var sql = $"""
            SELECT {FgsPermissionSql.SelectLookupColumns}
            FROM {FgsPermissionSql.Table}
            WHERE {whereClause}
            ORDER BY "DisplayOrder" ASC, "PermissionCode" ASC
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<FgsPermissionLookupRow>(
            new CommandDefinition(sql, cancellationToken: cancellationToken));

        return rows.Select(row => row.ToDto()).ToList();
    }

    public async Task<bool> ExistsByPermissionCodeAsync(
        string permissionCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {FgsPermissionSql.Table}
                WHERE "PermissionCode" = @PermissionCode
                  AND (@ExcludeId IS NULL OR "Id" <> @ExcludeId)
            )
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                sql,
                new
                {
                    PermissionCode = permissionCode.Trim().ToUpperInvariant(),
                    ExcludeId = excludeId
                },
                cancellationToken: cancellationToken));
    }
}
