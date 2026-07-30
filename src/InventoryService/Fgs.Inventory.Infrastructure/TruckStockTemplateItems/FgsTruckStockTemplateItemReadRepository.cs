using Dapper;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Abstractions.Persistence;
using Fgs.Inventory.Application.Abstractions.TruckStockTemplateItems;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.TruckStockTemplateItems.Dtos;
using Fgs.Inventory.Infrastructure.Common;
using Fgs.MultiTenancy;

namespace Fgs.Inventory.Infrastructure.TruckStockTemplateItems;

internal sealed class FgsTruckStockTemplateItemReadRepository : IFgsTruckStockTemplateItemReadRepository
{
    private readonly IInventoryReadConnectionFactory _connectionFactory;
    private readonly ITenantContextAccessor _tenantContextAccessor;

    public FgsTruckStockTemplateItemReadRepository(
        IInventoryReadConnectionFactory connectionFactory,
        ITenantContextAccessor tenantContextAccessor)
    {
        _connectionFactory = connectionFactory;
        _tenantContextAccessor = tenantContextAccessor;
    }

    public async Task<FgsTruckStockTemplateItemDetailDto?> GetByIdAsync(
        long templateId,
        long itemId,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = InventoryTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT {FgsTruckStockTemplateItemSql.SelectDetailColumns}
            FROM {FgsTruckStockTemplateItemSql.Table}
            WHERE "Id" = @ItemId
              AND "TruckStockTemplateId" = @TemplateId
              AND "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var row = await connection.QueryFirstOrDefaultAsync<FgsTruckStockTemplateItemDetailRow>(
            new CommandDefinition(
                sql,
                new { ItemId = itemId, TemplateId = templateId, TenantId = tenantId, CompanyId = companyId },
                cancellationToken: cancellationToken));

        return row?.ToDto();
    }

    public async Task<PagedResult<FgsTruckStockTemplateItemSummaryDto>> ListByTemplateAsync(
        long templateId,
        InventoryListQuery query,
        FgsTruckStockTemplateItemListFilters filters,
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
            "\"CompanyId\" = @CompanyId",
            "\"TruckStockTemplateId\" = @TemplateId"
        };

        if (filters.InventoryItemId.HasValue)
        {
            where.Add("\"InventoryItemId\" = @InventoryItemId");
        }

        var whereClause = string.Join(" AND ", where);
        var orderBy = FgsTruckStockTemplateItemSql.ResolveOrderBy(paging.SortBy, paging.SortDirection);

        var sql = $"""
            SELECT {FgsTruckStockTemplateItemSql.SelectSummaryColumns}
            FROM {FgsTruckStockTemplateItemSql.Table}
            WHERE {whereClause}
            {orderBy}
            LIMIT @PageSize OFFSET @Offset;

            SELECT COUNT(*)
            FROM {FgsTruckStockTemplateItemSql.Table}
            WHERE {whereClause};
            """;

        var parameters = new
        {
            TenantId = tenantId,
            CompanyId = companyId,
            TemplateId = templateId,
            InventoryItemId = filters.InventoryItemId,
            PageSize = pageSize,
            Offset = offset
        };

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

        var rows = (await multi.ReadAsync<FgsTruckStockTemplateItemSummaryRow>()).ToList();
        var totalCount = await multi.ReadSingleAsync<int>();

        return new PagedResult<FgsTruckStockTemplateItemSummaryDto>(
            rows.Select(r => r.ToDto()).ToList(),
            page,
            pageSize,
            totalCount);
    }

    public async Task<bool> ExistsByTemplateAndItemAsync(
        long templateId,
        long inventoryItemId,
        long? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = InventoryTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {FgsTruckStockTemplateItemSql.Table}
                WHERE "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND "TruckStockTemplateId" = @TemplateId
                  AND "InventoryItemId" = @InventoryItemId
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
                    TemplateId = templateId,
                    InventoryItemId = inventoryItemId,
                    ExcludeId = excludeId
                },
                cancellationToken: cancellationToken));
    }

    public async Task<bool> ExistsInventoryItemAsync(long inventoryItemId, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = InventoryTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {FgsTruckStockTemplateItemSql.InventoryItemTable}
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
