using Dapper;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Abstractions.ServiceAccountsSetups;
using Fgs.User.Application.Common.IdentityCrud;
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

    public async Task<PagedResult<FgsTenantServiceAccountsSetupSummaryDto>> ListAsync(
        IdentityListQuery query,
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

        if (paging.IsActive.HasValue)
        {
            where.Add("\"IsActive\" = @IsActive");
        }

        // Account mappings have no text columns, so a search term cannot match a row.
        if (!string.IsNullOrWhiteSpace(paging.Search))
        {
            where.Add("FALSE");
        }

        var whereClause = string.Join(" AND ", where);
        var orderBy = FgsTenantServiceAccountsSetupSql.ResolveOrderBy(paging.SortBy, paging.SortDirection);

        var sql = $"""
            SELECT {FgsTenantServiceAccountsSetupSql.SelectSummaryColumns}
            FROM {FgsTenantServiceAccountsSetupSql.Table}
            WHERE {whereClause}
            {orderBy}
            LIMIT @PageSize OFFSET @Offset;

            SELECT COUNT(*)
            FROM {FgsTenantServiceAccountsSetupSql.Table}
            WHERE {whereClause};
            """;

        var parameters = new
        {
            TenantId = tenantId,
            CompanyId = companyId,
            IsActive = paging.IsActive,
            PageSize = pageSize,
            Offset = offset
        };

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

        var items = (await multi.ReadAsync<FgsTenantServiceAccountsSetupDetailRow>()).Select(row => row.ToSummary()).ToList();
        var totalCount = await multi.ReadSingleAsync<int>();

        return new PagedResult<FgsTenantServiceAccountsSetupSummaryDto>(items, page, pageSize, totalCount);
    }

    public async Task<IReadOnlyList<FgsTenantServiceAccountsSetupLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var where = new List<string>
        {
            "\"TenantId\" = @TenantId",
            "\"CompanyId\" = @CompanyId"
        };

        if (activeOnly)
        {
            where.Add("\"IsActive\" = true");
        }

        var sql = $"""
            SELECT {FgsTenantServiceAccountsSetupSql.SelectLookupColumns}
            FROM {FgsTenantServiceAccountsSetupSql.Table}
            WHERE {string.Join(" AND ", where)}
            ORDER BY "CompanyId" ASC
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<FgsTenantServiceAccountsSetupLookupRow>(
            new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return rows.Select(row => row.ToDto()).ToList();
    }
}
