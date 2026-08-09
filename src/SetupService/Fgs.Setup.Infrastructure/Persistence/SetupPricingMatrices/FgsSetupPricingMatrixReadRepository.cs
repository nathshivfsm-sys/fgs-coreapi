using Dapper;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Persistence;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrices;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrices.Dtos;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.SetupPricingMatrices;

internal sealed class FgsSetupPricingMatrixReadRepository : IFgsSetupPricingMatrixReadRepository
{
    private readonly ISetupReadConnectionFactory _connectionFactory;
    private readonly ITenantContextAccessor _tenantContextAccessor;

    public FgsSetupPricingMatrixReadRepository(
        ISetupReadConnectionFactory connectionFactory,
        ITenantContextAccessor tenantContextAccessor)
    {
        _connectionFactory = connectionFactory;
        _tenantContextAccessor = tenantContextAccessor;
    }

    public async Task<FgsSetupPricingMatrixDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT {FgsSetupPricingMatrixSql.SelectHeaderColumns}
            FROM {FgsSetupPricingMatrixSql.Table} pm
            WHERE pm."Id" = @Id
              AND pm."TenantId" = @TenantId
              AND pm."CompanyId" = @CompanyId
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var header = await connection.QueryFirstOrDefaultAsync<FgsSetupPricingMatrixHeaderRow>(
            new CommandDefinition(sql, new { Id = id, TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return header?.ToDetailDto();
    }

    public async Task<FgsSetupPricingMatrixFlagsDto?> GetFlagsByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT {FgsSetupPricingMatrixSql.SelectFlagsColumns}
            FROM {FgsSetupPricingMatrixSql.Table} pm
            WHERE pm."Id" = @Id
              AND pm."TenantId" = @TenantId
              AND pm."CompanyId" = @CompanyId
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var header = await connection.QueryFirstOrDefaultAsync<FgsSetupPricingMatrixHeaderRow>(
            new CommandDefinition(sql, new { Id = id, TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return header?.ToFlagsDto();
    }

    public async Task<PagedResult<FgsSetupPricingMatrixSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsSetupPricingMatrixListFilters filters,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var paging = query.ToPagedQuery();
        var page = Math.Max(1, paging.Page);
        var pageSize = Math.Clamp(paging.PageSize, 1, 200);
        var offset = (page - 1) * pageSize;

        var where = new List<string>
        {
            "pm.\"TenantId\" = @TenantId",
            "pm.\"CompanyId\" = @CompanyId"
        };

        if (paging.IsActive.HasValue)
        {
            where.Add("pm.\"IsActive\" = @IsActive");
        }

        if (!string.IsNullOrWhiteSpace(filters.Code))
        {
            where.Add("pm.\"Code\" = @Code");
        }

        if (filters.IsDefault.HasValue)
        {
            where.Add("pm.\"IsDefault\" = @FilterIsDefault");
        }

        if (!string.IsNullOrWhiteSpace(paging.Search))
        {
            where.Add("(pm.\"Code\" ILIKE @Search OR pm.\"Name\" ILIKE @Search)");
        }

        var whereClause = string.Join(" AND ", where);
        var orderBy = FgsSetupPricingMatrixSql.ResolveOrderBy(paging.SortBy, paging.SortDirection);

        var sql = $"""
            SELECT {FgsSetupPricingMatrixSql.SelectSummaryColumns}
            FROM {FgsSetupPricingMatrixSql.Table} pm
            WHERE {whereClause}
            {orderBy}
            LIMIT @PageSize OFFSET @Offset;

            SELECT COUNT(*)
            FROM {FgsSetupPricingMatrixSql.Table} pm
            WHERE {whereClause};
            """;

        var parameters = new
        {
            TenantId = tenantId,
            CompanyId = companyId,
            IsActive = paging.IsActive,
            Code = filters.Code?.Trim().ToUpperInvariant(),
            FilterIsDefault = filters.IsDefault,
            Search = string.IsNullOrWhiteSpace(paging.Search) ? null : $"%{paging.Search.Trim()}%",
            PageSize = pageSize,
            Offset = offset
        };

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

        var rows = (await multi.ReadAsync<FgsSetupPricingMatrixHeaderRow>()).ToList();
        var totalCount = await multi.ReadSingleAsync<int>();

        return new PagedResult<FgsSetupPricingMatrixSummaryDto>(
            rows.Select(r => r.ToSummaryDto()).ToList(),
            totalCount,
            page,
            pageSize);
    }

    public async Task<IReadOnlyList<FgsSetupPricingMatrixLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var activeFilter = activeOnly ? "AND pm.\"IsActive\" = TRUE" : string.Empty;

        var sql = $"""
            SELECT {FgsSetupPricingMatrixSql.SelectLookupColumns}
            FROM {FgsSetupPricingMatrixSql.Table} pm
            WHERE pm."TenantId" = @TenantId
              AND pm."CompanyId" = @CompanyId
              {activeFilter}
            ORDER BY pm."Code"
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<FgsSetupPricingMatrixHeaderRow>(
            new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return rows.Select(r => r.ToLookupDto()).ToList();
    }

    public async Task<bool> ExistsByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {FgsSetupPricingMatrixSql.Table} pm
                WHERE pm."TenantId" = @TenantId
                  AND pm."CompanyId" = @CompanyId
                  AND pm."Id" = @Id
                  AND pm."IsActive" = TRUE
            )
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                sql,
                new { TenantId = tenantId, CompanyId = companyId, Id = id },
                cancellationToken: cancellationToken));
    }

    public async Task<bool> ExistsByCodeAsync(
        string code,
        long? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {FgsSetupPricingMatrixSql.Table} pm
                WHERE pm."TenantId" = @TenantId
                  AND pm."CompanyId" = @CompanyId
                  AND pm."Code" = @Code
                  AND (@ExcludeId IS NULL OR pm."Id" <> @ExcludeId)
            )
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                sql,
                new { TenantId = tenantId, CompanyId = companyId, Code = code.Trim().ToUpperInvariant(), ExcludeId = excludeId },
                cancellationToken: cancellationToken));
    }
}
