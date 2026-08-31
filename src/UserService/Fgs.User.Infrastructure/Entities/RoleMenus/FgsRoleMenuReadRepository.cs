using Dapper;
using Fgs.MultiTenancy;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Abstractions.RoleMenus;
using Fgs.User.Application.Features.RoleMenus.Dtos;
using Fgs.User.Infrastructure.Common;

namespace Fgs.User.Infrastructure.Entities.RoleMenus;

internal sealed class FgsRoleMenuReadRepository(
    IUserReadConnectionFactory connectionFactory,
    ITenantContextAccessor tenantContextAccessor) : IFgsRoleMenuReadRepository
{
    public async Task<IReadOnlyList<FgsRoleMenuDetailDto>> ListByRoleIdAsync(
        long roleId,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var sql = $"""
            SELECT {FgsRoleMenuSql.SelectColumns}
            FROM {FgsRoleMenuSql.Table}
            WHERE "RoleId" = @RoleId
              AND "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
            ORDER BY "DisplayOrder" ASC, "Id" ASC
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<FgsRoleMenuRow>(
            new CommandDefinition(
                sql,
                new { RoleId = roleId, TenantId = tenantId, CompanyId = companyId },
                cancellationToken: cancellationToken));

        return rows.Select(row => row.ToDetailDto()).ToList();
    }
}
