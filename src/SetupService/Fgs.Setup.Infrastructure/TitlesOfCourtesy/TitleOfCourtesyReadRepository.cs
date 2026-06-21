using Dapper;
using Fgs.Foundation.CatalogCrud;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Persistence;
using Fgs.Setup.Application.Abstractions.TitlesOfCourtesy;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Dtos;

namespace Fgs.Setup.Infrastructure.TitlesOfCourtesy;

internal sealed class TitleOfCourtesyReadRepository : ITitleOfCourtesyReadRepository
{
    private readonly ISetupReadConnectionFactory _connectionFactory;
    private readonly ITenantContextAccessor _tenantContextAccessor;

    public TitleOfCourtesyReadRepository(
        ISetupReadConnectionFactory connectionFactory,
        ITenantContextAccessor tenantContextAccessor)
    {
        _connectionFactory = connectionFactory;
        _tenantContextAccessor = tenantContextAccessor;
    }

    public async Task<TitleOfCourtesyDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = ResolveTenantScope();
        var sql = $"""
            SELECT {TitleOfCourtesySql.SelectDetailColumns}
            FROM {TitleOfCourtesySql.Table}
            WHERE "Id" = @Id
              AND "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var row = await connection.QueryFirstOrDefaultAsync<TitleOfCourtesyDetailRow>(
            new CommandDefinition(sql, new { Id = id, TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return row?.ToDto();
    }

    public async Task<PagedResult<TitleOfCourtesySummaryDto>> ListAsync(
        SetupListQuery query,
        TitleOfCourtesyListFilters filters,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = ResolveTenantScope();
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

        if (!string.IsNullOrWhiteSpace(filters.Code))
        {
            where.Add("\"Code\" = @Code");
        }

        if (!string.IsNullOrWhiteSpace(filters.DisplayName))
        {
            where.Add("\"DisplayName\" ILIKE @DisplayName");
        }

        if (!string.IsNullOrWhiteSpace(paging.Search))
        {
            where.Add(
                "(\"Code\" ILIKE @Search OR \"DisplayName\" ILIKE @Search)");
        }

        var whereClause = string.Join(" AND ", where);
        var orderBy = TitleOfCourtesySql.ResolveOrderBy(paging.SortBy, paging.SortDirection);

        var sql = $"""
            SELECT {TitleOfCourtesySql.SelectSummaryColumns}
            FROM {TitleOfCourtesySql.Table}
            WHERE {whereClause}
            {orderBy}
            LIMIT @PageSize OFFSET @Offset;

            SELECT COUNT(*)
            FROM {TitleOfCourtesySql.Table}
            WHERE {whereClause};
            """;

        var parameters = new
        {
            TenantId = tenantId,
            CompanyId = companyId,
            IsActive = paging.IsActive,
            Code = filters.Code?.Trim().ToUpperInvariant(),
            DisplayName = string.IsNullOrWhiteSpace(filters.DisplayName) ? null : $"%{filters.DisplayName.Trim()}%",
            Search = string.IsNullOrWhiteSpace(paging.Search) ? null : $"%{paging.Search.Trim()}%",
            PageSize = pageSize,
            Offset = offset
        };

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

        var rows = (await multi.ReadAsync<TitleOfCourtesySummaryRow>()).ToList();
        var totalCount = await multi.ReadSingleAsync<int>();

        return new PagedResult<TitleOfCourtesySummaryDto>(
            rows.Select(r => r.ToDto()).ToList(),
            page,
            pageSize,
            totalCount);
    }

    public async Task<IReadOnlyList<TitleOfCourtesyLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = ResolveTenantScope();
        var activeFilter = activeOnly ? "AND \"IsActive\" = TRUE" : string.Empty;
        var sql = $"""
            SELECT {TitleOfCourtesySql.SelectLookupColumns}
            FROM {TitleOfCourtesySql.Table}
            WHERE "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
              {activeFilter}
            ORDER BY "SortOrder" ASC NULLS LAST, "DisplayName" ASC
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<TitleOfCourtesyLookupRow>(
            new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return rows.Select(r => r.ToDto()).ToList();
    }

    public async Task<bool> ExistsByCodeAsync(
        string code,
        long? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = ResolveTenantScope();
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {TitleOfCourtesySql.Table}
                WHERE "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND "Code" = @Code
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
                    Code = code.Trim().ToUpperInvariant(),
                    ExcludeId = excludeId
                },
                cancellationToken: cancellationToken));
    }

    public async Task<bool> ExistsByDisplayNameAsync(
        string displayName,
        long? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = ResolveTenantScope();
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {TitleOfCourtesySql.Table}
                WHERE "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND "IsActive" = TRUE
                  AND LOWER("DisplayName") = LOWER(@DisplayName)
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
                    DisplayName = displayName.Trim(),
                    ExcludeId = excludeId
                },
                cancellationToken: cancellationToken));
    }

    private (long TenantId, long CompanyId) ResolveTenantScope()
    {
        if (_tenantContextAccessor.Current is { IsResolved: true } context)
        {
            return (context.TenantId, context.CompanyId);
        }

        throw new InvalidOperationException("Tenant context is not resolved.");
    }
}
