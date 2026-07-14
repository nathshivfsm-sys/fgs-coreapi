using Dapper;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.User.Application.Abstractions.ApiWebhookSubscriptions;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.ApiWebhookSubscriptions.Dtos;
using Fgs.User.Infrastructure.Common;

namespace Fgs.User.Infrastructure.Entities.ApiWebhookSubscriptions;

internal sealed class FgsApiWebhookSubscriptionReadRepository(
    IUserReadConnectionFactory connectionFactory,
    ITenantContextAccessor tenantContextAccessor) : IFgsApiWebhookSubscriptionReadRepository
{
    public async Task<FgsApiWebhookSubscriptionDetailDto?> GetByIdAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var sql = $"""
            SELECT {FgsApiWebhookSubscriptionSql.SelectColumns}
            FROM {FgsApiWebhookSubscriptionSql.Table}
            WHERE "Id" = @Id
              AND "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var row = await connection.QueryFirstOrDefaultAsync<FgsApiWebhookSubscriptionRow>(
            new CommandDefinition(
                sql,
                new { Id = id, TenantId = tenantId, CompanyId = companyId },
                cancellationToken: cancellationToken));

        return row?.ToDetailDto();
    }

    public async Task<PagedResult<FgsApiWebhookSubscriptionSummaryDto>> ListAsync(
        IdentityListQuery query,
        FgsApiWebhookSubscriptionListFilters filters,
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

        if (filters.FgsApiWebhookId.HasValue)
        {
            where.Add("\"FgsApiWebhookId\" = @FgsApiWebhookId");
        }

        if (filters.FgsApiEventId.HasValue)
        {
            where.Add("\"FgsApiEventId\" = @FgsApiEventId");
        }

        var whereClause = string.Join(" AND ", where);
        var orderBy = FgsApiWebhookSubscriptionSql.ResolveOrderBy(paging.SortBy, paging.SortDirection);

        var sql = $"""
            SELECT {FgsApiWebhookSubscriptionSql.SelectColumns}
            FROM {FgsApiWebhookSubscriptionSql.Table}
            WHERE {whereClause}
            {orderBy}
            LIMIT @PageSize OFFSET @Offset;

            SELECT COUNT(*)
            FROM {FgsApiWebhookSubscriptionSql.Table}
            WHERE {whereClause};
            """;

        var parameters = new
        {
            TenantId = tenantId,
            CompanyId = companyId,
            FgsApiWebhookId = filters.FgsApiWebhookId,
            FgsApiEventId = filters.FgsApiEventId,
            PageSize = pageSize,
            Offset = offset
        };

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

        var items = (await multi.ReadAsync<FgsApiWebhookSubscriptionRow>())
            .Select(row => row.ToSummaryDto())
            .ToList();
        var totalCount = await multi.ReadSingleAsync<int>();

        return new PagedResult<FgsApiWebhookSubscriptionSummaryDto>(items, page, pageSize, totalCount);
    }
}
