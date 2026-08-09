using Dapper;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Abstractions.Persistence;
using Fgs.Inventory.Application.Abstractions.TruckStockTemplates;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.TruckStockTemplates.Dtos;
using Fgs.Inventory.Infrastructure.Common;
using Fgs.MultiTenancy;

namespace Fgs.Inventory.Infrastructure.TruckStockTemplates;

internal sealed class FgsTruckStockTemplateReadRepository : IFgsTruckStockTemplateReadRepository
{
    private readonly IInventoryReadConnectionFactory _connectionFactory;
    private readonly ITenantContextAccessor _tenantContextAccessor;

    public FgsTruckStockTemplateReadRepository(
        IInventoryReadConnectionFactory connectionFactory,
        ITenantContextAccessor tenantContextAccessor)
    {
        _connectionFactory = connectionFactory;
        _tenantContextAccessor = tenantContextAccessor;
    }

    public async Task<FgsTruckStockTemplateDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = InventoryTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT {FgsTruckStockTemplateSql.SelectDetailColumns}
            FROM {FgsTruckStockTemplateSql.Table}
            WHERE "Id" = @Id
              AND "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId;

            SELECT {FgsTruckStockTemplateSql.SelectItemColumns}
            FROM {FgsTruckStockTemplateSql.ItemTable}
            WHERE "TruckStockTemplateId" = @Id
              AND "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
            ORDER BY "DisplayOrder" ASC, "Id" ASC;
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, new { Id = id, TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        var row = await multi.ReadFirstOrDefaultAsync<FgsTruckStockTemplateDetailRow>();
        if (row is null)
        {
            return null;
        }

        var items = (await multi.ReadAsync<FgsTruckStockTemplateItemRow>())
            .Select(i => i.ToDto())
            .ToList();

        return row.ToDto(items);
    }

    public async Task<PagedResult<FgsTruckStockTemplateSummaryDto>> ListAsync(
        InventoryListQuery query,
        FgsTruckStockTemplateListFilters filters,
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

        if (!string.IsNullOrWhiteSpace(filters.TemplateCode))
        {
            where.Add("\"TemplateCode\" = @TemplateCode");
        }

        if (!string.IsNullOrWhiteSpace(filters.Name))
        {
            where.Add("\"Name\" ILIKE @Name");
        }

        if (!string.IsNullOrWhiteSpace(paging.Search))
        {
            where.Add("(\"TemplateCode\" ILIKE @Search OR \"Name\" ILIKE @Search OR \"Description\" ILIKE @Search)");
        }

        var whereClause = string.Join(" AND ", where);
        var orderBy = FgsTruckStockTemplateSql.ResolveOrderBy(paging.SortBy, paging.SortDirection);

        var sql = $"""
            SELECT {FgsTruckStockTemplateSql.SelectSummaryColumns}
            FROM {FgsTruckStockTemplateSql.Table}
            WHERE {whereClause}
            {orderBy}
            LIMIT @PageSize OFFSET @Offset;

            SELECT COUNT(*)
            FROM {FgsTruckStockTemplateSql.Table}
            WHERE {whereClause};
            """;

        var parameters = new
        {
            TenantId = tenantId,
            CompanyId = companyId,
            IsActive = paging.IsActive,
            TemplateCode = filters.TemplateCode?.Trim().ToUpperInvariant(),
            Name = string.IsNullOrWhiteSpace(filters.Name) ? null : $"%{filters.Name.Trim()}%",
            Search = string.IsNullOrWhiteSpace(paging.Search) ? null : $"%{paging.Search.Trim()}%",
            PageSize = pageSize,
            Offset = offset
        };

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

        var rows = (await multi.ReadAsync<FgsTruckStockTemplateSummaryRow>()).ToList();
        var totalCount = await multi.ReadSingleAsync<int>();

        return new PagedResult<FgsTruckStockTemplateSummaryDto>(
            rows.Select(r => r.ToDto()).ToList(),
            page,
            pageSize,
            totalCount);
    }

    public async Task<IReadOnlyList<FgsTruckStockTemplateLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = InventoryTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var activeFilter = activeOnly ? "AND \"IsActive\" = TRUE" : string.Empty;
        var sql = $"""
            SELECT {FgsTruckStockTemplateSql.SelectLookupColumns}
            FROM {FgsTruckStockTemplateSql.Table}
            WHERE "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
              {activeFilter}
            ORDER BY "Name" ASC
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<FgsTruckStockTemplateLookupRow>(
            new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return rows.Select(r => r.ToDto()).ToList();
    }

    public async Task<bool> ExistsByTemplateCodeAsync(
        string templateCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = InventoryTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {FgsTruckStockTemplateSql.Table}
                WHERE "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND "TemplateCode" = @TemplateCode
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
                    TemplateCode = templateCode.Trim().ToUpperInvariant(),
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
                FROM {FgsTruckStockTemplateSql.Table}
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
                FROM {FgsTruckStockTemplateSql.InventoryItemTable}
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
