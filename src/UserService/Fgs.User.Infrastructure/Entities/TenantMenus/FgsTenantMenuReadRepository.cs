using Dapper;
using Fgs.MultiTenancy;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Abstractions.TenantMenus;
using Fgs.User.Application.Features.TenantMenus.Dtos;
using Fgs.User.Infrastructure.Common;

namespace Fgs.User.Infrastructure.Entities.TenantMenus;

internal sealed class FgsTenantMenuReadRepository(
    IUserReadConnectionFactory connectionFactory,
    ITenantContextAccessor tenantContextAccessor) : IFgsTenantMenuReadRepository
{
    public async Task<IReadOnlyList<FgsTenantMenuDetailDto>> ListAsync(CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var sql = $"""
            SELECT {FgsTenantMenuSql.SelectColumns}
            FROM {FgsTenantMenuSql.Table}
            WHERE "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
            ORDER BY "DisplayOrder" ASC, "Id" ASC
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<FgsTenantMenuRow>(
            new CommandDefinition(
                sql,
                new { TenantId = tenantId, CompanyId = companyId },
                cancellationToken: cancellationToken));

        return rows.Select(row => row.ToDetailDto()).ToList();
    }
}
