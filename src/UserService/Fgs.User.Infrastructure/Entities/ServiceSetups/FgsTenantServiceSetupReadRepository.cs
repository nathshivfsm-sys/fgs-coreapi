using Dapper;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Abstractions.ServiceSetups;
using Fgs.User.Application.Common.IdentityCrud;
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

    public async Task<PagedResult<FgsTenantServiceSetupSummaryDto>> ListAsync(
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

        if (!string.IsNullOrWhiteSpace(paging.Search))
        {
            where.Add("""
                ("BillHoursFromDispatchOrArrive" ILIKE @Search
                 OR "InvoiceNumberPrefix" ILIKE @Search
                 OR "QuoteNumberPrefix" ILIKE @Search
                 OR "PONumberPrefix" ILIKE @Search
                 OR "WorkOrderNumberPrefix" ILIKE @Search
                 OR "InvoiceBatchNumberFormat" ILIKE @Search
                 OR "EstimateRevisionCreationMode" ILIKE @Search)
                """);
        }

        var whereClause = string.Join(" AND ", where);
        var orderBy = FgsTenantServiceSetupSql.ResolveOrderBy(paging.SortBy, paging.SortDirection);

        var sql = $"""
            SELECT {FgsTenantServiceSetupSql.SelectSummaryColumns}
            FROM {FgsTenantServiceSetupSql.Table}
            WHERE {whereClause}
            {orderBy}
            LIMIT @PageSize OFFSET @Offset;

            SELECT COUNT(*)
            FROM {FgsTenantServiceSetupSql.Table}
            WHERE {whereClause};
            """;

        var parameters = new
        {
            TenantId = tenantId,
            CompanyId = companyId,
            IsActive = paging.IsActive,
            Search = paging.Search is null ? null : $"%{paging.Search.Trim()}%",
            PageSize = pageSize,
            Offset = offset
        };

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

        var items = (await multi.ReadAsync<FgsTenantServiceSetupDetailRow>()).Select(row => row.ToSummary()).ToList();
        var totalCount = await multi.ReadSingleAsync<int>();

        return new PagedResult<FgsTenantServiceSetupSummaryDto>(items, page, pageSize, totalCount);
    }

    public async Task<IReadOnlyList<FgsTenantServiceSetupLookupDto>> LookupAsync(
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
            SELECT {FgsTenantServiceSetupSql.SelectLookupColumns}
            FROM {FgsTenantServiceSetupSql.Table}
            WHERE {string.Join(" AND ", where)}
            ORDER BY "CompanyId" ASC
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<FgsTenantServiceSetupLookupRow>(
            new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return rows.Select(row => row.ToDto()).ToList();
    }
}
