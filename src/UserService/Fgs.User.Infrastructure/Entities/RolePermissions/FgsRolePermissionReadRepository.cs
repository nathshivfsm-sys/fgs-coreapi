using Dapper;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Abstractions.RolePermissions;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.RolePermissions.Dtos;
using Fgs.User.Infrastructure.Common;

namespace Fgs.User.Infrastructure.Entities.RolePermissions;

internal sealed class FgsRolePermissionReadRepository(
    IUserReadConnectionFactory connectionFactory,
    ITenantContextAccessor tenantContextAccessor) : IFgsRolePermissionReadRepository
{
    public async Task<FgsRolePermissionDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var sql = $"""
            SELECT {FgsRolePermissionSql.SelectColumns}
            FROM {FgsRolePermissionSql.Table}
            WHERE "Id" = @Id
              AND "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var row = await connection.QueryFirstOrDefaultAsync<FgsRolePermissionRow>(
            new CommandDefinition(sql, new { Id = id, TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return row?.ToDetailDto();
    }

    public async Task<PagedResult<FgsRolePermissionSummaryDto>> ListAsync(
        IdentityListQuery query,
        FgsRolePermissionListFilters filters,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var paging = query.ToPagedQuery();
        var page = Math.Max(1, paging.Page);
        var pageSize = Math.Clamp(paging.PageSize, 1, 200);
        var offset = (page - 1) * pageSize;

        var where = new List<string>
        {
            "\"TenantId\" = @TenantId",
            "\"CompanyId\" = @CompanyId"
        };

        if (filters.FgsRoleId.HasValue)
        {
            where.Add("\"FgsRoleId\" = @FgsRoleId");
        }

        if (filters.FgsPermissionId.HasValue)
        {
            where.Add("\"FgsPermissionId\" = @FgsPermissionId");
        }

        var whereClause = string.Join(" AND ", where);
        var orderBy = FgsRolePermissionSql.ResolveOrderBy(paging.SortBy, paging.SortDirection);

        var sql = $"""
            SELECT {FgsRolePermissionSql.SelectColumns}
            FROM {FgsRolePermissionSql.Table}
            WHERE {whereClause}
            {orderBy}
            LIMIT @PageSize OFFSET @Offset;

            SELECT COUNT(*)
            FROM {FgsRolePermissionSql.Table}
            WHERE {whereClause};
            """;

        var parameters = new
        {
            TenantId = tenantId,
            CompanyId = companyId,
            FgsRoleId = filters.FgsRoleId,
            FgsPermissionId = filters.FgsPermissionId,
            PageSize = pageSize,
            Offset = offset
        };

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

        var items = (await multi.ReadAsync<FgsRolePermissionRow>()).Select(row => row.ToSummaryDto()).ToList();
        var totalCount = await multi.ReadSingleAsync<int>();

        return new PagedResult<FgsRolePermissionSummaryDto>(items, page, pageSize, totalCount);
    }
}
