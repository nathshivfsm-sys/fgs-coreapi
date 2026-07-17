using Dapper;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Abstractions.ApiEvents;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.ApiEvents.Dtos;

namespace Fgs.User.Infrastructure.Entities.ApiEvents;

internal sealed class FgsApiEventReadRepository(
    IUserReadConnectionFactory connectionFactory) : IFgsApiEventReadRepository
{
    public async Task<FgsApiEventDetailDto?> GetByIdAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        var sql = $"""
            SELECT {FgsApiEventSql.SelectDetailColumns}
            FROM {FgsApiEventSql.Table}
            WHERE "Id" = @Id
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var row = await connection.QueryFirstOrDefaultAsync<FgsApiEventDetailRow>(
            new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));

        return row?.ToDto();
    }

    public async Task<PagedResult<FgsApiEventSummaryDto>> ListAsync(
        IdentityListQuery query,
        FgsApiEventListFilters filters,
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

        if (!string.IsNullOrWhiteSpace(filters.EventCode))
        {
            where.Add("\"EventCode\" = @EventCode");
        }

        if (!string.IsNullOrWhiteSpace(filters.EventCategory))
        {
            where.Add("\"EventCategory\" = @EventCategory");
        }

        if (!string.IsNullOrWhiteSpace(filters.Name))
        {
            where.Add("\"Name\" = @Name");
        }

        if (!string.IsNullOrWhiteSpace(paging.Search))
        {
            where.Add(
                """
                ("EventCode" ILIKE @Search
                 OR "EventCategory" ILIKE @Search
                 OR "Name" ILIKE @Search
                 OR "Description" ILIKE @Search)
                """);
        }

        var whereClause = where.Count > 0 ? string.Join(" AND ", where) : "TRUE";
        var orderBy = FgsApiEventSql.ResolveOrderBy(paging.SortBy, paging.SortDirection);

        var sql = $"""
            SELECT {FgsApiEventSql.SelectSummaryColumns}
            FROM {FgsApiEventSql.Table}
            WHERE {whereClause}
            {orderBy}
            LIMIT @PageSize OFFSET @Offset;

            SELECT COUNT(*)
            FROM {FgsApiEventSql.Table}
            WHERE {whereClause};
            """;

        var parameters = new
        {
            IsActive = paging.IsActive,
            EventCode = filters.EventCode?.Trim().ToUpperInvariant(),
            EventCategory = filters.EventCategory?.Trim(),
            Name = filters.Name?.Trim(),
            Search = paging.Search is null ? null : $"%{paging.Search.Trim()}%",
            PageSize = pageSize,
            Offset = offset
        };

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

        var items = (await multi.ReadAsync<FgsApiEventSummaryRow>()).Select(row => row.ToDto()).ToList();
        var totalCount = await multi.ReadSingleAsync<int>();

        return new PagedResult<FgsApiEventSummaryDto>(items, page, pageSize, totalCount);
    }

    public async Task<IReadOnlyList<FgsApiEventLookupDto>> LookupAsync(
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
            SELECT {FgsApiEventSql.SelectLookupColumns}
            FROM {FgsApiEventSql.Table}
            WHERE {whereClause}
            ORDER BY "DisplayOrder" ASC, "EventCode" ASC
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<FgsApiEventLookupRow>(
            new CommandDefinition(sql, cancellationToken: cancellationToken));

        return rows.Select(row => row.ToDto()).ToList();
    }

    public async Task<bool> ExistsByEventCodeAsync(
        string eventCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {FgsApiEventSql.Table}
                WHERE "EventCode" = @EventCode
                  AND (@ExcludeId IS NULL OR "Id" <> @ExcludeId)
            )
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                sql,
                new
                {
                    EventCode = eventCode.Trim().ToUpperInvariant(),
                    ExcludeId = excludeId
                },
                cancellationToken: cancellationToken));
    }
}
