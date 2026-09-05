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
    public async Task<FgsRolePermissionDetailDto?> GetByIdAsync(
        long id,
        CancellationToken cancellationToken = default)
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
            new CommandDefinition(
                sql,
                new { Id = id, TenantId = tenantId, CompanyId = companyId },
                cancellationToken: cancellationToken));

        return row?.ToDetailDto();
    }

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

    public async Task<IReadOnlyList<FgsRolePermissionLookupDto>> LookupAsync(
        long fgsRoleId,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var sql = $"""
            SELECT "Id", "FgsRoleId", "FgsPermissionId"
            FROM {FgsRolePermissionSql.Table}
            WHERE "FgsRoleId" = @FgsRoleId
              AND "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
            ORDER BY "Id" ASC
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<FgsRolePermissionLookupRow>(
            new CommandDefinition(
                sql,
                new { FgsRoleId = fgsRoleId, TenantId = tenantId, CompanyId = companyId },
                cancellationToken: cancellationToken));

        return rows.Select(row => row.ToLookupDto()).ToList();
    }

    public async Task<bool> ExistsByRoleIdAndPermissionIdAsync(
        long fgsRoleId,
        long fgsPermissionId,
        long? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {FgsRolePermissionSql.Table}
                WHERE "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND "FgsRoleId" = @FgsRoleId
                  AND "FgsPermissionId" = @FgsPermissionId
                  AND (@ExcludeId IS NULL OR "Id" <> @ExcludeId)
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
                    FgsRoleId = fgsRoleId,
                    FgsPermissionId = fgsPermissionId,
                    ExcludeId = excludeId
                },
                cancellationToken: cancellationToken));
    }
}
