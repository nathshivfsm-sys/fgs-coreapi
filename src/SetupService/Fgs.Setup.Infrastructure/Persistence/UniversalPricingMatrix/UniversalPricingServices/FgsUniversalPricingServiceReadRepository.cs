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
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);

        var headerSql = $"""
            SELECT {FgsUniversalPricingServiceSql.SelectDetailColumns}
            FROM {FgsUniversalPricingServiceSql.Table}
            WHERE "Id" = @Id
              AND "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
            """;

        var header = await connection.QueryFirstOrDefaultAsync<FgsUniversalPricingServiceHeaderRow>(
            new CommandDefinition(headerSql, new { Id = id, TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        if (header is null)
        {
            return null;
        }

        var parameters = new { UniversalPricingServiceId = id, TenantId = tenantId, CompanyId = companyId };

        var tiers = (await connection.QueryAsync<FgsUniversalMatrixTierRow>(
            new CommandDefinition(
                $"""
                SELECT {FgsUniversalPricingServiceSql.SelectTierColumns}
                FROM {FgsUniversalPricingServiceSql.TierTable}
                WHERE "UniversalPricingServiceId" = @UniversalPricingServiceId
                  AND "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND "IsActive" = TRUE
                ORDER BY "DisplayOrder" ASC, "Name" ASC
                """,
                parameters,
                cancellationToken: cancellationToken))).Select(r => r.ToDto()).ToList();

        var sizeTiers = (await connection.QueryAsync<FgsUniversalMatrixSizeTierRow>(
            new CommandDefinition(
                $"""
                SELECT {FgsUniversalPricingServiceSql.SelectSizeTierColumns}
                FROM {FgsUniversalPricingServiceSql.SizeTierTable}
                WHERE "UniversalPricingServiceId" = @UniversalPricingServiceId
                  AND "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND "IsActive" = TRUE
                ORDER BY "DisplayOrder" ASC, "Name" ASC
                """,
                parameters,
                cancellationToken: cancellationToken))).Select(r => r.ToDto()).ToList();

        var items = (await connection.QueryAsync<FgsUniversalMatrixItemRow>(
            new CommandDefinition(
                $"""
                SELECT {FgsUniversalPricingServiceSql.SelectItemColumns}
                FROM {FgsUniversalPricingServiceSql.ItemTable}
                WHERE "UniversalPricingServiceId" = @UniversalPricingServiceId
                  AND "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND "IsActive" = TRUE
                ORDER BY "DisplayOrder" ASC, "ItemName" ASC
                """,
                parameters,
                cancellationToken: cancellationToken))).Select(r => r.ToDto()).ToList();

        var frequencyDiscounts = (await connection.QueryAsync<FgsUniversalMatrixFrequencyDiscountRow>(
            new CommandDefinition(
                $"""
                SELECT {FgsUniversalPricingServiceSql.SelectFrequencyDiscountColumns}
                FROM {FgsUniversalPricingServiceSql.FrequencyDiscountTable}
                WHERE "UniversalPricingServiceId" = @UniversalPricingServiceId
                  AND "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND "IsActive" = TRUE
                ORDER BY "DisplayOrder" ASC, "Name" ASC
                """,
                parameters,
                cancellationToken: cancellationToken))).Select(r => r.ToDto()).ToList();

        var oneTimeFees = (await connection.QueryAsync<FgsUniversalMatrixOneTimeFeeRow>(
            new CommandDefinition(
                $"""
                SELECT {FgsUniversalPricingServiceSql.SelectOneTimeFeeColumns}
                FROM {FgsUniversalPricingServiceSql.OneTimeFeeTable}
                WHERE "UniversalPricingServiceId" = @UniversalPricingServiceId
                  AND "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND "IsActive" = TRUE
                ORDER BY "DisplayOrder" ASC, "Name" ASC
                """,
                parameters,
                cancellationToken: cancellationToken))).Select(r => r.ToDto()).ToList();

        var addOns = (await connection.QueryAsync<FgsUniversalMatrixAddOnRow>(
            new CommandDefinition(
                $"""
                SELECT {FgsUniversalPricingServiceSql.SelectAddOnColumns}
                FROM {FgsUniversalPricingServiceSql.AddOnTable}
                WHERE "UniversalPricingServiceId" = @UniversalPricingServiceId
                  AND "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND "IsActive" = TRUE
                ORDER BY "DisplayOrder" ASC, "Name" ASC
                """,
                parameters,
                cancellationToken: cancellationToken))).Select(r => r.ToDto()).ToList();

        return new FgsUniversalPricingServiceDetailDto(
            header.Id,
            header.UniversalPricingServiceCode,
            header.DisplayOrder,
            header.IsActive,
            tiers,
            sizeTiers,
            items,
            frequencyDiscounts,
            oneTimeFees,
            addOns);
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

    public async Task<bool> ExistsGloUniversalPricingServiceCodeAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        var sql = """
            SELECT EXISTS(
                SELECT 1
                FROM glo."GloUniversalPricingService"
                WHERE "ServiceCode" = UPPER(@Code)
            )
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                sql,
                new { Code = code.Trim().ToUpperInvariant() },
                cancellationToken: cancellationToken));
    }
}
