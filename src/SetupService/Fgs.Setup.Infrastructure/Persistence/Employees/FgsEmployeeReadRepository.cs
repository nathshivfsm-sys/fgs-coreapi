using Dapper;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Employees;
using Fgs.Setup.Application.Abstractions.Persistence;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.Employees.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.Employees;

internal sealed class FgsEmployeeReadRepository : IFgsEmployeeReadRepository
{
    private readonly ISetupReadConnectionFactory _connectionFactory;
    private readonly ITenantContextAccessor _tenantContextAccessor;

    public FgsEmployeeReadRepository(
        ISetupReadConnectionFactory connectionFactory,
        ITenantContextAccessor tenantContextAccessor)
    {
        _connectionFactory = connectionFactory;
        _tenantContextAccessor = tenantContextAccessor;
    }

    public async Task<FgsEmployeeDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT {FgsEmployeeSql.SelectDetailColumns}
            FROM {FgsEmployeeSql.Table} e
            {FgsEmployeeSql.LocationJoin}
            {FgsEmployeeSql.TechnicianProfileJoin}
            WHERE e."Id" = @Id
              AND e."TenantId" = @TenantId
              AND e."CompanyId" = @CompanyId
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var row = await connection.QueryFirstOrDefaultAsync<FgsEmployeeDetailRow>(
            new CommandDefinition(sql, new { Id = id, TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return row?.ToDto();
    }

    public async Task<FgsEmployeeListResultDto> ListAsync(
        SetupListQuery query,
        FgsEmployeeListFilters filters,
        bool includeSummary = true,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var paging = query.ToPagedQuery();
        var page = Math.Max(1, paging.Page);
        var pageSize = Math.Clamp(paging.PageSize, 1, 200);
        var offset = (page - 1) * pageSize;

        var techTradeIds = ToDistinctArray(filters.TechTradeIds);
        var techSkillIds = ToDistinctArray(filters.TechSkillIds);
        var dispatchZoneIds = ToDistinctArray(filters.DispatchZoneIds);
        var userIds = filters.UserIds is { Count: > 0 }
            ? filters.UserIds.Distinct().ToArray()
            : [];

        var where = new List<string>
        {
            "e.\"TenantId\" = @TenantId",
            "e.\"CompanyId\" = @CompanyId"
        };

        if (filters.StatusId.HasValue)
        {
            where.Add("e.\"StatusId\" = @StatusId");
        }
        else if (paging.IsActive == true)
        {
            where.Add("e.\"StatusId\" = @ActiveStatusId");
        }
        else if (paging.IsActive == false)
        {
            where.Add("e.\"StatusId\" <> @ActiveStatusId");
        }

        if (!string.IsNullOrWhiteSpace(filters.EmployeeNumber))
        {
            where.Add("e.\"EmployeeNumber\" ILIKE @EmployeeNumber");
        }

        if (filters.EmployeeTypeId.HasValue)
        {
            where.Add("e.\"EmployeeTypeId\" = @EmployeeTypeId");
        }

        if (techTradeIds.Length > 0)
        {
            where.Add("tp.\"TechTradeId\" = ANY(@TechTradeIds)");
        }

        if (techSkillIds.Length > 0)
        {
            where.Add("tp.\"TechSkillId\" = ANY(@TechSkillIds)");
        }

        if (dispatchZoneIds.Length > 0)
        {
            where.Add("tp.\"DispatchZoneId\" = ANY(@DispatchZoneIds)");
        }

        if (userIds.Length > 0)
        {
            where.Add("e.\"UserId\" = ANY(@UserIds)");
        }

        if (!string.IsNullOrWhiteSpace(paging.Search))
        {
            where.Add(
                """
                (e."EmployeeNumber" ILIKE @Search
                 OR e."DisplayName" ILIKE @Search
                 OR e."LegalFirstName" ILIKE @Search
                 OR e."LegalLastName" ILIKE @Search
                 OR e."OfficeEmail" ILIKE @Search
                 OR e."PersonalEmail" ILIKE @Search
                 OR e."PersonalPhone" ILIKE @Search
                 OR e."OfficePhone" ILIKE @Search
                 OR tp."CustomerFacingPhone" ILIKE @Search)
                """);
        }

        var needsTechnicianJoin =
            techTradeIds.Length > 0
            || techSkillIds.Length > 0
            || dispatchZoneIds.Length > 0
            || !string.IsNullOrWhiteSpace(paging.Search);

        var whereClause = string.Join(" AND ", where);
        var orderBy = FgsEmployeeSql.ResolveOrderBy(paging.SortBy, paging.SortDirection);
        var countTechnicianJoin = needsTechnicianJoin ? FgsEmployeeSql.TechnicianProfileJoin : string.Empty;

        var sql = $"""
            SELECT {FgsEmployeeSql.SelectSummaryColumns}
            FROM {FgsEmployeeSql.Table} e
            {FgsEmployeeSql.TechnicianProfileJoin}
            WHERE {whereClause}
            {orderBy}
            LIMIT @PageSize OFFSET @Offset;

            SELECT COUNT(*)
            FROM {FgsEmployeeSql.Table} e
            {countTechnicianJoin}
            WHERE {whereClause};
            {(includeSummary ? SummarySql : string.Empty)}
            """;

        var parameters = new
        {
            TenantId = tenantId,
            CompanyId = companyId,
            StatusId = filters.StatusId,
            ActiveStatusId = EmployeeStatusIds.Active,
            InactiveStatusId = EmployeeStatusIds.Inactive,
            EmployeeNumber = string.IsNullOrWhiteSpace(filters.EmployeeNumber)
                ? null
                : $"%{filters.EmployeeNumber.Trim()}%",
            EmployeeTypeId = filters.EmployeeTypeId,
            TechTradeIds = techTradeIds,
            TechSkillIds = techSkillIds,
            DispatchZoneIds = dispatchZoneIds,
            UserIds = userIds,
            Search = string.IsNullOrWhiteSpace(paging.Search) ? null : $"%{paging.Search.Trim()}%",
            PageSize = pageSize,
            Offset = offset
        };

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

        var rows = (await multi.ReadAsync<FgsEmployeeSummaryRow>()).ToList();
        var totalCount = await multi.ReadSingleAsync<int>();

        FgsEmployeeListSummaryDto summary;
        if (includeSummary)
        {
            var summaryRow = await multi.ReadSingleAsync<FgsEmployeeListSummaryRow>();
            summary = summaryRow.ToDto();
        }
        else
        {
            summary = EmptySummary;
        }

        return new FgsEmployeeListResultDto(
            rows.Select(r => r.ToDto()).ToList(),
            page,
            pageSize,
            totalCount,
            summary);
    }

    public async Task<FgsEmployeeListSummaryDto> GetListSummaryAsync(CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var row = await connection.QuerySingleAsync<FgsEmployeeListSummaryRow>(
            new CommandDefinition(
                SummarySql,
                new
                {
                    TenantId = tenantId,
                    CompanyId = companyId,
                    ActiveStatusId = EmployeeStatusIds.Active,
                    InactiveStatusId = EmployeeStatusIds.Inactive
                },
                cancellationToken: cancellationToken));

        return row.ToDto();
    }

    private static readonly FgsEmployeeListSummaryDto EmptySummary = new(0, 0, 0);

    // Company-scoped card counts — ignore list filters (search/status/tech/role).
    private const string SummarySql = """
        SELECT
            COUNT(*)::int AS "TotalEmployees",
            COUNT(*) FILTER (WHERE e."StatusId" = @ActiveStatusId)::int AS "ActiveEmployees",
            COUNT(*) FILTER (WHERE e."StatusId" = @InactiveStatusId)::int AS "InactiveEmployees"
        FROM setup."FgsEmployee" e
        WHERE e."TenantId" = @TenantId
          AND e."CompanyId" = @CompanyId;
        """;

    private static long[] ToDistinctArray(IReadOnlyList<long>? values) =>
        values is { Count: > 0 } ? values.Distinct().ToArray() : [];

    public async Task<IReadOnlyList<FgsEmployeeLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var activeFilter = activeOnly ? $"AND \"StatusId\" = {EmployeeStatusIds.Active}" : string.Empty;
        var sql = $"""
            SELECT {FgsEmployeeSql.SelectLookupColumns}
            FROM {FgsEmployeeSql.Table}
            WHERE "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
              {activeFilter}
            ORDER BY "DisplayName" ASC
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<FgsEmployeeLookupRow>(
            new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return rows.Select(r => r.ToDto()).ToList();
    }

    public async Task<bool> ExistsByEmployeeNumberAsync(
        string employeeNumber,
        long? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var excludeClause = excludeId.HasValue ? "AND \"Id\" <> @ExcludeId" : string.Empty;
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {FgsEmployeeSql.Table}
                WHERE "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND LOWER("EmployeeNumber") = LOWER(@EmployeeNumber)
                  {excludeClause})
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                sql,
                new
                {
                    TenantId = tenantId,
                    CompanyId = companyId,
                    EmployeeNumber = employeeNumber.Trim(),
                    ExcludeId = excludeId
                },
                cancellationToken: cancellationToken));
    }

    public async Task<bool> ExistsByOfficeEmailAsync(
        string officeEmail,
        long? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(officeEmail))
        {
            return false;
        }

        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var excludeClause = excludeId.HasValue ? "AND \"Id\" <> @ExcludeId" : string.Empty;
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {FgsEmployeeSql.Table}
                WHERE "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND "OfficeEmail" IS NOT NULL
                  AND LOWER("OfficeEmail") = LOWER(@OfficeEmail)
                  {excludeClause})
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                sql,
                new
                {
                    TenantId = tenantId,
                    CompanyId = companyId,
                    OfficeEmail = officeEmail.Trim(),
                    ExcludeId = excludeId
                },
                cancellationToken: cancellationToken));
    }

    public async Task<bool> ExistsByUserIdAsync(
        Guid userId,
        long? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var excludeClause = excludeId.HasValue ? "AND \"Id\" <> @ExcludeId" : string.Empty;
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {FgsEmployeeSql.Table}
                WHERE "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND "UserId" = @UserId
                  {excludeClause})
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                sql,
                new
                {
                    TenantId = tenantId,
                    CompanyId = companyId,
                    UserId = userId,
                    ExcludeId = excludeId
                },
                cancellationToken: cancellationToken));
    }

    public async Task<bool> ExistsByTechCodeAsync(
        string techCode,
        long? excludeEmployeeId = null,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var excludeClause = excludeEmployeeId.HasValue ? "AND \"EmployeeId\" <> @ExcludeEmployeeId" : string.Empty;
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {FgsEmployeeSql.TechnicianProfileTable}
                WHERE "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND LOWER("TechCode") = LOWER(@TechCode)
                  {excludeClause})
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                sql,
                new
                {
                    TenantId = tenantId,
                    CompanyId = companyId,
                    TechCode = techCode.Trim(),
                    ExcludeEmployeeId = excludeEmployeeId
                },
                cancellationToken: cancellationToken));
    }

    public async Task<bool> ExistsTechnicianProfileByEmployeeIdAsync(
        long employeeId,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {FgsEmployeeSql.TechnicianProfileTable}
                WHERE "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND "EmployeeId" = @EmployeeId)
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                sql,
                new
                {
                    TenantId = tenantId,
                    CompanyId = companyId,
                    EmployeeId = employeeId
                },
                cancellationToken: cancellationToken));
    }
}
