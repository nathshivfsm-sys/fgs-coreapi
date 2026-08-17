using Dapper;
using Fgs.MultiTenancy;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Abstractions.ServiceAccountsSetups;
using Fgs.User.Application.Features.ServiceAccountsSetups.Dtos;
using Fgs.User.Infrastructure.Common;

namespace Fgs.User.Infrastructure.Entities.ServiceAccountsSetups;

internal sealed class FgsTenantServiceAccountsSetupReadRepository(
    IUserReadConnectionFactory connectionFactory,
    ITenantContextAccessor tenantContextAccessor) : IFgsTenantServiceAccountsSetupReadRepository
{
    public Task<FgsTenantServiceAccountsSetupDetailDto?> GetCurrentAsync(CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        return GetByTenantCompanyAsync(tenantId, companyId, cancellationToken);
    }

    public async Task<FgsTenantServiceAccountsSetupDetailDto?> GetByTenantCompanyAsync(
        long tenantId,
        long companyId,
        CancellationToken cancellationToken = default)
    {
        var sql = $"""
            SELECT {FgsTenantServiceAccountsSetupSql.SelectDetailColumns}
            FROM {FgsTenantServiceAccountsSetupSql.Table}
            WHERE "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var row = await connection.QueryFirstOrDefaultAsync<FgsTenantServiceAccountsSetupDetailRow>(
            new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return row?.ToDto();
    }
}
