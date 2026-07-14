using Dapper;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.User.Application.Abstractions.ApiSecrets;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.ApiSecrets.Dtos;
using Fgs.User.Infrastructure.Common;

namespace Fgs.User.Infrastructure.Entities.ApiSecrets;

internal sealed class FgsApiSecretReadRepository(
    IUserReadConnectionFactory connectionFactory,
    ITenantContextAccessor tenantContextAccessor) : IFgsApiSecretReadRepository
{
    public async Task<FgsApiSecretDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var sql = $"""
            SELECT {FgsApiSecretSql.SelectDetailColumns}
            FROM {FgsApiSecretSql.Table}
            WHERE "Id" = @Id
              AND "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var row = await connection.QueryFirstOrDefaultAsync<FgsApiSecretDetailRow>(
            new CommandDefinition(sql, new { Id = id, TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return row?.ToDto();
    }

    public async Task<PagedResult<FgsApiSecretSummaryDto>> ListAsync(
        IdentityListQuery query,
        FgsApiSecretListFilters filters,
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

        if (filters.FgsApiClientId.HasValue)
        {
            where.Add("\"FgsApiClientId\" = @FgsApiClientId");
        }

        if (!string.IsNullOrWhiteSpace(paging.Search))
        {
            where.Add("\"Name\" ILIKE @Search");
        }

        var whereClause = string.Join(" AND ", where);
        var orderBy = FgsApiSecretSql.ResolveOrderBy(paging.SortBy, paging.SortDirection);

        var sql = $"""
            SELECT {FgsApiSecretSql.SelectSummaryColumns}
            FROM {FgsApiSecretSql.Table}
            WHERE {whereClause}
            {orderBy}
            LIMIT @PageSize OFFSET @Offset;

            SELECT COUNT(*)
            FROM {FgsApiSecretSql.Table}
            WHERE {whereClause};
            """;

        var parameters = new
        {
            TenantId = tenantId,
            CompanyId = companyId,
            IsActive = paging.IsActive,
            FgsApiClientId = filters.FgsApiClientId,
            Search = paging.Search is null ? null : $"%{paging.Search.Trim()}%",
            PageSize = pageSize,
            Offset = offset
        };

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

        var items = (await multi.ReadAsync<FgsApiSecretSummaryRow>()).Select(row => row.ToDto()).ToList();
        var totalCount = await multi.ReadSingleAsync<int>();

        return new PagedResult<FgsApiSecretSummaryDto>(items, page, pageSize, totalCount);
    }

    public async Task<bool> ExistsByNameAsync(
        long fgsApiClientId,
        string name,
        long? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {FgsApiSecretSql.Table}
                WHERE "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND "FgsApiClientId" = @FgsApiClientId
                  AND "Name" = @Name
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
                    FgsApiClientId = fgsApiClientId,
                    Name = name.Trim(),
                    ExcludeId = excludeId
                },
                cancellationToken: cancellationToken));
    }
}
