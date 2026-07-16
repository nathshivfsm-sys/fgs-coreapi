using Dapper;
using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Persistence;
using Fgs.Setup.Application.Abstractions.TechTrades;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.TechTrades.Dtos;

namespace Fgs.Setup.Infrastructure.Persistence.TechTrades;

internal sealed class TechTradeReadRepository : ITechTradeReadRepository
{
    private readonly ISetupReadConnectionFactory _connectionFactory;
    private readonly ITenantContextAccessor _tenantContextAccessor;

    public TechTradeReadRepository(
        ISetupReadConnectionFactory connectionFactory,
        ITenantContextAccessor tenantContextAccessor)
    {
        _connectionFactory = connectionFactory;
        _tenantContextAccessor = tenantContextAccessor;
    }

    public async Task<TechTradeDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT {TechTradeSql.SelectDetailColumns}
            FROM {TechTradeSql.Table}
            WHERE "Id" = @Id
              AND "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var row = await connection.QueryFirstOrDefaultAsync<TechTradeDetailRow>(
            new CommandDefinition(sql, new { Id = id, TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return row?.ToDto();
    }

    public async Task<PagedResult<TechTradeSummaryDto>> ListAsync(
        SetupListQuery query,
        TechTradeListFilters filters,
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

        if (!string.IsNullOrWhiteSpace(filters.TradeCode))
        {
            where.Add("\"TradeCode\" = @TradeCode");
        }

        if (!string.IsNullOrWhiteSpace(filters.Name))
        {
            where.Add("\"Name\" ILIKE @Name");
        }

        if (!string.IsNullOrWhiteSpace(paging.Search))
        {
            where.Add(
                "(\"TradeCode\" ILIKE @Search OR \"Name\" ILIKE @Search OR \"Description\" ILIKE @Search)");
        }

        var whereClause = string.Join(" AND ", where);
        var orderBy = TechTradeSql.ResolveOrderBy(paging.SortBy, paging.SortDirection);

        var sql = $"""
            SELECT {TechTradeSql.SelectSummaryColumns}
            FROM {TechTradeSql.Table}
            WHERE {whereClause}
            {orderBy}
            LIMIT @PageSize OFFSET @Offset;

            SELECT COUNT(*)
            FROM {TechTradeSql.Table}
            WHERE {whereClause};
            """;

        var parameters = new
        {
            TenantId = tenantId,
            CompanyId = companyId,
            IsActive = paging.IsActive,
            TradeCode = filters.TradeCode?.Trim().ToUpperInvariant(),
            Name = string.IsNullOrWhiteSpace(filters.Name) ? null : $"%{filters.Name.Trim()}%",
            Search = string.IsNullOrWhiteSpace(paging.Search) ? null : $"%{paging.Search.Trim()}%",
            PageSize = pageSize,
            Offset = offset
        };

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

        var rows = (await multi.ReadAsync<TechTradeSummaryRow>()).ToList();
        var totalCount = await multi.ReadSingleAsync<int>();

        return new PagedResult<TechTradeSummaryDto>(
            rows.Select(r => r.ToDto()).ToList(),
            page,
            pageSize,
            totalCount);
    }

    public async Task<IReadOnlyList<TechTradeLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var activeFilter = activeOnly ? "AND \"IsActive\" = TRUE" : string.Empty;
        var sql = $"""
            SELECT {TechTradeSql.SelectLookupColumns}
            FROM {TechTradeSql.Table}
            WHERE "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
              {activeFilter}
            ORDER BY "SortOrder" ASC NULLS LAST, "Name" ASC
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<TechTradeLookupRow>(
            new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return rows.Select(r => r.ToDto()).ToList();
    }

    public async Task<bool> ExistsByTradeCodeAsync(
        string tradeCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {TechTradeSql.Table}
                WHERE "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND "TradeCode" = @TradeCode
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
                    TradeCode = tradeCode.Trim().ToUpperInvariant(),
                    ExcludeId = excludeId
                },
                cancellationToken: cancellationToken));
    }

    public async Task<bool> ExistsByNameAsync(
        string name,
        long? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {TechTradeSql.Table}
                WHERE "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND "IsActive" = TRUE
                  AND LOWER("Name") = LOWER(@Name)
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
                    Name = name.Trim(),
                    ExcludeId = excludeId
                },
                cancellationToken: cancellationToken));
    }

    public async Task<bool> ExistsActiveTradeCodeAsync(
        string tradeCode,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {TechTradeSql.Table}
                WHERE "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND "TradeCode" = @TradeCode
                  AND "IsActive" = TRUE
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
                    TradeCode = tradeCode.Trim().ToUpperInvariant()
                },
                cancellationToken: cancellationToken));
    }
}
