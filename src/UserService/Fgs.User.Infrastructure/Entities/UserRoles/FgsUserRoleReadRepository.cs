using Dapper;
using Fgs.MultiTenancy;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Abstractions.UserRoles;
using Fgs.User.Application.Features.UserRoles.Dtos;
using Fgs.User.Infrastructure.Common;

namespace Fgs.User.Infrastructure.Entities.UserRoles;

internal sealed class FgsUserRoleReadRepository(
    IUserReadConnectionFactory connectionFactory,
    ITenantContextAccessor tenantContextAccessor) : IFgsUserRoleReadRepository
{
    public async Task<IReadOnlyList<FgsUserRoleDetailDto>> ListByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var sql = $"""
            SELECT {FgsUserRoleSql.SelectColumns}
            FROM {FgsUserRoleSql.Table}
            WHERE "UserId" = @UserId
              AND "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
            ORDER BY "Id" ASC
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<FgsUserRoleRow>(
            new CommandDefinition(
                sql,
                new { UserId = userId, TenantId = tenantId, CompanyId = companyId },
                cancellationToken: cancellationToken));

        return rows.Select(row => row.ToDetailDto()).ToList();
    }
}
