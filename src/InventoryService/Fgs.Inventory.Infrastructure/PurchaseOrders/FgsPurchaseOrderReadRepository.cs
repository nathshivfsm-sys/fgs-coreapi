using Dapper;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Abstractions.Persistence;
using Fgs.Inventory.Application.Abstractions.PurchaseOrders;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.PurchaseOrders.Dtos;
using Fgs.Inventory.Infrastructure.Common;
using Fgs.MultiTenancy;

namespace Fgs.Inventory.Infrastructure.PurchaseOrders;

internal sealed class FgsPurchaseOrderReadRepository : IFgsPurchaseOrderReadRepository
{
    private readonly IInventoryReadConnectionFactory _connectionFactory;
    private readonly ITenantContextAccessor _tenantContextAccessor;

    public FgsPurchaseOrderReadRepository(
        IInventoryReadConnectionFactory connectionFactory,
        ITenantContextAccessor tenantContextAccessor)
    {
        _connectionFactory = connectionFactory;
        _tenantContextAccessor = tenantContextAccessor;
    }

    public async Task<FgsPurchaseOrderDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = InventoryTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT {FgsPurchaseOrderSql.SelectDetailColumns}
            FROM {FgsPurchaseOrderSql.Table}
            WHERE "Id" = @Id
              AND "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId;

            SELECT {FgsPurchaseOrderSql.SelectLineColumns}
            FROM {FgsPurchaseOrderSql.DetailTable}
            WHERE "PurchaseOrderId" = @Id
              AND "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
            ORDER BY "LineNumber" ASC, "Id" ASC;
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, new { Id = id, TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        var row = await multi.ReadFirstOrDefaultAsync<FgsPurchaseOrderDetailRow>();
        if (row is null)
        {
            return null;
        }

        var lines = (await multi.ReadAsync<FgsPurchaseOrderLineRow>())
            .Select(l => l.ToDto())
            .ToList();

        return row.ToDto(lines);
    }

    public async Task<PagedResult<FgsPurchaseOrderSummaryDto>> ListAsync(
        InventoryListQuery query,
        FgsPurchaseOrderListFilters filters,
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

        if (!string.IsNullOrWhiteSpace(filters.PurchaseOrderNumber))
        {
            where.Add("\"PurchaseOrderNumber\" = @PurchaseOrderNumber");
        }

        if (filters.VendorId.HasValue)
        {
            where.Add("\"VendorId\" = @VendorId");
        }

        if (!string.IsNullOrWhiteSpace(filters.PurchaseOrderStatus))
        {
            where.Add("\"PurchaseOrderStatus\" = @PurchaseOrderStatus");
        }

        if (!string.IsNullOrWhiteSpace(paging.Search))
        {
            where.Add(
                "(\"PurchaseOrderNumber\" ILIKE @Search OR \"VendorReferenceNumber\" ILIKE @Search OR \"ShipToName\" ILIKE @Search OR \"InternalNotes\" ILIKE @Search)");
        }

        var whereClause = string.Join(" AND ", where);
        var orderBy = FgsPurchaseOrderSql.ResolveOrderBy(paging.SortBy, paging.SortDirection);

        var sql = $"""
            SELECT {FgsPurchaseOrderSql.SelectSummaryColumns}
            FROM {FgsPurchaseOrderSql.Table}
            WHERE {whereClause}
            {orderBy}
            LIMIT @PageSize OFFSET @Offset;

            SELECT COUNT(*)
            FROM {FgsPurchaseOrderSql.Table}
            WHERE {whereClause};
            """;

        var parameters = new
        {
            TenantId = tenantId,
            CompanyId = companyId,
            PurchaseOrderNumber = filters.PurchaseOrderNumber?.Trim(),
            VendorId = filters.VendorId,
            PurchaseOrderStatus = filters.PurchaseOrderStatus?.Trim().ToUpperInvariant(),
            Search = string.IsNullOrWhiteSpace(paging.Search) ? null : $"%{paging.Search.Trim()}%",
            PageSize = pageSize,
            Offset = offset
        };

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

        var rows = (await multi.ReadAsync<FgsPurchaseOrderSummaryRow>()).ToList();
        var totalCount = await multi.ReadSingleAsync<int>();

        return new PagedResult<FgsPurchaseOrderSummaryDto>(
            rows.Select(r => r.ToDto()).ToList(),
            page,
            pageSize,
            totalCount);
    }

    public async Task<bool> ExistsByPurchaseOrderNumberAsync(
        string purchaseOrderNumber,
        long? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = InventoryTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {FgsPurchaseOrderSql.Table}
                WHERE "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND "PurchaseOrderNumber" = @PurchaseOrderNumber
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
                    PurchaseOrderNumber = purchaseOrderNumber.Trim(),
                    ExcludeId = excludeId
                },
                cancellationToken: cancellationToken));
    }

    public async Task<bool> ExistsInventoryLocationAsync(long inventoryLocationId, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = InventoryTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {FgsPurchaseOrderSql.InventoryLocationTable}
                WHERE "Id" = @InventoryLocationId
                  AND "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND "IsActive" = TRUE
            )
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                sql,
                new { InventoryLocationId = inventoryLocationId, TenantId = tenantId, CompanyId = companyId },
                cancellationToken: cancellationToken));
    }

    public async Task<bool> ExistsInventoryItemAsync(long inventoryItemId, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = InventoryTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {FgsPurchaseOrderSql.InventoryItemTable}
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
