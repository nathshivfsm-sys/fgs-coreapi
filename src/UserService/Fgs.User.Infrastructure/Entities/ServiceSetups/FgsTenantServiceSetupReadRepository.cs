using Dapper;
using Fgs.MultiTenancy;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Abstractions.ServiceSetups;
using Fgs.User.Application.Features.ServiceSetups.Dtos;
using Fgs.User.Infrastructure.Common;

namespace Fgs.User.Infrastructure.Entities.ServiceSetups;

internal sealed class FgsTenantServiceSetupReadRepository(
    IUserReadConnectionFactory connectionFactory,
    ITenantContextAccessor tenantContextAccessor) : IFgsTenantServiceSetupReadRepository
{
    public Task<FgsTenantServiceSetupDetailDto?> GetCurrentAsync(CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        return GetByTenantCompanyAsync(tenantId, companyId, cancellationToken);
    }

    public async Task<FgsTenantServiceSetupDetailDto?> GetByTenantCompanyAsync(
        long tenantId,
        long companyId,
        CancellationToken cancellationToken = default)
    {
        var sql = $"""
            SELECT {FgsTenantServiceSetupSql.SelectDetailColumns}
            FROM {FgsTenantServiceSetupSql.Table}
            WHERE "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var row = await connection.QueryFirstOrDefaultAsync<FgsTenantServiceSetupDetailRow>(
            new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return row?.ToDto();
    }
}
