using Dapper;
using Fgs.MultiTenancy;
using Fgs.Security.Constants;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Abstractions.Users;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.Users.Dtos;
using Fgs.User.Infrastructure.Common;

namespace Fgs.User.Infrastructure.Entities.Users;

internal sealed class FgsUserReadRepository(
    IUserReadConnectionFactory connectionFactory,
    ITenantContextAccessor tenantContextAccessor) : IFgsUserReadRepository
{
    private static readonly FgsUserListSummaryDto EmptySummary = new(0, 0, 0, 0, 0);

    private const string LatestInvitationJoin = """
        LEFT JOIN LATERAL (
            SELECT i."Status"
            FROM identity."FgsInvitation" i
            WHERE i."UserId" = u."Id"
            ORDER BY i."CreatedOn" DESC
            LIMIT 1
        ) inv ON TRUE
        """;

    private const string PrimaryRoleJoin = """
        LEFT JOIN LATERAL (
            SELECT ur0."FgsRoleId"
            FROM identity."FgsUserRole" ur0
            WHERE ur0."UserId" = u."Id"
              AND ur0."TenantId" = u."TenantId"
              AND ur0."CompanyId" = u."CompanyId"
            ORDER BY ur0."Id" ASC
            LIMIT 1
        ) ur ON TRUE
        LEFT JOIN identity."FgsRole" r
            ON r."Id" = ur."FgsRoleId"
           AND r."TenantId" = u."TenantId"
           AND r."CompanyId" = u."CompanyId"
        """;

    // Company-scoped card counts — ignore list filters (search/role/isActive/email/displayName).
    // ActiveRegistered matches HasAcceptedInvitation / detail DTO: any invitation Status = 'Accepted'
    // (not IsActive, not EntraObjectId). Admins = assigned RoleCode TENANT_ADMIN (only built-in admin code).
    private const string SummarySql = """
        SELECT
            COUNT(*)::int AS "TotalUsers",
            COUNT(*) FILTER (WHERE inv."Status" = 'Pending')::int AS "PendingInvitation",
            COUNT(*) FILTER (
                WHERE EXISTS (
                    SELECT 1
                    FROM identity."FgsInvitation" ai
                    WHERE ai."UserId" = u."Id"
                      AND ai."Status" = 'Accepted'
                )
            )::int AS "ActiveRegistered",
            COUNT(*) FILTER (WHERE u."IsActive" = FALSE)::int AS "Inactive",
            COUNT(*) FILTER (
                WHERE EXISTS (
                    SELECT 1
                    FROM identity."FgsUserRole" ura
                    INNER JOIN identity."FgsRole" ra
                        ON ra."Id" = ura."FgsRoleId"
                       AND ra."TenantId" = u."TenantId"
                       AND ra."CompanyId" = u."CompanyId"
                    WHERE ura."UserId" = u."Id"
                      AND ura."TenantId" = u."TenantId"
                      AND ura."CompanyId" = u."CompanyId"
                      AND ra."RoleCode" = @AdminRoleCode
                )
            )::int AS "Admins"
        FROM identity."FgsUser" u
        LEFT JOIN LATERAL (
            SELECT i."Status"
            FROM identity."FgsInvitation" i
            WHERE i."UserId" = u."Id"
            ORDER BY i."CreatedOn" DESC
            LIMIT 1
        ) inv ON TRUE
        WHERE u."TenantId" = @TenantId
          AND u."CompanyId" = @CompanyId
          AND u."IsDeleted" = FALSE;
        """;

    public async Task<FgsUserDetailDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var sql = $"""
            SELECT {FgsUserSql.SelectDetailColumns}
            FROM {FgsUserSql.UserTable} u
            {PrimaryRoleJoin}
            {LatestInvitationJoin}
            WHERE u."Id" = @Id
              AND u."TenantId" = @TenantId
              AND u."CompanyId" = @CompanyId
              AND u."IsDeleted" = FALSE
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var row = await connection.QueryFirstOrDefaultAsync<FgsUserDetailRow>(
            new CommandDefinition(sql, new { Id = id, TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return row?.ToDto();
    }

    public async Task<FgsUserListResultDto> ListAsync(
        IdentityListQuery query,
        FgsUserListFilters filters,
        bool includeSummary = true,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var paging = query.ToPagedQuery();
        var page = Math.Max(1, paging.Page);
        var pageSize = Math.Clamp(paging.PageSize, 1, 200);
        var offset = (page - 1) * pageSize;

        var where = new List<string>
        {
            "u.\"TenantId\" = @TenantId",
            "u.\"CompanyId\" = @CompanyId",
            "u.\"IsDeleted\" = FALSE"
        };

        if (paging.IsActive.HasValue)
        {
            where.Add("u.\"IsActive\" = @IsActive");
        }

        if (!string.IsNullOrWhiteSpace(filters.Email))
        {
            where.Add("u.\"Email\" ILIKE @Email");
        }

        if (!string.IsNullOrWhiteSpace(filters.DisplayName))
        {
            where.Add("u.\"DisplayName\" ILIKE @DisplayName");
        }

        var roleIds = filters.RoleIds is { Count: > 0 }
            ? filters.RoleIds.Distinct().ToArray()
            : [];

        if (roleIds.Length > 0)
        {
            where.Add("""
                EXISTS (
                    SELECT 1
                    FROM identity."FgsUserRole" urf
                    WHERE urf."UserId" = u."Id"
                      AND urf."TenantId" = u."TenantId"
                      AND urf."CompanyId" = u."CompanyId"
                      AND urf."FgsRoleId" = ANY(@RoleIds)
                )
                """);
        }

        if (!string.IsNullOrWhiteSpace(paging.Search))
        {
            where.Add(
                "(u.\"Email\" ILIKE @Search OR u.\"DisplayName\" ILIKE @Search OR u.\"PhoneNumber\" ILIKE @Search)");
        }

        var whereClause = string.Join(" AND ", where);
        var orderBy = FgsUserSql.ResolveOrderBy(paging.SortBy, paging.SortDirection);

        var sql = $"""
            SELECT {FgsUserSql.SelectSummaryColumns}
            FROM {FgsUserSql.UserTable} u
            {PrimaryRoleJoin}
            {LatestInvitationJoin}
            WHERE {whereClause}
            {orderBy}
            LIMIT @PageSize OFFSET @Offset;

            SELECT COUNT(*)
            FROM {FgsUserSql.UserTable} u
            {PrimaryRoleJoin}
            WHERE {whereClause};
            {(includeSummary ? SummarySql : string.Empty)}
            """;

        var parameters = new
        {
            TenantId = tenantId,
            CompanyId = companyId,
            IsActive = paging.IsActive,
            Email = filters.Email is null ? null : $"%{filters.Email.Trim()}%",
            DisplayName = filters.DisplayName is null ? null : $"%{filters.DisplayName.Trim()}%",
            RoleIds = roleIds,
            Search = paging.Search is null ? null : $"%{paging.Search.Trim()}%",
            PageSize = pageSize,
            Offset = offset,
            AdminRoleCode = FgsRoleCodes.TenantAdmin
        };

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

        var rows = (await multi.ReadAsync<FgsUserSummaryRow>()).ToList();
        var totalCount = await multi.ReadSingleAsync<int>();

        FgsUserListSummaryDto summary;
        if (includeSummary)
        {
            var summaryRow = await multi.ReadSingleAsync<FgsUserListSummaryRow>();
            summary = summaryRow.ToDto();
        }
        else
        {
            summary = EmptySummary;
        }

        return new FgsUserListResultDto(
            rows.Select(r => r.ToDto()).ToList(),
            page,
            pageSize,
            totalCount,
            summary);
    }

    public async Task<bool> ExistsByEmailAsync(
        string email,
        Guid? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {FgsUserSql.UserTable}
                WHERE "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND LOWER("Email") = LOWER(@Email)
                  AND "IsDeleted" = FALSE
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
                    Email = email.Trim(),
                    ExcludeId = excludeId
                },
                cancellationToken: cancellationToken));
    }

    public async Task<bool> HasAcceptedInvitationAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {FgsUserSql.InvitationTable} i
                INNER JOIN {FgsUserSql.UserTable} u ON u."Id" = i."UserId"
                WHERE i."UserId" = @UserId
                  AND u."TenantId" = @TenantId
                  AND u."CompanyId" = @CompanyId
                  AND i."Status" = 'Accepted'
            )
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                sql,
                new { UserId = userId, TenantId = tenantId, CompanyId = companyId },
                cancellationToken: cancellationToken));
    }
}
