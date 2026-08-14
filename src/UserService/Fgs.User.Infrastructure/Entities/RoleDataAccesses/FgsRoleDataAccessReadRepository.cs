using Dapper;
using Fgs.MultiTenancy;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Abstractions.RoleDataAccesses;
using Fgs.User.Application.Features.RoleDataAccesses.Dtos;
using Fgs.User.Infrastructure.Common;

namespace Fgs.User.Infrastructure.Entities.RoleDataAccesses;

internal sealed class FgsRoleDataAccessReadRepository(
    IUserReadConnectionFactory connectionFactory,
    ITenantContextAccessor tenantContextAccessor) : IFgsRoleDataAccessReadRepository
{
    public async Task<IReadOnlyList<FgsRoleDataAccessDetailDto>> ListByRoleIdAsync(
        long fgsRoleId,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var sql = $"""
            SELECT {FgsRoleDataAccessSql.SelectColumns}
            FROM {FgsRoleDataAccessSql.Table}
            WHERE "FgsRoleId" = @FgsRoleId
              AND "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
            ORDER BY "Id" ASC
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<FgsRoleDataAccessRow>(
            new CommandDefinition(
                sql,
                new { FgsRoleId = fgsRoleId, TenantId = tenantId, CompanyId = companyId },
                cancellationToken: cancellationToken));

        return rows.Select(row => row.ToDetailDto()).ToList();
    }
}
