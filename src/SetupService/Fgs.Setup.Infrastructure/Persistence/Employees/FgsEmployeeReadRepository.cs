using Dapper;
using Fgs.Foundation.Paging;
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
            WHERE e."Id" = @Id
              AND e."TenantId" = @TenantId
              AND e."CompanyId" = @CompanyId
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var row = await connection.QueryFirstOrDefaultAsync<FgsEmployeeDetailRow>(
            new CommandDefinition(sql, new { Id = id, TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return row?.ToDto();
    }

    public async Task<PagedResult<FgsEmployeeSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsEmployeeListFilters filters,
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

        if (filters.StatusId.HasValue)
        {
            where.Add("\"StatusId\" = @StatusId");
        }
        else if (paging.IsActive == true)
        {
            where.Add("\"StatusId\" = @ActiveStatusId");
        }
        else if (paging.IsActive == false)
        {
            where.Add("\"StatusId\" <> @ActiveStatusId");
        }

        if (!string.IsNullOrWhiteSpace(filters.EmployeeNumber))
        {
            where.Add("\"EmployeeNumber\" ILIKE @EmployeeNumber");
        }

        if (filters.EmployeeTypeId.HasValue)
        {
            where.Add("\"EmployeeTypeId\" = @EmployeeTypeId");
        }

        if (!string.IsNullOrWhiteSpace(paging.Search))
        {
            where.Add(
                """
                ("EmployeeNumber" ILIKE @Search
                 OR "DisplayName" ILIKE @Search
                 OR "LegalFirstName" ILIKE @Search
                 OR "LegalLastName" ILIKE @Search
                 OR "OfficeEmail" ILIKE @Search
                 OR "PersonalEmail" ILIKE @Search)
                """);
        }

        var whereClause = string.Join(" AND ", where);
        var orderBy = FgsEmployeeSql.ResolveOrderBy(paging.SortBy, paging.SortDirection);

        var sql = $"""
            SELECT {FgsEmployeeSql.SelectSummaryColumns}
            FROM {FgsEmployeeSql.Table}
            WHERE {whereClause}
            {orderBy}
            LIMIT @PageSize OFFSET @Offset;

            SELECT COUNT(*)
            FROM {FgsEmployeeSql.Table}
            WHERE {whereClause};
            """;

        var parameters = new
        {
            TenantId = tenantId,
            CompanyId = companyId,
            StatusId = filters.StatusId,
            ActiveStatusId = EmployeeStatusIds.Active,
            EmployeeNumber = string.IsNullOrWhiteSpace(filters.EmployeeNumber)
                ? null
                : $"%{filters.EmployeeNumber.Trim()}%",
            EmployeeTypeId = filters.EmployeeTypeId,
            Search = string.IsNullOrWhiteSpace(paging.Search) ? null : $"%{paging.Search.Trim()}%",
            PageSize = pageSize,
            Offset = offset
        };

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

        var rows = (await multi.ReadAsync<FgsEmployeeSummaryRow>()).ToList();
        var totalCount = await multi.ReadSingleAsync<int>();

        return new PagedResult<FgsEmployeeSummaryDto>(
            rows.Select(r => r.ToDto()).ToList(),
            page,
            pageSize,
            totalCount);
    }

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

    public async Task<bool> ExistsByUserIdAsync(
        long userId,
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
}
