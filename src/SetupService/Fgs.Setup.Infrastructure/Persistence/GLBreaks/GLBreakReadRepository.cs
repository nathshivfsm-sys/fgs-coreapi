using Dapper;
using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.GLBreaks;
using Fgs.Setup.Application.Abstractions.Persistence;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.GLBreaks.Dtos;

namespace Fgs.Setup.Infrastructure.Persistence.GLBreaks;

internal sealed class GLBreakReadRepository : IGLBreakReadRepository
{
    private readonly ISetupReadConnectionFactory _connectionFactory;
    private readonly ITenantContextAccessor _tenantContextAccessor;

    public GLBreakReadRepository(
        ISetupReadConnectionFactory connectionFactory,
        ITenantContextAccessor tenantContextAccessor)
    {
        _connectionFactory = connectionFactory;
        _tenantContextAccessor = tenantContextAccessor;
    }

    public async Task<GLBreakDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT {GLBreakSql.SelectDetailColumns}
            FROM {GLBreakSql.Table} glb
            {GLBreakSql.LocationJoin}
            WHERE glb."Id" = @Id
              AND glb."TenantId" = @TenantId
              AND glb."CompanyId" = @CompanyId
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var row = await connection.QueryFirstOrDefaultAsync<GLBreakDetailRow>(
            new CommandDefinition(sql, new { Id = id, TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        if (row is null)
        {
            return null;
        }

        var trades = await LoadTradesAsync(connection, id, tenantId, companyId, cancellationToken);
        return row.ToDto(trades);
    }

    public async Task<PagedResult<GLBreakSummaryDto>> ListAsync(
        SetupListQuery query,
        GLBreakListFilters filters,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var paging = query.ToPagedQuery();
        var page = Math.Max(1, paging.Page);
        var pageSize = Math.Clamp(paging.PageSize, 1, 200);
        var offset = (page - 1) * pageSize;

        var where = new List<string>
        {
            "glb.\"TenantId\" = @TenantId",
            "glb.\"CompanyId\" = @CompanyId"
        };

        if (paging.IsActive.HasValue)
        {
            where.Add("glb.\"IsActive\" = @IsActive");
        }

        if (!string.IsNullOrWhiteSpace(filters.Code))
        {
            where.Add("glb.\"Code\" = @Code");
        }

        if (!string.IsNullOrWhiteSpace(filters.Name))
        {
            where.Add("glb.\"Name\" ILIKE @Name");
        }

        if (filters.BreakLevel.HasValue)
        {
            where.Add("glb.\"BreakLevel\" = @BreakLevel");
        }

        if (!string.IsNullOrWhiteSpace(filters.TradeCode))
        {
            where.Add("""
                EXISTS (
                    SELECT 1
                    FROM setup."FgsSetupGLBreakTrade" t
                    WHERE t."GLBreakId" = glb."Id"
                      AND t."TenantId" = glb."TenantId"
                      AND t."CompanyId" = glb."CompanyId"
                      AND t."TradeCode" = @TradeCode
                )
                """);
        }

        if (!string.IsNullOrWhiteSpace(paging.Search))
        {
            where.Add(
                "(glb.\"Code\" ILIKE @Search OR glb.\"Name\" ILIKE @Search OR glb.\"BreakLabel\" ILIKE @Search)");
        }

        var whereClause = string.Join(" AND ", where);
        var orderBy = GLBreakSql.ResolveOrderBy(paging.SortBy, paging.SortDirection)
            .Replace("ORDER BY \"", "ORDER BY glb.\"");

        var sql = $"""
            SELECT {GLBreakSql.SelectSummaryColumns}
            FROM {GLBreakSql.Table} glb
            WHERE {whereClause}
            {orderBy}
            LIMIT @PageSize OFFSET @Offset;

            SELECT COUNT(*)
            FROM {GLBreakSql.Table} glb
            WHERE {whereClause};
            """;

        var parameters = new
        {
            TenantId = tenantId,
            CompanyId = companyId,
            IsActive = paging.IsActive,
            Code = filters.Code?.Trim(),
            Name = string.IsNullOrWhiteSpace(filters.Name) ? null : $"%{filters.Name.Trim()}%",
            BreakLevel = filters.BreakLevel,
            TradeCode = filters.TradeCode?.Trim().ToUpperInvariant(),
            Search = string.IsNullOrWhiteSpace(paging.Search) ? null : $"%{paging.Search.Trim()}%",
            PageSize = pageSize,
            Offset = offset
        };

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

        var rows = (await multi.ReadAsync<GLBreakSummaryRow>()).ToList();
        var totalCount = await multi.ReadSingleAsync<int>();

        return new PagedResult<GLBreakSummaryDto>(
            rows.Select(r => r.ToDto()).ToList(),
            page,
            pageSize,
            totalCount);
    }

    public async Task<IReadOnlyList<GLBreakLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var activeFilter = activeOnly ? "AND \"IsActive\" = TRUE" : string.Empty;
        var sql = $"""
            SELECT {GLBreakSql.SelectLookupColumns}
            FROM {GLBreakSql.Table}
            WHERE "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
              {activeFilter}
            ORDER BY "BreakLevel" ASC, "Name" ASC
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<GLBreakLookupRow>(
            new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return rows.Select(r => r.ToDto()).ToList();
    }

    public async Task<bool> ExistsByCodeAndBreakLevelAsync(
        string code,
        short breakLevel,
        long? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {GLBreakSql.Table}
                WHERE "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND "Code" = @Code
                  AND "BreakLevel" = @BreakLevel
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
                    Code = code.Trim(),
                    BreakLevel = breakLevel,
                    ExcludeId = excludeId
                },
                cancellationToken: cancellationToken));
    }

    public async Task<short?> GetBreakLevelByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT "BreakLevel"
            FROM {GLBreakSql.Table}
            WHERE "Id" = @Id
              AND "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.QueryFirstOrDefaultAsync<short?>(
            new CommandDefinition(
                sql,
                new { Id = id, TenantId = tenantId, CompanyId = companyId },
                cancellationToken: cancellationToken));
    }

    private static async Task<IReadOnlyList<GLBreakTradeDto>> LoadTradesAsync(
        System.Data.Common.DbConnection connection,
        long glBreakId,
        long tenantId,
        long companyId,
        CancellationToken cancellationToken)
    {
        var sql = $"""
            SELECT {GLBreakSql.SelectTradeColumns}
            FROM {GLBreakSql.TradeTable}
            WHERE "GLBreakId" = @GLBreakId
              AND "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
            ORDER BY "TradeCode" ASC
            """;

        var rows = await connection.QueryAsync<GLBreakTradeRow>(
            new CommandDefinition(
                sql,
                new { GLBreakId = glBreakId, TenantId = tenantId, CompanyId = companyId },
                cancellationToken: cancellationToken));

        return rows.Select(r => r.ToDto()).ToList();
    }
}
