using Dapper;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.EntityDefaultTermsConditions;
using Fgs.Setup.Application.Abstractions.Persistence;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Dtos;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.EntityDefaultTermsConditions;

internal sealed class FgsEntityDefaultTermsConditionReadRepository : IFgsEntityDefaultTermsConditionReadRepository
{
    private readonly ISetupReadConnectionFactory _connectionFactory;
    private readonly ITenantContextAccessor _tenantContextAccessor;

    public FgsEntityDefaultTermsConditionReadRepository(
        ISetupReadConnectionFactory connectionFactory,
        ITenantContextAccessor tenantContextAccessor)
    {
        _connectionFactory = connectionFactory;
        _tenantContextAccessor = tenantContextAccessor;
    }

    public async Task<FgsEntityDefaultTermsConditionDetailDto?> GetByIdAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT {FgsEntityDefaultTermsConditionSql.SelectDetailColumns}
            FROM {FgsEntityDefaultTermsConditionSql.Table} d
            LEFT JOIN {FgsEntityDefaultTermsConditionSql.TermsConditionTable} t
              ON t."Id" = d."TermsConditionId"
            WHERE d."Id" = @Id
              AND d."TenantId" = @TenantId
              AND d."CompanyId" = @CompanyId
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var row = await connection.QueryFirstOrDefaultAsync<FgsEntityDefaultTermsConditionDetailRow>(
            new CommandDefinition(sql, new { Id = id, TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return row?.ToDto();
    }

    public async Task<PagedResult<FgsEntityDefaultTermsConditionSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsEntityDefaultTermsConditionListFilters filters,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var paging = query.ToPagedQuery();
        var page = Math.Max(1, paging.Page);
        var pageSize = Math.Clamp(paging.PageSize, 1, 200);
        var offset = (page - 1) * pageSize;

        var where = new List<string>
        {
            "d.\"TenantId\" = @TenantId",
            "d.\"CompanyId\" = @CompanyId"
        };

        if (paging.IsActive.HasValue)
        {
            where.Add("d.\"IsActive\" = @IsActive");
        }

        if (!string.IsNullOrWhiteSpace(filters.EntityType))
        {
            where.Add("d.\"EntityType\" ILIKE @EntityType");
        }

        if (filters.TermsConditionId.HasValue)
        {
            where.Add("d.\"TermsConditionId\" = @TermsConditionId");
        }

        if (!string.IsNullOrWhiteSpace(paging.Search))
        {
            where.Add(
                "(d.\"EntityType\" ILIKE @Search OR t.\"Code\" ILIKE @Search OR t.\"Name\" ILIKE @Search)");
        }

        var whereClause = string.Join(" AND ", where);
        var orderBy = FgsEntityDefaultTermsConditionSql.ResolveOrderBy(paging.SortBy, paging.SortDirection);

        var sql = $"""
            SELECT {FgsEntityDefaultTermsConditionSql.SelectSummaryColumns}
            FROM {FgsEntityDefaultTermsConditionSql.Table} d
            LEFT JOIN {FgsEntityDefaultTermsConditionSql.TermsConditionTable} t
              ON t."Id" = d."TermsConditionId"
            WHERE {whereClause}
            {orderBy}
            LIMIT @PageSize OFFSET @Offset;

            SELECT COUNT(*)
            FROM {FgsEntityDefaultTermsConditionSql.Table} d
            LEFT JOIN {FgsEntityDefaultTermsConditionSql.TermsConditionTable} t
              ON t."Id" = d."TermsConditionId"
            WHERE {whereClause};
            """;

        var parameters = new
        {
            TenantId = tenantId,
            CompanyId = companyId,
            IsActive = paging.IsActive,
            EntityType = string.IsNullOrWhiteSpace(filters.EntityType) ? null : $"%{filters.EntityType.Trim()}%",
            TermsConditionId = filters.TermsConditionId,
            Search = string.IsNullOrWhiteSpace(paging.Search) ? null : $"%{paging.Search.Trim()}%",
            PageSize = pageSize,
            Offset = offset
        };

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

        var rows = (await multi.ReadAsync<FgsEntityDefaultTermsConditionSummaryRow>()).ToList();
        var totalCount = await multi.ReadSingleAsync<int>();

        return new PagedResult<FgsEntityDefaultTermsConditionSummaryDto>(
            rows.Select(r => r.ToDto()).ToList(),
            page,
            pageSize,
            totalCount);
    }

    public async Task<IReadOnlyList<FgsEntityDefaultTermsConditionLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var activeFilter = activeOnly ? "AND d.\"IsActive\" = TRUE" : string.Empty;
        var sql = $"""
            SELECT {FgsEntityDefaultTermsConditionSql.SelectLookupColumns}
            FROM {FgsEntityDefaultTermsConditionSql.Table} d
            WHERE d."TenantId" = @TenantId
              AND d."CompanyId" = @CompanyId
              {activeFilter}
            ORDER BY d."EntityType" ASC
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<FgsEntityDefaultTermsConditionLookupRow>(
            new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return rows.Select(r => r.ToDto()).ToList();
    }

    public async Task<bool> ExistsByEntityTypeAsync(
        string entityType,
        long? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {FgsEntityDefaultTermsConditionSql.Table}
                WHERE "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND "IsActive" = TRUE
                  AND LOWER("EntityType") = LOWER(@EntityType)
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
                    EntityType = entityType.Trim(),
                    ExcludeId = excludeId
                },
                cancellationToken: cancellationToken));
    }
}
