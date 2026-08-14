using Dapper;
using Fgs.MultiTenancy;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Abstractions.RolePermissions;
using Fgs.User.Application.Features.RolePermissions.Dtos;
using Fgs.User.Infrastructure.Common;

namespace Fgs.User.Infrastructure.Entities.RolePermissions;

internal sealed class FgsRolePermissionReadRepository(
    IUserReadConnectionFactory connectionFactory,
    ITenantContextAccessor tenantContextAccessor) : IFgsRolePermissionReadRepository
{
    public async Task<IReadOnlyList<FgsRolePermissionDetailDto>> ListByRoleIdAsync(
        long fgsRoleId,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var sql = $"""
            SELECT {FgsRolePermissionSql.SelectColumns}
            FROM {FgsRolePermissionSql.Table}
            WHERE "FgsRoleId" = @FgsRoleId
              AND "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
            ORDER BY "Id" ASC
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<FgsRolePermissionRow>(
            new CommandDefinition(
                sql,
                new { FgsRoleId = fgsRoleId, TenantId = tenantId, CompanyId = companyId },
                cancellationToken: cancellationToken));

        return rows.Select(row => row.ToDetailDto()).ToList();
    }
}
