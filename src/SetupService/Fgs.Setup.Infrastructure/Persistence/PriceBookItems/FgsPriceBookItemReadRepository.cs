using Dapper;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Persistence;
using Fgs.Setup.Application.Abstractions.PriceBookItems;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.PriceBookItems.Dtos;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.PriceBookItems;

internal sealed class FgsPriceBookItemReadRepository(
    ISetupReadConnectionFactory connectionFactory,
    ITenantContextAccessor tenantContextAccessor) : IFgsPriceBookItemReadRepository
{
    public async Task<FgsPriceBookItemDetailDto?> GetByIdAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var sql = $"""
            SELECT {FgsPriceBookItemSql.SelectDetailColumns}
            FROM {FgsPriceBookItemSql.Table}
            WHERE "Id" = @Id
              AND "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var row = await connection.QueryFirstOrDefaultAsync<FgsPriceBookItemDetailRow>(
            new CommandDefinition(sql, new { Id = id, TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return row?.ToDto();
    }

    public async Task<PagedResult<FgsPriceBookItemSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsPriceBookItemListFilters filters,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var paging = query.ToPagedQuery();
        var page = Math.Max(1, paging.Page);
        var pageSize = Math.Clamp(paging.PageSize, 1, 200);
        var offset = (page - 1) * pageSize;

        var where = new List<string>
        {
            "\"TenantId\" = @TenantId",
            "\"CompanyId\" = @CompanyId"
        };

        if (filters.PriceBookId.HasValue)
        {
            where.Add("\"PriceBookId\" = @PriceBookId");
        }

        if (!string.IsNullOrWhiteSpace(filters.ItemCode))
        {
            where.Add("\"ItemCode\" ILIKE @ItemCode");
        }

        if (!string.IsNullOrWhiteSpace(filters.ItemDescription))
        {
            where.Add("\"ItemDescription\" ILIKE @ItemDescription");
        }

        if (!string.IsNullOrWhiteSpace(paging.Search))
        {
            where.Add("(\"ItemCode\" ILIKE @Search OR \"ItemDescription\" ILIKE @Search)");
        }

        var whereClause = string.Join(" AND ", where);
        var orderBy = FgsPriceBookItemSql.ResolveOrderBy(paging.SortBy, paging.SortDirection);

        var sql = $"""
            SELECT {FgsPriceBookItemSql.SelectSummaryColumns}
            FROM {FgsPriceBookItemSql.Table}
            WHERE {whereClause}
            {orderBy}
            LIMIT @PageSize OFFSET @Offset;

            SELECT COUNT(*)
            FROM {FgsPriceBookItemSql.Table}
            WHERE {whereClause};
            """;

        var parameters = new
        {
            TenantId = tenantId,
            CompanyId = companyId,
            PriceBookId = filters.PriceBookId,
            ItemCode = string.IsNullOrWhiteSpace(filters.ItemCode) ? null : $"%{filters.ItemCode.Trim()}%",
            ItemDescription = string.IsNullOrWhiteSpace(filters.ItemDescription) ? null : $"%{filters.ItemDescription.Trim()}%",
            Search = string.IsNullOrWhiteSpace(paging.Search) ? null : $"%{paging.Search.Trim()}%",
            PageSize = pageSize,
            Offset = offset
        };

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

        var rows = (await multi.ReadAsync<FgsPriceBookItemSummaryRow>()).ToList();
        var totalCount = await multi.ReadSingleAsync<int>();

        return new PagedResult<FgsPriceBookItemSummaryDto>(
            rows.Select(r => r.ToDto()).ToList(),
            page,
            pageSize,
            totalCount);
    }

    public async Task<IReadOnlyList<FgsPriceBookItemLookupDto>> LookupAsync(
        long? priceBookId = null,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var parentFilter = priceBookId.HasValue
            ? "AND \"PriceBookId\" = @PriceBookId"
            : string.Empty;
        var sql = $"""
            SELECT {FgsPriceBookItemSql.SelectLookupColumns}
            FROM {FgsPriceBookItemSql.Table}
            WHERE "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
              {parentFilter}
            ORDER BY "DisplayOrder" ASC NULLS LAST, "ItemDescription" ASC
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<FgsPriceBookItemLookupRow>(
            new CommandDefinition(
                sql,
                new { TenantId = tenantId, CompanyId = companyId, PriceBookId = priceBookId },
                cancellationToken: cancellationToken));

        return rows.Select(r => r.ToDto()).ToList();
    }

    public async Task<bool> ExistsPriceBookIdAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {FgsPriceBookItemSql.ParentTable}
                WHERE "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND "Id" = @Id
                  AND "IsActive" = TRUE
            )
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                sql,
                new { TenantId = tenantId, CompanyId = companyId, Id = id },
                cancellationToken: cancellationToken));
    }
}
