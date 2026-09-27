using Dapper;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Persistence;
using Fgs.Setup.Application.Abstractions.JobTypes;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.JobTypes.Dtos;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Persistence.JobTypeCategories;

namespace Fgs.Setup.Infrastructure.Persistence.JobTypes;

internal sealed class JobTypeReadRepository : IJobTypeReadRepository
{
    private readonly ISetupReadConnectionFactory _connectionFactory;
    private readonly ITenantContextAccessor _tenantContextAccessor;

    public JobTypeReadRepository(
        ISetupReadConnectionFactory connectionFactory,
        ITenantContextAccessor tenantContextAccessor)
    {
        _connectionFactory = connectionFactory;
        _tenantContextAccessor = tenantContextAccessor;
    }

    public async Task<JobTypeDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT {JobTypeSql.SelectDetailColumns}
            FROM {JobTypeSql.Table}
            WHERE "Id" = @Id
              AND "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var row = await connection.QueryFirstOrDefaultAsync<JobTypeDetailRow>(
            new CommandDefinition(sql, new { Id = id, TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        if (row is null)
        {
            return null;
        }

        var childrenSql = $"""
            SELECT {JobTypeCategorySql.SelectJobTypeChildColumns}
            FROM {JobTypeCategorySql.FromJoins}
            WHERE jtc."JobTypeId" = @Id
              AND jtc."TenantId" = @TenantId
              AND jtc."CompanyId" = @CompanyId
            ORDER BY jtc."DisplayOrder" ASC NULLS LAST, jtc."Id" ASC
            """;
        var children = await connection.QueryAsync<JobTypeSubCategoryRow>(
            new CommandDefinition(
                childrenSql,
                new { Id = id, TenantId = tenantId, CompanyId = companyId },
                cancellationToken: cancellationToken));

        return row.ToDto(children.Select(c => c.ToDto()).ToList());
    }

    public async Task<PagedResult<JobTypeSummaryDto>> ListAsync(
        SetupListQuery query,
        JobTypeListFilters filters,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var paging = query.ToPagedQuery();
        var page = Math.Max(1, paging.Page);
        var pageSize = Math.Clamp(paging.PageSize, 1, 200);
        var offset = (page - 1) * pageSize;

        var where = new List<string>
        {
            "jt.\"TenantId\" = @TenantId",
            "jt.\"CompanyId\" = @CompanyId"
        };

        if (paging.IsActive.HasValue)
        {
            where.Add("jt.\"IsActive\" = @IsActive");
        }

        AppendSharedListFilters(where, paging.Search, filters);

        var whereClause = string.Join(" AND ", where);
        var orderBy = JobTypeSql.ResolveOrderBy(paging.SortBy, paging.SortDirection);

        var sql = $"""
            SELECT {JobTypeSql.SelectSummaryColumns}
            FROM {JobTypeSql.Table} jt
            WHERE {whereClause}
            {orderBy}
            LIMIT @PageSize OFFSET @Offset;

            SELECT COUNT(*)
            FROM {JobTypeSql.Table} jt
            WHERE {whereClause};
            """;

        var parameters = new
        {
            TenantId = tenantId,
            CompanyId = companyId,
            IsActive = paging.IsActive,
            JobTypeCode = filters.JobTypeCode?.Trim().ToUpperInvariant(),
            Name = string.IsNullOrWhiteSpace(filters.Name) ? null : $"%{filters.Name.Trim()}%",
            UsedFor = filters.UsedFor,
            JobTypeTaskId = filters.JobTypeTaskId,
            BusinessUnit = string.IsNullOrWhiteSpace(filters.BusinessUnit) ? null : $"%{filters.BusinessUnit.Trim()}%",
            Search = string.IsNullOrWhiteSpace(paging.Search) ? null : $"%{paging.Search.Trim()}%",
            PageSize = pageSize,
            Offset = offset
        };

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

        var rows = (await multi.ReadAsync<JobTypeSummaryRow>()).ToList();
        var totalCount = await multi.ReadSingleAsync<int>();

        return new PagedResult<JobTypeSummaryDto>(
            rows.Select(r => r.ToDto()).ToList(),
            page,
            pageSize,
            totalCount);
    }

    public async Task<JobTypeCountsDto> GetCountsAsync(
        string? search,
        JobTypeListFilters filters,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);

        var where = new List<string>
        {
            "jt.\"TenantId\" = @TenantId",
            "jt.\"CompanyId\" = @CompanyId"
        };

        AppendSharedListFilters(where, search, filters);

        var whereClause = string.Join(" AND ", where);
        var sql = $"""
            SELECT
                COUNT(*) FILTER (WHERE jt."IsActive" = TRUE) AS "ActiveCount",
                COUNT(*) FILTER (WHERE jt."IsActive" = FALSE) AS "InactiveCount"
            FROM {JobTypeSql.Table} jt
            WHERE {whereClause};
            """;

        var parameters = new
        {
            TenantId = tenantId,
            CompanyId = companyId,
            JobTypeCode = filters.JobTypeCode?.Trim().ToUpperInvariant(),
            Name = string.IsNullOrWhiteSpace(filters.Name) ? null : $"%{filters.Name.Trim()}%",
            UsedFor = filters.UsedFor,
            JobTypeTaskId = filters.JobTypeTaskId,
            BusinessUnit = string.IsNullOrWhiteSpace(filters.BusinessUnit) ? null : $"%{filters.BusinessUnit.Trim()}%",
            Search = string.IsNullOrWhiteSpace(search) ? null : $"%{search.Trim()}%"
        };

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var row = await connection.QueryFirstAsync<JobTypeCountsRow>(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

        return row.ToDto();
    }

    public async Task<IReadOnlyList<JobTypeLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var activeFilter = activeOnly ? "AND \"IsActive\" = TRUE" : string.Empty;
        var sql = $"""
            SELECT {JobTypeSql.SelectLookupColumns}
            FROM {JobTypeSql.Table}
            WHERE "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
              {activeFilter}
            ORDER BY "DisplayOrder" ASC NULLS LAST, "Name" ASC
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<JobTypeLookupRow>(
            new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return rows.Select(r => r.ToDto()).ToList();
    }

    public async Task<bool> ExistsByJobTypeCodeAsync(
        string jobTypeCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {JobTypeSql.Table}
                WHERE "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND "IsActive" = TRUE
                  AND "JobTypeCode" = @JobTypeCode
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
                    JobTypeCode = jobTypeCode.Trim().ToUpperInvariant(),
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
                FROM {JobTypeSql.Table}
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

    private static void AppendSharedListFilters(
        List<string> where,
        string? search,
        JobTypeListFilters filters)
    {
        if (!string.IsNullOrWhiteSpace(filters.JobTypeCode))
        {
            where.Add("jt.\"JobTypeCode\" = @JobTypeCode");
        }

        if (!string.IsNullOrWhiteSpace(filters.Name))
        {
            where.Add("jt.\"Name\" ILIKE @Name");
        }

        if (filters.UsedFor.HasValue)
        {
            where.Add("jt.\"UsedFor\" = @UsedFor");
        }

        if (filters.JobTypeTaskId.HasValue)
        {
            where.Add(JobTypeSql.RelatedJobTypeTaskIdExists);
        }

        if (!string.IsNullOrWhiteSpace(filters.BusinessUnit))
        {
            where.Add("jt.\"BusinessUnit\" ILIKE @BusinessUnit");
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            where.Add(
                $"(jt.\"JobTypeCode\" ILIKE @Search OR jt.\"Name\" ILIKE @Search OR {JobTypeSql.RelatedTaskNameExists})");
        }
    }
}
