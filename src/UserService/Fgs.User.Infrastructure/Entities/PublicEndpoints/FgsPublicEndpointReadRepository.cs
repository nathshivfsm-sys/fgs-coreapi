using Dapper;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.User.Application.Abstractions.PublicEndpoints;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.PublicEndpoints;
using Fgs.User.Application.Features.PublicEndpoints.Dtos;
using Fgs.User.Infrastructure.Common;

namespace Fgs.User.Infrastructure.Entities.PublicEndpoints;

internal sealed class FgsPublicEndpointReadRepository(
    IUserReadConnectionFactory connectionFactory,
    ITenantContextAccessor tenantContextAccessor) : IFgsPublicEndpointReadRepository
{
    public async Task<FgsPublicEndpointDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var sql = $"""
            SELECT {FgsPublicEndpointSql.SelectDetailColumns}
            FROM {FgsPublicEndpointSql.Table}
            WHERE "Id" = @Id
              AND "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var row = await connection.QueryFirstOrDefaultAsync<FgsPublicEndpointDetailRow>(
            new CommandDefinition(sql, new { Id = id, TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return row?.ToDto();
    }

    public async Task<PagedResult<FgsPublicEndpointSummaryDto>> ListAsync(
        IdentityListQuery query,
        FgsPublicEndpointListFilters filters,
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

        if (!string.IsNullOrWhiteSpace(filters.EndpointType))
        {
            where.Add("\"EndpointType\" = @EndpointType");
        }

        if (!string.IsNullOrWhiteSpace(filters.EnvironmentCode))
        {
            where.Add("\"EnvironmentCode\" = @EnvironmentCode");
        }

        if (!string.IsNullOrWhiteSpace(paging.Search))
        {
            where.Add(
                "(\"EndpointType\" ILIKE @Search OR \"EnvironmentCode\" ILIKE @Search OR \"BaseUrl\" ILIKE @Search OR \"DisplayName\" ILIKE @Search)");
        }

        var whereClause = string.Join(" AND ", where);
        var orderBy = FgsPublicEndpointSql.ResolveOrderBy(paging.SortBy, paging.SortDirection);

        var sql = $"""
            SELECT {FgsPublicEndpointSql.SelectSummaryColumns}
            FROM {FgsPublicEndpointSql.Table}
            WHERE {whereClause}
            {orderBy}
            LIMIT @PageSize OFFSET @Offset;

            SELECT COUNT(*)
            FROM {FgsPublicEndpointSql.Table}
            WHERE {whereClause};
            """;

        var parameters = new
        {
            TenantId = tenantId,
            CompanyId = companyId,
            IsActive = paging.IsActive,
            EndpointType = filters.EndpointType is null ? null : PublicEndpointCodes.Normalize(filters.EndpointType),
            EnvironmentCode = filters.EnvironmentCode is null
                ? null
                : PublicEndpointCodes.Normalize(filters.EnvironmentCode),
            Search = paging.Search is null ? null : $"%{paging.Search.Trim()}%",
            PageSize = pageSize,
            Offset = offset
        };

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

        var items = (await multi.ReadAsync<FgsPublicEndpointSummaryRow>()).Select(row => row.ToDto()).ToList();
        var totalCount = await multi.ReadSingleAsync<int>();

        return new PagedResult<FgsPublicEndpointSummaryDto>(items, page, pageSize, totalCount);
    }

    public async Task<IReadOnlyList<FgsPublicEndpointLookupDto>> LookupAsync(
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
            SELECT {FgsPublicEndpointSql.SelectLookupColumns}
            FROM {FgsPublicEndpointSql.Table}
            WHERE {string.Join(" AND ", where)}
            ORDER BY "EndpointType" ASC, "EnvironmentCode" ASC
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<FgsPublicEndpointLookupRow>(
            new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return rows.Select(row => row.ToDto()).ToList();
    }

    public async Task<IReadOnlyList<FgsPublicEndpointDetailDto>> ListActiveForTenantCompanyAsync(
        long tenantId,
        long companyId,
        CancellationToken cancellationToken = default)
    {
        var sql = $"""
            SELECT {FgsPublicEndpointSql.SelectDetailColumns}
            FROM {FgsPublicEndpointSql.Table}
            WHERE "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
              AND "IsActive" = true
            ORDER BY "EndpointType" ASC, "EnvironmentCode" ASC
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<FgsPublicEndpointDetailRow>(
            new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return rows.Select(row => row.ToDto()).ToList();
    }

    public async Task<bool> ExistsByTypeAndEnvironmentAsync(
        string endpointType,
        string environmentCode,
        long? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {FgsPublicEndpointSql.Table}
                WHERE "TenantId" = @TenantId
                  AND "CompanyId" = @CompanyId
                  AND "EndpointType" = @EndpointType
                  AND "EnvironmentCode" = @EnvironmentCode
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
                    EndpointType = PublicEndpointCodes.Normalize(endpointType),
                    EnvironmentCode = PublicEndpointCodes.Normalize(environmentCode),
                    ExcludeId = excludeId
                },
                cancellationToken: cancellationToken));
    }
}
