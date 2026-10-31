using Dapper;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Persistence;
using Fgs.Setup.Application.Abstractions.SetupPriceBookVersions;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPriceBookVersions.Dtos;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Entities.SetupPriceBookVersions;

internal sealed class FgsSetupPriceBookVersionReadRepository(
    ISetupReadConnectionFactory connectionFactory,
    ITenantContextAccessor tenantContextAccessor) : IFgsSetupPriceBookVersionReadRepository
{
    public async Task<FgsSetupPriceBookVersionDetailDto?> GetByIdAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var sql = $"""
            SELECT {FgsSetupPriceBookVersionSql.SelectDetailColumns}
            FROM {FgsSetupPriceBookVersionSql.Table}
            WHERE "Id" = @Id
              AND "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var row = await connection.QueryFirstOrDefaultAsync<FgsSetupPriceBookVersionDetailRow>(
            new CommandDefinition(sql, new { Id = id, TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return row?.ToDto();
    }

    public async Task<PagedResult<FgsSetupPriceBookVersionSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsSetupPriceBookVersionListFilters filters,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(tenantContextAccessor);
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

        if (!string.IsNullOrWhiteSpace(filters.Version))
        {
            where.Add("\"Version\" ILIKE @Version");
        }

        if (!string.IsNullOrWhiteSpace(paging.Search))
        {
            where.Add("\"Version\" ILIKE @Search");
        }

        var whereClause = string.Join(" AND ", where);
        var orderBy = FgsSetupPriceBookVersionSql.ResolveOrderBy(paging.SortBy, paging.SortDirection);

        var sql = $"""
            SELECT {FgsSetupPriceBookVersionSql.SelectSummaryColumns}
            FROM {FgsSetupPriceBookVersionSql.Table}
            WHERE {whereClause}
            {orderBy}
            LIMIT @PageSize OFFSET @Offset;

            SELECT COUNT(*)
            FROM {FgsSetupPriceBookVersionSql.Table}
            WHERE {whereClause};
            """;

        var parameters = new
        {
            TenantId = tenantId,
            CompanyId = companyId,
            IsActive = paging.IsActive,
            Version = string.IsNullOrWhiteSpace(filters.Version) ? null : $"%{filters.Version.Trim()}%",
            Search = string.IsNullOrWhiteSpace(paging.Search) ? null : $"%{paging.Search.Trim()}%",
            PageSize = pageSize,
            Offset = offset
        };

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

        var rows = (await multi.ReadAsync<FgsSetupPriceBookVersionSummaryRow>()).ToList();
        var totalCount = await multi.ReadSingleAsync<int>();

        return new PagedResult<FgsSetupPriceBookVersionSummaryDto>(
            rows.Select(r => r.ToDto()).ToList(),
            page,
            pageSize,
            totalCount);
    }

    public async Task<IReadOnlyList<FgsSetupPriceBookVersionLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var activeFilter = activeOnly ? "AND \"IsActive\" = TRUE" : string.Empty;
        var sql = $"""
            SELECT {FgsSetupPriceBookVersionSql.SelectLookupColumns}
            FROM {FgsSetupPriceBookVersionSql.Table}
            WHERE "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
              {activeFilter}
            ORDER BY "EffectiveFrom" DESC, "Version" ASC
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<FgsSetupPriceBookVersionLookupRow>(
            new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return rows.Select(r => r.ToDto()).ToList();
    }

    public async Task<bool> ExistsByVersionAsync(
        string version,
        long? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {FgsSetupPriceBookVersionSql.Table}
                WHERE "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND "Version" = @Version
                  {(excludeId.HasValue ? "AND \"Id\" <> @ExcludeId" : string.Empty)}
            )
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                sql,
                new
                {
                    TenantId = tenantId,
                    CompanyId = companyId,
                    Version = version.Trim(),
                    ExcludeId = excludeId
                },
                cancellationToken: cancellationToken));
    }
}
