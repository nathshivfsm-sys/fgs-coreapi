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
    public async Task<FgsRoleDataAccessDetailDto?> GetByIdAsync(
        long id,
        CancellationToken cancellationToken = default)
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
            new CommandDefinition(
                sql,
                new { Id = id, TenantId = tenantId, CompanyId = companyId },
                cancellationToken: cancellationToken));

        return row?.ToDetailDto();
    }

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

    public async Task<IReadOnlyList<FgsRoleDataAccessLookupDto>> LookupAsync(
        long fgsRoleId,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var sql = $"""
            SELECT "Id", "FgsRoleId", "FgsDataAccessId"
            FROM {FgsRoleDataAccessSql.Table}
            WHERE "FgsRoleId" = @FgsRoleId
              AND "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
            ORDER BY "Id" ASC
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<FgsRoleDataAccessLookupRow>(
            new CommandDefinition(
                sql,
                new { FgsRoleId = fgsRoleId, TenantId = tenantId, CompanyId = companyId },
                cancellationToken: cancellationToken));

        return rows.Select(row => row.ToLookupDto()).ToList();
    }

    public async Task<bool> ExistsByRoleIdAndDataAccessIdAsync(
        long fgsRoleId,
        long fgsDataAccessId,
        long? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {FgsRoleDataAccessSql.Table}
                WHERE "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND "FgsRoleId" = @FgsRoleId
                  AND "FgsDataAccessId" = @FgsDataAccessId
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
                    FgsDataAccessId = fgsDataAccessId,
                    ExcludeId = excludeId
                },
                cancellationToken: cancellationToken));
    }
}
