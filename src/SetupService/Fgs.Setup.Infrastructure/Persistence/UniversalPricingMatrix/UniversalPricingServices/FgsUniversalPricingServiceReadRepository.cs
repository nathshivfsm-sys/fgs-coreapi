using Dapper;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Persistence;
using Fgs.Setup.Application.Abstractions.UniversalPricingServices;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.UniversalPricingServices.Dtos;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.UniversalPricingMatrix.UniversalPricingServices;

internal sealed class FgsUniversalPricingServiceReadRepository : IFgsUniversalPricingServiceReadRepository
{
    private readonly ISetupReadConnectionFactory _connectionFactory;
    private readonly ITenantContextAccessor _tenantContextAccessor;

    public FgsUniversalPricingServiceReadRepository(
        ISetupReadConnectionFactory connectionFactory,
        ITenantContextAccessor tenantContextAccessor)
    {
        _connectionFactory = connectionFactory;
        _tenantContextAccessor = tenantContextAccessor;
    }

    public async Task<FgsUniversalPricingServiceDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT {FgsUniversalPricingServiceSql.SelectDetailColumns}
            FROM {FgsUniversalPricingServiceSql.Table}
            WHERE "Id" = @Id
              AND "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var row = await connection.QueryFirstOrDefaultAsync<FgsUniversalPricingServiceDetailRow>(
            new CommandDefinition(sql, new { Id = id, TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return row?.ToDto();
    }

    public async Task<PagedResult<FgsUniversalPricingServiceSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsUniversalPricingServiceListFilters filters,
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

        if (!string.IsNullOrWhiteSpace(filters.UniversalPricingServiceCode))
        {
            where.Add("\"UniversalPricingServiceCode\" = @UniversalPricingServiceCode");
        }

        if (!string.IsNullOrWhiteSpace(paging.Search))
        {
            where.Add("(\"UniversalPricingServiceCode\" ILIKE @Search)");
        }

        var whereClause = string.Join(" AND ", where);
        var orderBy = FgsUniversalPricingServiceSql.ResolveOrderBy(paging.SortBy, paging.SortDirection);

        var sql = $"""
            SELECT {FgsUniversalPricingServiceSql.SelectSummaryColumns}
            FROM {FgsUniversalPricingServiceSql.Table}
            WHERE {whereClause}
            {orderBy}
            LIMIT @PageSize OFFSET @Offset;

            SELECT COUNT(*)
            FROM {FgsUniversalPricingServiceSql.Table}
            WHERE {whereClause};
            """;

        var parameters = new
        {
            TenantId = tenantId,
            CompanyId = companyId,
            IsActive = paging.IsActive,
            UniversalPricingServiceCode = filters.UniversalPricingServiceCode?.Trim().ToUpperInvariant(),
            Search = string.IsNullOrWhiteSpace(paging.Search) ? null : $"%{paging.Search.Trim()}%",
            PageSize = pageSize,
            Offset = offset
        };

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

        var rows = (await multi.ReadAsync<FgsUniversalPricingServiceSummaryRow>()).ToList();
        var totalCount = await multi.ReadSingleAsync<int>();

        return new PagedResult<FgsUniversalPricingServiceSummaryDto>(
            rows.Select(r => r.ToDto()).ToList(),
            page,
            pageSize,
            totalCount);
    }

    public async Task<IReadOnlyList<FgsUniversalPricingServiceLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var activeFilter = activeOnly ? "AND \"IsActive\" = TRUE" : string.Empty;
        var sql = $"""
            SELECT {FgsUniversalPricingServiceSql.SelectLookupColumns}
            FROM {FgsUniversalPricingServiceSql.Table}
            WHERE "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
              {activeFilter}
            ORDER BY "DisplayOrder" ASC NULLS LAST, "UniversalPricingServiceCode" ASC
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<FgsUniversalPricingServiceLookupRow>(
            new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return rows.Select(r => r.ToDto()).ToList();
    }

    public async Task<bool> ExistsByUniversalPricingServiceCodeAsync(
        string universalPricingServiceCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {FgsUniversalPricingServiceSql.Table}
                WHERE "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND "UniversalPricingServiceCode" = @UniversalPricingServiceCode
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
                    UniversalPricingServiceCode = universalPricingServiceCode.Trim().ToUpperInvariant(),
                    ExcludeId = excludeId
                },
                cancellationToken: cancellationToken));
    }
}
