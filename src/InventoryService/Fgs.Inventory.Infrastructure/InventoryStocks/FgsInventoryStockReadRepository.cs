using Dapper;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Abstractions.InventoryStocks;
using Fgs.Inventory.Application.Abstractions.Persistence;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.InventoryStocks.Dtos;
using Fgs.Inventory.Infrastructure.Common;
using Fgs.MultiTenancy;

namespace Fgs.Inventory.Infrastructure.InventoryStocks;

internal sealed class FgsInventoryStockReadRepository : IFgsInventoryStockReadRepository
{
    private readonly IInventoryReadConnectionFactory _connectionFactory;
    private readonly ITenantContextAccessor _tenantContextAccessor;

    public FgsInventoryStockReadRepository(
        IInventoryReadConnectionFactory connectionFactory,
        ITenantContextAccessor tenantContextAccessor)
    {
        _connectionFactory = connectionFactory;
        _tenantContextAccessor = tenantContextAccessor;
    }

    public async Task<FgsInventoryStockDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = InventoryTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT {FgsInventoryStockSql.SelectDetailColumns}
            FROM {FgsInventoryStockSql.Table}
            WHERE "Id" = @Id
              AND "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var row = await connection.QueryFirstOrDefaultAsync<FgsInventoryStockDetailRow>(
            new CommandDefinition(sql, new { Id = id, TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return row?.ToDto();
    }

    public async Task<PagedResult<FgsInventoryStockSummaryDto>> ListAsync(
        InventoryListQuery query,
        FgsInventoryStockListFilters filters,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = InventoryTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var paging = query.ToPagedQuery();
        var page = Math.Max(1, paging.Page);
        var pageSize = Math.Clamp(paging.PageSize, 1, 200);
        var offset = (page - 1) * pageSize;

        var where = new List<string>
        {
            "\"TenantId\" = @TenantId",
            "\"CompanyId\" = @CompanyId"
        };

        if (filters.InventoryItemId.HasValue)
        {
            where.Add("\"InventoryItemId\" = @InventoryItemId");
        }

        if (!string.IsNullOrWhiteSpace(paging.Search))
        {
            where.Add("\"InventoryItemId\"::text ILIKE @Search");
        }

        var whereClause = string.Join(" AND ", where);
        var orderBy = FgsInventoryStockSql.ResolveOrderBy(paging.SortBy, paging.SortDirection);

        var sql = $"""
            SELECT {FgsInventoryStockSql.SelectSummaryColumns}
            FROM {FgsInventoryStockSql.Table}
            WHERE {whereClause}
            {orderBy}
            LIMIT @PageSize OFFSET @Offset;

            SELECT COUNT(*)
            FROM {FgsInventoryStockSql.Table}
            WHERE {whereClause};
            """;

        var parameters = new
        {
            TenantId = tenantId,
            CompanyId = companyId,
            InventoryItemId = filters.InventoryItemId,
            Search = string.IsNullOrWhiteSpace(paging.Search) ? null : $"%{paging.Search.Trim()}%",
            PageSize = pageSize,
            Offset = offset
        };

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

        var rows = (await multi.ReadAsync<FgsInventoryStockSummaryRow>()).ToList();
        var totalCount = await multi.ReadSingleAsync<int>();

        return new PagedResult<FgsInventoryStockSummaryDto>(
            rows.Select(r => r.ToDto()).ToList(),
            page,
            pageSize,
            totalCount);
    }

    public async Task<bool> ExistsByInventoryItemIdAsync(
        long inventoryItemId,
        long? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = InventoryTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {FgsInventoryStockSql.Table}
                WHERE "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND "InventoryItemId" = @InventoryItemId
                  {(excludeId.HasValue ? "AND \"Id\" <> @ExcludeId" : string.Empty)}
            )
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                sql,
                new { TenantId = tenantId, CompanyId = companyId, InventoryItemId = inventoryItemId, ExcludeId = excludeId },
                cancellationToken: cancellationToken));
    }

    public async Task<bool> ExistsInventoryItemAsync(long inventoryItemId, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = InventoryTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {FgsInventoryStockSql.InventoryItemTable}
                WHERE "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND "Id" = @InventoryItemId
                  AND "IsActive" = TRUE
            )
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                sql,
                new { TenantId = tenantId, CompanyId = companyId, InventoryItemId = inventoryItemId },
                cancellationToken: cancellationToken));
    }
}
