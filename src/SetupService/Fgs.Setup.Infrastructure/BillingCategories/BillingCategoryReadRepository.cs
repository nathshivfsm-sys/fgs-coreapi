using Dapper;
using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Persistence;
using Fgs.Setup.Application.Abstractions.BillingCategories;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.BillingCategories.Dtos;

namespace Fgs.Setup.Infrastructure.BillingCategories;

internal sealed class BillingCategoryReadRepository : IBillingCategoryReadRepository
{
    private readonly ISetupReadConnectionFactory _connectionFactory;
    private readonly ITenantContextAccessor _tenantContextAccessor;

    public BillingCategoryReadRepository(
        ISetupReadConnectionFactory connectionFactory,
        ITenantContextAccessor tenantContextAccessor)
    {
        _connectionFactory = connectionFactory;
        _tenantContextAccessor = tenantContextAccessor;
    }

    public async Task<BillingCategoryDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT {BillingCategorySql.SelectDetailColumns}
            FROM {BillingCategorySql.Table}
            WHERE "Id" = @Id
              AND "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var row = await connection.QueryFirstOrDefaultAsync<BillingCategoryDetailRow>(
            new CommandDefinition(sql, new { Id = id, TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return row?.ToDto();
    }

    public async Task<PagedResult<BillingCategorySummaryDto>> ListAsync(
        SetupListQuery query,
        BillingCategoryListFilters filters,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
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

        if (!string.IsNullOrWhiteSpace(filters.BillingCategoryType))
        {
            where.Add("\"BillingCategoryType\" = @BillingCategoryType");
        }
        if (!string.IsNullOrWhiteSpace(filters.BillingCategoryName))
        {
            where.Add("\"BillingCategoryName\" ILIKE @BillingCategoryName");
        }

        if (!string.IsNullOrWhiteSpace(paging.Search))
        {
            where.Add(
                "(\"BillingCategoryType\" ILIKE @Search OR \"BillingCategoryName\" ILIKE @Search OR \"Description\" ILIKE @Search)");
        }

        var whereClause = string.Join(" AND ", where);
        var orderBy = BillingCategorySql.ResolveOrderBy(paging.SortBy, paging.SortDirection);

        var sql = $"""
            SELECT {BillingCategorySql.SelectSummaryColumns}
            FROM {BillingCategorySql.Table}
            WHERE {whereClause}
            {orderBy}
            LIMIT @PageSize OFFSET @Offset;

            SELECT COUNT(*)
            FROM {BillingCategorySql.Table}
            WHERE {whereClause};
            """;

        var parameters = new
        {
            TenantId = tenantId,
            CompanyId = companyId,
            IsActive = paging.IsActive,
            BillingCategoryType = filters.BillingCategoryType?.Trim().ToUpperInvariant(),
            BillingCategoryName = string.IsNullOrWhiteSpace(filters.BillingCategoryName) ? null : $"%{filters.BillingCategoryName.Trim()}%",
            Search = string.IsNullOrWhiteSpace(paging.Search) ? null : $"%{paging.Search.Trim()}%",
            PageSize = pageSize,
            Offset = offset
        };

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

        var rows = (await multi.ReadAsync<BillingCategorySummaryRow>()).ToList();
        var totalCount = await multi.ReadSingleAsync<int>();

        return new PagedResult<BillingCategorySummaryDto>(
            rows.Select(r => r.ToDto()).ToList(),
            page,
            pageSize,
            totalCount);
    }

    public async Task<IReadOnlyList<BillingCategoryLookupDto>> LookupAsync(
        bool activeOnly = true,
        bool? showToFieldTech = null,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var filters = new List<string>
        {
            "\"IsActive\" = TRUE",
            "\"AllowToPick\" = TRUE"
        };

        if (activeOnly)
        {
            // Pickable active rows are always required; activeOnly retained for API compatibility.
        }

        if (showToFieldTech.HasValue)
        {
            filters.Add("\"ShowToFieldTech\" = @ShowToFieldTech");
        }

        var filterClause = string.Join(" AND ", filters);
        var sql = $"""
            SELECT {BillingCategorySql.SelectLookupColumns}
            FROM {BillingCategorySql.Table}
            WHERE "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
              AND {filterClause}
            ORDER BY "DisplayOrder" ASC NULLS LAST, "BillingCategoryName" ASC
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<BillingCategoryLookupRow>(
            new CommandDefinition(
                sql,
                new { TenantId = tenantId, CompanyId = companyId, ShowToFieldTech = showToFieldTech },
                cancellationToken: cancellationToken));

        return rows.Select(r => r.ToDto()).ToList();
    }

    public async Task<bool> ExistsByBillingCategoryTypeAndBillingCategoryNameAsync(
        string billingCategoryType, string billingCategoryName,
        long? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {BillingCategorySql.Table}
                WHERE "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND "BillingCategoryType" = @BillingCategoryType AND "BillingCategoryName" = @BillingCategoryName
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
                    BillingCategoryType = billingCategoryType.Trim().ToUpperInvariant(),
                    BillingCategoryName = billingCategoryName.Trim(),
                    ExcludeId = excludeId
                },
                cancellationToken: cancellationToken));
    }
}
