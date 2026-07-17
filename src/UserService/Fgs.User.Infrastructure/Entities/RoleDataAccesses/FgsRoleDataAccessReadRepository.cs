using Dapper;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Abstractions.RoleDataAccesses;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.RoleDataAccesses.Dtos;
using Fgs.User.Infrastructure.Common;

namespace Fgs.User.Infrastructure.Entities.RoleDataAccesses;

internal sealed class FgsRoleDataAccessReadRepository(
    IUserReadConnectionFactory connectionFactory,
    ITenantContextAccessor tenantContextAccessor) : IFgsRoleDataAccessReadRepository
{
    public async Task<FgsRoleDataAccessDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var sql = $"""
            SELECT {FgsRoleDataAccessSql.SelectColumns}
            FROM {FgsRoleDataAccessSql.Table}
            WHERE "Id" = @Id
              AND "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var row = await connection.QueryFirstOrDefaultAsync<FgsRoleDataAccessRow>(
            new CommandDefinition(sql, new { Id = id, TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return row?.ToDetailDto();
    }

    public async Task<PagedResult<FgsRoleDataAccessSummaryDto>> ListAsync(
        IdentityListQuery query,
        FgsRoleDataAccessListFilters filters,
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

        if (filters.FgsDataAccessId.HasValue)
        {
            where.Add("\"FgsDataAccessId\" = @FgsDataAccessId");
        }

        var whereClause = string.Join(" AND ", where);
        var orderBy = FgsRoleDataAccessSql.ResolveOrderBy(paging.SortBy, paging.SortDirection);

        var sql = $"""
            SELECT {FgsRoleDataAccessSql.SelectColumns}
            FROM {FgsRoleDataAccessSql.Table}
            WHERE {whereClause}
            {orderBy}
            LIMIT @PageSize OFFSET @Offset;

            SELECT COUNT(*)
            FROM {FgsRoleDataAccessSql.Table}
            WHERE {whereClause};
            """;

        var parameters = new
        {
            TenantId = tenantId,
            CompanyId = companyId,
            FgsRoleId = filters.FgsRoleId,
            FgsDataAccessId = filters.FgsDataAccessId,
            PageSize = pageSize,
            Offset = offset
        };

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

        var items = (await multi.ReadAsync<FgsRoleDataAccessRow>()).Select(row => row.ToSummaryDto()).ToList();
        var totalCount = await multi.ReadSingleAsync<int>();

        return new PagedResult<FgsRoleDataAccessSummaryDto>(items, page, pageSize, totalCount);
    }
}
