using Dapper;
using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Persistence;
using Fgs.Setup.Application.Abstractions.ResolutionCodes;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.ResolutionCodes.Dtos;

namespace Fgs.Setup.Infrastructure.Persistence.ResolutionCodes;

internal sealed class ResolutionCodeReadRepository : IResolutionCodeReadRepository
{
    private readonly ISetupReadConnectionFactory _connectionFactory;
    private readonly ITenantContextAccessor _tenantContextAccessor;

    public ResolutionCodeReadRepository(
        ISetupReadConnectionFactory connectionFactory,
        ITenantContextAccessor tenantContextAccessor)
    {
        _connectionFactory = connectionFactory;
        _tenantContextAccessor = tenantContextAccessor;
    }

    public async Task<ResolutionCodeDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT {ResolutionCodeSql.SelectDetailColumns}
            FROM {ResolutionCodeSql.Table}
            WHERE "Id" = @Id
              AND "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var row = await connection.QueryFirstOrDefaultAsync<ResolutionCodeDetailRow>(
            new CommandDefinition(sql, new { Id = id, TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return row?.ToDto();
    }

    public async Task<PagedResult<ResolutionCodeSummaryDto>> ListAsync(
        SetupListQuery query,
        ResolutionCodeListFilters filters,
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

        if (!string.IsNullOrWhiteSpace(filters.ResolutionCode))
        {
            where.Add("\"ResolutionCode\" = @ResolutionCode");
        }
        if (!string.IsNullOrWhiteSpace(filters.ResolutionName))
        {
            where.Add("\"ResolutionName\" ILIKE @ResolutionName");
        }

        if (!string.IsNullOrWhiteSpace(paging.Search))
        {
            where.Add(
                "(\"ResolutionCode\" ILIKE @Search OR \"ResolutionName\" ILIKE @Search)");
        }

        var whereClause = string.Join(" AND ", where);
        var orderBy = ResolutionCodeSql.ResolveOrderBy(paging.SortBy, paging.SortDirection);

        var sql = $"""
            SELECT {ResolutionCodeSql.SelectSummaryColumns}
            FROM {ResolutionCodeSql.Table}
            WHERE {whereClause}
            {orderBy}
            LIMIT @PageSize OFFSET @Offset;

            SELECT COUNT(*)
            FROM {ResolutionCodeSql.Table}
            WHERE {whereClause};
            """;

        var parameters = new
        {
            TenantId = tenantId,
            CompanyId = companyId,
            IsActive = paging.IsActive,
            ResolutionCode = filters.ResolutionCode?.Trim().ToUpperInvariant(),
            ResolutionName = string.IsNullOrWhiteSpace(filters.ResolutionName) ? null : $"%{filters.ResolutionName.Trim()}%",
            Search = string.IsNullOrWhiteSpace(paging.Search) ? null : $"%{paging.Search.Trim()}%",
            PageSize = pageSize,
            Offset = offset
        };

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

        var rows = (await multi.ReadAsync<ResolutionCodeSummaryRow>()).ToList();
        var totalCount = await multi.ReadSingleAsync<int>();

        return new PagedResult<ResolutionCodeSummaryDto>(
            rows.Select(r => r.ToDto()).ToList(),
            page,
            pageSize,
            totalCount);
    }

    public async Task<IReadOnlyList<ResolutionCodeLookupDto>> LookupAsync(
        bool activeOnly = true,
        bool? isMobileVisible = null,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var filters = new List<string>();

        if (activeOnly)
        {
            filters.Add("\"IsActive\" = TRUE");
        }

        if (isMobileVisible.HasValue)
        {
            filters.Add("\"IsMobileVisible\" = @IsMobileVisible");
        }

        var filterClause = filters.Count > 0 ? $"AND {string.Join(" AND ", filters)}" : string.Empty;
        var sql = $"""
            SELECT {ResolutionCodeSql.SelectLookupColumns}
            FROM {ResolutionCodeSql.Table}
            WHERE "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
              {filterClause}
            ORDER BY "ResolutionName" ASC
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<ResolutionCodeLookupRow>(
            new CommandDefinition(
                sql,
                new
                {
                    TenantId = tenantId,
                    CompanyId = companyId,
                    IsMobileVisible = isMobileVisible
                },
                cancellationToken: cancellationToken));

        return rows.Select(r => r.ToDto()).ToList();
    }

    public async Task<bool> ExistsByResolutionCodeAsync(
        string resolutionCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {ResolutionCodeSql.Table}
                WHERE "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND "ResolutionCode" = @ResolutionCode
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
                    ResolutionCode = resolutionCode.Trim().ToUpperInvariant(),
                    ExcludeId = excludeId
                },
                cancellationToken: cancellationToken));
    }
    public async Task<bool> ExistsGloResolutionTypeIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM setup."GloResolutionTypeCache"
                WHERE "ResolutionTypeId" = @Id AND "IsActive" = TRUE
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
