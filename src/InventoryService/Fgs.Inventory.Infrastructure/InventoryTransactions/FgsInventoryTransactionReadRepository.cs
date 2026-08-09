using Dapper;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Abstractions.InventoryTransactions;
using Fgs.Inventory.Application.Abstractions.Persistence;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.InventoryTransactions.Dtos;
using Fgs.Inventory.Infrastructure.Common;
using Fgs.MultiTenancy;

namespace Fgs.Inventory.Infrastructure.InventoryTransactions;

internal sealed class FgsInventoryTransactionReadRepository : IFgsInventoryTransactionReadRepository
{
    private readonly IInventoryReadConnectionFactory _connectionFactory;
    private readonly ITenantContextAccessor _tenantContextAccessor;

    public FgsInventoryTransactionReadRepository(
        IInventoryReadConnectionFactory connectionFactory,
        ITenantContextAccessor tenantContextAccessor)
    {
        _connectionFactory = connectionFactory;
        _tenantContextAccessor = tenantContextAccessor;
    }

    public async Task<FgsInventoryTransactionDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = InventoryTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT {FgsInventoryTransactionSql.SelectDetailColumns}
            FROM {FgsInventoryTransactionSql.Table}
            WHERE "Id" = @Id
              AND "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var row = await connection.QueryFirstOrDefaultAsync<FgsInventoryTransactionDetailRow>(
            new CommandDefinition(sql, new { Id = id, TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return row?.ToDto();
    }

    public async Task<PagedResult<FgsInventoryTransactionSummaryDto>> ListAsync(
        InventoryListQuery query,
        FgsInventoryTransactionListFilters filters,
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

        if (!string.IsNullOrWhiteSpace(filters.TransactionType))
        {
            where.Add("\"TransactionType\" = @TransactionType");
        }

        if (!string.IsNullOrWhiteSpace(paging.Search))
        {
            where.Add(
                "(\"TransactionNumber\" ILIKE @Search OR \"ReferenceType\" ILIKE @Search OR \"Notes\" ILIKE @Search)");
        }

        var whereClause = string.Join(" AND ", where);
        var orderBy = FgsInventoryTransactionSql.ResolveOrderBy(paging.SortBy, paging.SortDirection);

        var sql = $"""
            SELECT {FgsInventoryTransactionSql.SelectSummaryColumns}
            FROM {FgsInventoryTransactionSql.Table}
            WHERE {whereClause}
            {orderBy}
            LIMIT @PageSize OFFSET @Offset;

            SELECT COUNT(*)
            FROM {FgsInventoryTransactionSql.Table}
            WHERE {whereClause};
            """;

        var parameters = new
        {
            TenantId = tenantId,
            CompanyId = companyId,
            InventoryItemId = filters.InventoryItemId,
            TransactionType = filters.TransactionType?.Trim().ToUpperInvariant(),
            Search = string.IsNullOrWhiteSpace(paging.Search) ? null : $"%{paging.Search.Trim()}%",
            PageSize = pageSize,
            Offset = offset
        };

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

        var rows = (await multi.ReadAsync<FgsInventoryTransactionSummaryRow>()).ToList();
        var totalCount = await multi.ReadSingleAsync<int>();

        return new PagedResult<FgsInventoryTransactionSummaryDto>(
            rows.Select(r => r.ToDto()).ToList(),
            page,
            pageSize,
            totalCount);
    }

    public async Task<bool> ExistsByTransactionNumberAsync(
        string transactionNumber,
        long? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = InventoryTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {FgsInventoryTransactionSql.Table}
                WHERE "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND "TransactionNumber" = @TransactionNumber
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
                    TransactionNumber = transactionNumber.Trim(),
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
                FROM {FgsInventoryTransactionSql.InventoryItemTable}
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
