using Dapper;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Abstractions.Persistence;
using Fgs.Inventory.Application.Abstractions.InventoryItemTypes;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.InventoryItemTypes.Dtos;
using Fgs.Inventory.Infrastructure.Common;
using Fgs.MultiTenancy;

namespace Fgs.Inventory.Infrastructure.InventoryItemTypes;

internal sealed class FgsInventoryItemTypeReadRepository : IFgsInventoryItemTypeReadRepository
{
    private readonly IInventoryReadConnectionFactory _connectionFactory;
    private readonly ITenantContextAccessor _tenantContextAccessor;

    public FgsInventoryItemTypeReadRepository(
        IInventoryReadConnectionFactory connectionFactory,
        ITenantContextAccessor tenantContextAccessor)
    {
        _connectionFactory = connectionFactory;
        _tenantContextAccessor = tenantContextAccessor;
    }

    public async Task<FgsInventoryItemTypeDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = InventoryTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT {FgsInventoryItemTypeSql.SelectDetailColumns}
            FROM {FgsInventoryItemTypeSql.Table}
            WHERE "Id" = @Id AND "TenantId" = @TenantId AND "CompanyId" = @CompanyId
            """;
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var row = await connection.QueryFirstOrDefaultAsync<FgsInventoryItemTypeDetailRow>(
            new CommandDefinition(sql, new { Id = id, TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));
        return row?.ToDto();
    }

    public async Task<PagedResult<FgsInventoryItemTypeSummaryDto>> ListAsync(
        InventoryListQuery query,
        FgsInventoryItemTypeListFilters filters,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = InventoryTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var paging = query.ToPagedQuery();
        var page = Math.Max(1, paging.Page);
        var pageSize = Math.Clamp(paging.PageSize, 1, 200);
        var offset = (page - 1) * pageSize;
        var where = new List<string> { "\"TenantId\" = @TenantId", "\"CompanyId\" = @CompanyId" };
        if (paging.IsActive.HasValue) where.Add("\"IsActive\" = @IsActive");
        if (!string.IsNullOrWhiteSpace(filters.ItemTypeCode)) where.Add("\"ItemTypeCode\" = @ItemTypeCode");
        if (!string.IsNullOrWhiteSpace(filters.Name)) where.Add("\"Name\" ILIKE @Name");
        if (!string.IsNullOrWhiteSpace(paging.Search))
            where.Add("(\"ItemTypeCode\" ILIKE @Search OR \"Name\" ILIKE @Search OR \"Description\" ILIKE @Search)");
        var whereClause = string.Join(" AND ", where);
        var orderBy = FgsInventoryItemTypeSql.ResolveOrderBy(paging.SortBy, paging.SortDirection);
        var sql = $"""
            SELECT {FgsInventoryItemTypeSql.SelectSummaryColumns}
            FROM {FgsInventoryItemTypeSql.Table}
            WHERE {whereClause}
            {orderBy}
            LIMIT @PageSize OFFSET @Offset;
            SELECT COUNT(*) FROM {FgsInventoryItemTypeSql.Table} WHERE {whereClause};
            """;
        var parameters = new
        {
            TenantId = tenantId,
            CompanyId = companyId,
            IsActive = paging.IsActive,
            ItemTypeCode = filters.ItemTypeCode?.Trim().ToUpperInvariant(),
            Name = string.IsNullOrWhiteSpace(filters.Name) ? null : $"%{filters.Name.Trim()}%",
            Search = string.IsNullOrWhiteSpace(paging.Search) ? null : $"%{paging.Search.Trim()}%",
            PageSize = pageSize,
            Offset = offset
        };
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));
        var rows = (await multi.ReadAsync<FgsInventoryItemTypeSummaryRow>()).ToList();
        var totalCount = await multi.ReadSingleAsync<int>();
        return new PagedResult<FgsInventoryItemTypeSummaryDto>(rows.Select(r => r.ToDto()).ToList(), page, pageSize, totalCount);
    }

    public async Task<IReadOnlyList<FgsInventoryItemTypeLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = InventoryTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var activeFilter = activeOnly ? "AND \"IsActive\" = TRUE" : string.Empty;
        var sql = $"""
            SELECT {FgsInventoryItemTypeSql.SelectLookupColumns}
            FROM {FgsInventoryItemTypeSql.Table}
            WHERE "TenantId" = @TenantId AND "CompanyId" = @CompanyId {activeFilter}
            ORDER BY "DisplayOrder" ASC, "Name" ASC
            """;
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<FgsInventoryItemTypeLookupRow>(
            new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));
        return rows.Select(r => r.ToDto()).ToList();
    }

    public async Task<bool> ExistsByItemTypeCodeAsync(
        string itemTypeCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = InventoryTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT EXISTS(
                SELECT 1 FROM {FgsInventoryItemTypeSql.Table}
                WHERE "TenantId" = @TenantId AND "CompanyId" = @CompanyId AND "ItemTypeCode" = @ItemTypeCode
                {(excludeId.HasValue ? "AND \"Id\" <> @ExcludeId" : string.Empty)}
            )
            """;
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(sql, new
            {
                TenantId = tenantId,
                CompanyId = companyId,
                ItemTypeCode = itemTypeCode.Trim().ToUpperInvariant(),
                ExcludeId = excludeId
            }, cancellationToken: cancellationToken));
    }

    public async Task<bool> ExistsAsync(long id, bool activeOnly = true, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = InventoryTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var activeFilter = activeOnly ? "AND \"IsActive\" = TRUE" : string.Empty;
        var sql = $"""
            SELECT EXISTS(
                SELECT 1 FROM {FgsInventoryItemTypeSql.Table}
                WHERE "Id" = @Id AND "TenantId" = @TenantId AND "CompanyId" = @CompanyId {activeFilter}
            )
            """;
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(sql, new { Id = id, TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));
    }
}
