using Dapper;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Persistence;
using Fgs.Setup.Application.Abstractions.UniversalMatrixItems;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.UniversalMatrixItems.Dtos;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.UniversalPricingMatrix.UniversalMatrixItems;

internal sealed class FgsUniversalMatrixItemReadRepository : IFgsUniversalMatrixItemReadRepository
{
    private readonly ISetupReadConnectionFactory _connectionFactory;
    private readonly ITenantContextAccessor _tenantContextAccessor;

    public FgsUniversalMatrixItemReadRepository(
        ISetupReadConnectionFactory connectionFactory,
        ITenantContextAccessor tenantContextAccessor)
    {
        _connectionFactory = connectionFactory;
        _tenantContextAccessor = tenantContextAccessor;
    }

    public async Task<FgsUniversalMatrixItemDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT {FgsUniversalMatrixItemSql.SelectDetailColumns}
            FROM {FgsUniversalMatrixItemSql.Table}
            WHERE "Id" = @Id
              AND "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var row = await connection.QueryFirstOrDefaultAsync<FgsUniversalMatrixItemDetailRow>(
            new CommandDefinition(sql, new { Id = id, TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return row?.ToDto();
    }

    public async Task<PagedResult<FgsUniversalMatrixItemSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsUniversalMatrixItemListFilters filters,
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

        if (!string.IsNullOrWhiteSpace(filters.ItemName))
        {
            where.Add("\"ItemName\" ILIKE @ItemName");
        }
        if (filters.UniversalPricingServiceId.HasValue)
        {
            where.Add("\"UniversalPricingServiceId\" = @UniversalPricingServiceId");
        }

        if (!string.IsNullOrWhiteSpace(paging.Search))
        {
            where.Add(
                "(\"ItemName\" ILIKE @Search OR \"UnitType\" ILIKE @Search)");
        }

        var whereClause = string.Join(" AND ", where);
        var orderBy = FgsUniversalMatrixItemSql.ResolveOrderBy(paging.SortBy, paging.SortDirection);

        var sql = $"""
            SELECT {FgsUniversalMatrixItemSql.SelectSummaryColumns}
            FROM {FgsUniversalMatrixItemSql.Table}
            WHERE {whereClause}
            {orderBy}
            LIMIT @PageSize OFFSET @Offset;

            SELECT COUNT(*)
            FROM {FgsUniversalMatrixItemSql.Table}
            WHERE {whereClause};
            """;

        var parameters = new
        {
            TenantId = tenantId,
            CompanyId = companyId,
            IsActive = paging.IsActive,
            ItemName = string.IsNullOrWhiteSpace(filters.ItemName) ? null : $"%{filters.ItemName.Trim()}%",
            UniversalPricingServiceId = filters.UniversalPricingServiceId,
            Search = string.IsNullOrWhiteSpace(paging.Search) ? null : $"%{paging.Search.Trim()}%",
            PageSize = pageSize,
            Offset = offset
        };

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

        var rows = (await multi.ReadAsync<FgsUniversalMatrixItemSummaryRow>()).ToList();
        var totalCount = await multi.ReadSingleAsync<int>();

        return new PagedResult<FgsUniversalMatrixItemSummaryDto>(
            rows.Select(r => r.ToDto()).ToList(),
            page,
            pageSize,
            totalCount);
    }

    public async Task<IReadOnlyList<FgsUniversalMatrixItemLookupDto>> LookupAsync(
        bool activeOnly = true,
        long? universalPricingServiceId = null,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var activeFilter = activeOnly ? "AND \"IsActive\" = TRUE" : string.Empty;
        var universalPricingServiceFilter = universalPricingServiceId.HasValue
            ? "AND \"UniversalPricingServiceId\" = @UniversalPricingServiceId"
            : string.Empty;
        var sql = $"""
            SELECT {FgsUniversalMatrixItemSql.SelectLookupColumns}
            FROM {FgsUniversalMatrixItemSql.Table}
            WHERE "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
              {activeFilter}
              {universalPricingServiceFilter}
            ORDER BY "DisplayOrder" ASC NULLS LAST, "ItemName" ASC
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<FgsUniversalMatrixItemLookupRow>(
            new CommandDefinition(
                sql,
                new { TenantId = tenantId, CompanyId = companyId, UniversalPricingServiceId = universalPricingServiceId },
                cancellationToken: cancellationToken));

        return rows.Select(r => r.ToDto()).ToList();
    }

    public async Task<bool> ExistsByUniversalPricingServiceIdAndItemNameAsync(
        long universalPricingServiceId, string itemName,
        long? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {FgsUniversalMatrixItemSql.Table}
                WHERE "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND "UniversalPricingServiceId" = @UniversalPricingServiceId AND "ItemName" = @ItemName
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
                    UniversalPricingServiceId = universalPricingServiceId,
                    ItemName = itemName.Trim(),
                    ExcludeId = excludeId
                },
                cancellationToken: cancellationToken));
    }
    public async Task<bool> ExistsUniversalPricingServiceIdAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM setup."FgsUniversalPricingService"
                WHERE "TenantId" = @TenantId AND "CompanyId" = @CompanyId AND "Id" = @Id AND "IsActive" = TRUE
            )
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                sql,
                new { TenantId = tenantId, CompanyId = companyId, Id = id },
                cancellationToken: cancellationToken));
    }
}
