using Dapper;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Abstractions.InventoryItems;
using Fgs.Inventory.Application.Abstractions.Persistence;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.InventoryItems.Dtos;
using Fgs.Inventory.Infrastructure.Common;
using Fgs.MultiTenancy;

namespace Fgs.Inventory.Infrastructure.InventoryItems;

internal sealed class FgsInventoryItemReadRepository : IFgsInventoryItemReadRepository
{
    private readonly IInventoryReadConnectionFactory _connectionFactory;
    private readonly ITenantContextAccessor _tenantContextAccessor;

    public FgsInventoryItemReadRepository(
        IInventoryReadConnectionFactory connectionFactory,
        ITenantContextAccessor tenantContextAccessor)
    {
        _connectionFactory = connectionFactory;
        _tenantContextAccessor = tenantContextAccessor;
    }

    public async Task<FgsInventoryItemDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = InventoryTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT {FgsInventoryItemSql.SelectDetailColumns}
            FROM {FgsInventoryItemSql.Table}
            WHERE "Id" = @Id
              AND "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId;

            SELECT {FgsInventoryItemSql.SelectAlternateColumns}
            FROM {FgsInventoryItemSql.AlternateTable}
            WHERE "InventoryItemId" = @Id
              AND "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
            ORDER BY "PriorityOrder" ASC, "Id" ASC;

            SELECT {FgsInventoryItemSql.SelectDependencyColumns}
            FROM {FgsInventoryItemSql.DependencyTable}
            WHERE "InventoryItemId" = @Id
              AND "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
            ORDER BY "DisplayOrder" ASC, "Id" ASC;
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, new { Id = id, TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        var row = await multi.ReadFirstOrDefaultAsync<FgsInventoryItemDetailRow>();
        if (row is null)
        {
            return null;
        }

        var alternates = (await multi.ReadAsync<FgsInventoryItemAlternateRow>())
            .Select(a => a.ToDto())
            .ToList();
        var dependencies = (await multi.ReadAsync<FgsInventoryItemDependencyRow>())
            .Select(d => d.ToDto())
            .ToList();

        return row.ToDto(alternates, dependencies);
    }

    public async Task<PagedResult<FgsInventoryItemSummaryDto>> ListAsync(
        InventoryListQuery query,
        FgsInventoryItemListFilters filters,
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

        if (paging.IsActive.HasValue)
        {
            where.Add("\"IsActive\" = @IsActive");
        }

        if (!string.IsNullOrWhiteSpace(filters.ItemCode))
        {
            where.Add("\"ItemCode\" = @ItemCode");
        }

        if (!string.IsNullOrWhiteSpace(filters.Name))
        {
            where.Add("\"Name\" ILIKE @Name");
        }

        if (filters.InventoryItemTypeId.HasValue)
        {
            where.Add("\"InventoryItemTypeId\" = @InventoryItemTypeId");
        }

        if (!string.IsNullOrWhiteSpace(paging.Search))
        {
            where.Add("(\"ItemCode\" ILIKE @Search OR \"Name\" ILIKE @Search OR \"Description\" ILIKE @Search OR \"Sku\" ILIKE @Search)");
        }

        var whereClause = string.Join(" AND ", where);
        var orderBy = FgsInventoryItemSql.ResolveOrderBy(paging.SortBy, paging.SortDirection);

        var sql = $"""
            SELECT {FgsInventoryItemSql.SelectSummaryColumns}
            FROM {FgsInventoryItemSql.Table}
            WHERE {whereClause}
            {orderBy}
            LIMIT @PageSize OFFSET @Offset;

            SELECT COUNT(*)
            FROM {FgsInventoryItemSql.Table}
            WHERE {whereClause};
            """;

        var parameters = new
        {
            TenantId = tenantId,
            CompanyId = companyId,
            IsActive = paging.IsActive,
            ItemCode = filters.ItemCode?.Trim().ToUpperInvariant(),
            Name = string.IsNullOrWhiteSpace(filters.Name) ? null : $"%{filters.Name.Trim()}%",
            InventoryItemTypeId = filters.InventoryItemTypeId,
            Search = string.IsNullOrWhiteSpace(paging.Search) ? null : $"%{paging.Search.Trim()}%",
            PageSize = pageSize,
            Offset = offset
        };

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

        var rows = (await multi.ReadAsync<FgsInventoryItemSummaryRow>()).ToList();
        var totalCount = await multi.ReadSingleAsync<int>();

        return new PagedResult<FgsInventoryItemSummaryDto>(
            rows.Select(r => r.ToDto()).ToList(),
            page,
            pageSize,
            totalCount);
    }

    public async Task<IReadOnlyList<FgsInventoryItemLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = InventoryTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var activeFilter = activeOnly ? "AND \"IsActive\" = TRUE" : string.Empty;
        var sql = $"""
            SELECT {FgsInventoryItemSql.SelectLookupColumns}
            FROM {FgsInventoryItemSql.Table}
            WHERE "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
              {activeFilter}
            ORDER BY "Name" ASC
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<FgsInventoryItemLookupRow>(
            new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return rows.Select(r => r.ToDto()).ToList();
    }

    public async Task<bool> ExistsByItemCodeAsync(
        string itemCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = InventoryTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {FgsInventoryItemSql.Table}
                WHERE "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND "ItemCode" = @ItemCode
                  {(excludeId.HasValue ? "AND \"Id\" <> @ExcludeId" : string.Empty)}
            )
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                sql,
                new
                {
                    TenantId = tenantId,
                    CompanyId = companyId,
                    ItemCode = itemCode.Trim().ToUpperInvariant(),
                    ExcludeId = excludeId
                },
                cancellationToken: cancellationToken));
    }

    public async Task<bool> ExistsAsync(long id, bool activeOnly = true, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = InventoryTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var activeFilter = activeOnly ? "AND \"IsActive\" = TRUE" : string.Empty;
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {FgsInventoryItemSql.Table}
                WHERE "Id" = @Id
                  AND "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  {activeFilter}
            )
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                sql,
                new { Id = id, TenantId = tenantId, CompanyId = companyId },
                cancellationToken: cancellationToken));
    }

    public async Task<bool> ExistsInventoryItemAsync(long inventoryItemId, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = InventoryTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {FgsInventoryItemSql.Table}
                WHERE "Id" = @InventoryItemId
                  AND "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND "IsActive" = TRUE
            )
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                sql,
                new { InventoryItemId = inventoryItemId, TenantId = tenantId, CompanyId = companyId },
                cancellationToken: cancellationToken));
    }
}
