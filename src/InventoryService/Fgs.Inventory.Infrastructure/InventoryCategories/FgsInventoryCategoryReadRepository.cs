using Dapper;
using Fgs.Foundation.Paging;
using Fgs.Inventory.Application.Abstractions.InventoryCategories;
using Fgs.Inventory.Application.Abstractions.Persistence;
using Fgs.Inventory.Application.Common.InventoryCrud;
using Fgs.Inventory.Application.Features.InventoryCategories.Dtos;
using Fgs.Inventory.Infrastructure.Common;
using Fgs.Inventory.Infrastructure.InventorySubCategories;
using Fgs.MultiTenancy;

namespace Fgs.Inventory.Infrastructure.InventoryCategories;

internal sealed class FgsInventoryCategoryReadRepository : IFgsInventoryCategoryReadRepository
{
    private readonly IInventoryReadConnectionFactory _connectionFactory;
    private readonly ITenantContextAccessor _tenantContextAccessor;

    public FgsInventoryCategoryReadRepository(
        IInventoryReadConnectionFactory connectionFactory,
        ITenantContextAccessor tenantContextAccessor)
    {
        _connectionFactory = connectionFactory;
        _tenantContextAccessor = tenantContextAccessor;
    }

    public async Task<FgsInventoryCategoryDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = InventoryTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT {FgsInventoryCategorySql.SelectDetailColumns}
            FROM {FgsInventoryCategorySql.Table}
            WHERE "Id" = @Id AND "TenantId" = @TenantId AND "CompanyId" = @CompanyId
            """;
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var row = await connection.QueryFirstOrDefaultAsync<FgsInventoryCategoryDetailRow>(
            new CommandDefinition(sql, new { Id = id, TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));
        return row?.ToDto();
    }

    public async Task<PagedResult<FgsInventoryCategorySummaryDto>> ListAsync(
        InventoryListQuery query,
        FgsInventoryCategoryListFilters filters,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = InventoryTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var paging = query.ToPagedQuery();
        var page = Math.Max(1, paging.Page);
        var pageSize = Math.Clamp(paging.PageSize, 1, 200);
        var offset = (page - 1) * pageSize;
        var where = new List<string> { "\"TenantId\" = @TenantId", "\"CompanyId\" = @CompanyId" };
        if (paging.IsActive.HasValue) where.Add("\"IsActive\" = @IsActive");
        if (!string.IsNullOrWhiteSpace(filters.CategoryCode)) where.Add("\"CategoryCode\" = @CategoryCode");
        if (!string.IsNullOrWhiteSpace(filters.Name)) where.Add("\"Name\" ILIKE @Name");
        if (!string.IsNullOrWhiteSpace(paging.Search))
            where.Add("(\"CategoryCode\" ILIKE @Search OR \"Name\" ILIKE @Search OR \"Description\" ILIKE @Search)");
        var whereClause = string.Join(" AND ", where);
        var orderBy = FgsInventoryCategorySql.ResolveOrderBy(paging.SortBy, paging.SortDirection);
        var sql = $"""
            SELECT {FgsInventoryCategorySql.SelectSummaryColumns}
            FROM {FgsInventoryCategorySql.Table}
            WHERE {whereClause}
            {orderBy}
            LIMIT @PageSize OFFSET @Offset;
            SELECT COUNT(*) FROM {FgsInventoryCategorySql.Table} WHERE {whereClause};
            """;
        var parameters = new
        {
            TenantId = tenantId,
            CompanyId = companyId,
            IsActive = paging.IsActive,
            CategoryCode = filters.CategoryCode?.Trim().ToUpperInvariant(),
            Name = string.IsNullOrWhiteSpace(filters.Name) ? null : $"%{filters.Name.Trim()}%",
            Search = string.IsNullOrWhiteSpace(paging.Search) ? null : $"%{paging.Search.Trim()}%",
            PageSize = pageSize,
            Offset = offset
        };
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));
        var rows = (await multi.ReadAsync<FgsInventoryCategorySummaryRow>()).ToList();
        var totalCount = await multi.ReadSingleAsync<int>();
        return new PagedResult<FgsInventoryCategorySummaryDto>(rows.Select(r => r.ToDto()).ToList(), page, pageSize, totalCount);
    }

    public async Task<IReadOnlyList<FgsInventoryCategoryLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = InventoryTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var activeFilter = activeOnly ? "AND \"IsActive\" = TRUE" : string.Empty;
        var sql = $"""
            SELECT {FgsInventoryCategorySql.SelectLookupColumns}
            FROM {FgsInventoryCategorySql.Table}
            WHERE "TenantId" = @TenantId AND "CompanyId" = @CompanyId {activeFilter}
            ORDER BY "DisplayOrder" ASC, "Name" ASC
            """;
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<FgsInventoryCategoryLookupRow>(
            new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));
        return rows.Select(r => r.ToDto()).ToList();
    }

    public async Task<bool> ExistsByCategoryCodeAsync(
        string categoryCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = InventoryTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT EXISTS(
                SELECT 1 FROM {FgsInventoryCategorySql.Table}
                WHERE "TenantId" = @TenantId AND "CompanyId" = @CompanyId AND "CategoryCode" = @CategoryCode
                {(excludeId.HasValue ? "AND \"Id\" <> @ExcludeId" : string.Empty)}
            )
            """;
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(sql, new
            {
                TenantId = tenantId,
                CompanyId = companyId,
                CategoryCode = categoryCode.Trim().ToUpperInvariant(),
                ExcludeId = excludeId
            }, cancellationToken: cancellationToken));
    }

    public async Task<bool> ExistsByNameAsync(
        string name,
        long? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = InventoryTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT EXISTS(
                SELECT 1 FROM {FgsInventoryCategorySql.Table}
                WHERE "TenantId" = @TenantId AND "CompanyId" = @CompanyId
                  AND LOWER("Name") = LOWER(@Name)
                {(excludeId.HasValue ? "AND \"Id\" <> @ExcludeId" : string.Empty)}
            )
            """;
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(sql, new
            {
                TenantId = tenantId,
                CompanyId = companyId,
                Name = name.Trim(),
                ExcludeId = excludeId
            }, cancellationToken: cancellationToken));
    }

    public async Task<PagedResult<FgsInventoryCategoryWithSubCategoriesDto>> ListWithSubCategoriesAsync(
        InventoryListQuery query,
        FgsInventoryCategoryListFilters filters,
        CancellationToken cancellationToken = default)
    {
        var page = await ListAsync(query, filters, cancellationToken);
        if (page.Items.Count == 0)
        {
            return new PagedResult<FgsInventoryCategoryWithSubCategoriesDto>(
                [], page.Page, page.PageSize, page.TotalCount);
        }

        var (tenantId, companyId) = InventoryTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var categoryIds = page.Items.Select(c => c.Id).ToArray();
        var subActiveFilter = query.IsActive.HasValue ? "AND \"IsActive\" = @IsActive" : string.Empty;
        var sql = $"""
            SELECT {FgsInventorySubCategorySql.SelectSummaryColumns}
            FROM {FgsInventorySubCategorySql.Table}
            WHERE "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
              AND "InventoryCategoryId" = ANY(@CategoryIds)
              {subActiveFilter}
            ORDER BY "DisplayOrder" ASC, "Id" ASC
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var subRows = (await connection.QueryAsync<FgsInventorySubCategorySummaryRow>(
            new CommandDefinition(
                sql,
                new
                {
                    TenantId = tenantId,
                    CompanyId = companyId,
                    CategoryIds = categoryIds,
                    IsActive = query.IsActive
                },
                cancellationToken: cancellationToken))).ToList();

        var subsByCategory = subRows
            .GroupBy(r => r.InventoryCategoryId)
            .ToDictionary(
                g => g.Key,
                g => (IReadOnlyList<FgsInventorySubCategoryNestedDto>)g
                    .Select(r => new FgsInventorySubCategoryNestedDto(
                        r.Id,
                        r.SubCategoryCode,
                        r.Name,
                        r.Description,
                        r.DisplayOrder,
                        r.IsSystem,
                        r.IsActive))
                    .ToList());

        var items = page.Items.Select(c => new FgsInventoryCategoryWithSubCategoriesDto(
            c.Id,
            c.CategoryCode,
            c.Name,
            c.Description,
            c.TextColor,
            c.BackgroundColor,
            c.DisplayIconFileId,
            c.DisplayOrder,
            c.IsSystem,
            c.IsActive,
            subsByCategory.TryGetValue(c.Id, out var subs) ? subs : [])).ToList();

        return new PagedResult<FgsInventoryCategoryWithSubCategoriesDto>(
            items, page.Page, page.PageSize, page.TotalCount);
    }

    public async Task<bool> ExistsAsync(long id, bool activeOnly = true, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = InventoryTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var activeFilter = activeOnly ? "AND \"IsActive\" = TRUE" : string.Empty;
        var sql = $"""
            SELECT EXISTS(
                SELECT 1 FROM {FgsInventoryCategorySql.Table}
                WHERE "Id" = @Id AND "TenantId" = @TenantId AND "CompanyId" = @CompanyId {activeFilter}
            )
            """;
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(sql, new { Id = id, TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));
    }
}
