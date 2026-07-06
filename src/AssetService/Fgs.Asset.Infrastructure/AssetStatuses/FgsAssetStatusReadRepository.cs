
using Dapper;
using Fgs.Asset.Application.Abstractions.Persistence;
using Fgs.Asset.Application.Abstractions.AssetStatuses;
using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.AssetStatuses.Dtos;
using Fgs.Asset.Infrastructure.Common;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;

namespace Fgs.Asset.Infrastructure.AssetStatuses;

internal sealed class FgsAssetStatusReadRepository : IFgsAssetStatusReadRepository
{
    private readonly IAssetReadConnectionFactory _connectionFactory;
    private readonly ITenantContextAccessor _tenantContextAccessor;
    public FgsAssetStatusReadRepository(IAssetReadConnectionFactory connectionFactory, ITenantContextAccessor tenantContextAccessor)
    { _connectionFactory = connectionFactory; _tenantContextAccessor = tenantContextAccessor; }

    public async Task<FgsAssetStatusDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"SELECT {FgsAssetStatusSql.SelectDetailColumns} FROM {FgsAssetStatusSql.Table} WHERE \"Id\" = @Id AND \"TenantId\" = @TenantId AND \"CompanyId\" = @CompanyId";
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return (await connection.QueryFirstOrDefaultAsync<FgsAssetStatusDetailRow>(new CommandDefinition(sql, new { Id = id, TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken)))?.ToDto();
    }

    public async Task<PagedResult<FgsAssetStatusSummaryDto>> ListAsync(AssetListQuery query, FgsAssetStatusListFilters filters, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var paging = query.ToPagedQuery();
        var page = Math.Max(1, paging.Page);
        var pageSize = Math.Clamp(paging.PageSize, 1, 200);
        var offset = (page - 1) * pageSize;
        var where = new List<string> { "\"TenantId\" = @TenantId", "\"CompanyId\" = @CompanyId" };
        if (paging.IsActive.HasValue) where.Add("\"IsActive\" = @IsActive");
        if (!string.IsNullOrWhiteSpace(filters.Code)) where.Add("\"Code\" = @Code");
        if (!string.IsNullOrWhiteSpace(filters.Name)) where.Add("\"Name\" ILIKE @Name");
        if (!string.IsNullOrWhiteSpace(paging.Search)) where.Add("(\"Code\" ILIKE @Search OR \"Name\" ILIKE @Search OR \"Description\" ILIKE @Search)");
        var whereClause = string.Join(" AND ", where);
        var sql = $"SELECT {FgsAssetStatusSql.SelectSummaryColumns} FROM {FgsAssetStatusSql.Table} WHERE {whereClause} {FgsAssetStatusSql.ResolveOrderBy(paging.SortBy, paging.SortDirection)} LIMIT @PageSize OFFSET @Offset; SELECT COUNT(*) FROM {FgsAssetStatusSql.Table} WHERE {whereClause};";
        var parameters = new { TenantId = tenantId, CompanyId = companyId, IsActive = paging.IsActive, Code = filters.Code?.Trim().ToUpperInvariant(), Name = string.IsNullOrWhiteSpace(filters.Name) ? null : $"%{filters.Name.Trim()}%", Search = string.IsNullOrWhiteSpace(paging.Search) ? null : $"%{paging.Search.Trim()}%", PageSize = pageSize, Offset = offset };
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));
        var rows = (await multi.ReadAsync<FgsAssetStatusSummaryRow>()).ToList();
        return new PagedResult<FgsAssetStatusSummaryDto>(rows.Select(r => r.ToDto()).ToList(), page, pageSize, await multi.ReadSingleAsync<int>());
    }

    public async Task<IReadOnlyList<FgsAssetStatusLookupDto>> LookupAsync(bool activeOnly = true, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var activeFilter = activeOnly ? "AND \"IsActive\" = TRUE" : string.Empty;
        var sql = $"SELECT {FgsAssetStatusSql.SelectLookupColumns} FROM {FgsAssetStatusSql.Table} WHERE \"TenantId\" = @TenantId AND \"CompanyId\" = @CompanyId {activeFilter} ORDER BY \"Name\" ASC";
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return (await connection.QueryAsync<FgsAssetStatusLookupRow>(new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken))).Select(r => r.ToDto()).ToList();
    }

    public async Task<bool> ExistsByCodeAsync(string code, long? excludeId = null, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var exclude = excludeId.HasValue ? "AND \"Id\" <> @ExcludeId" : string.Empty;
        var sql = $"SELECT EXISTS(SELECT 1 FROM {FgsAssetStatusSql.Table} WHERE \"TenantId\" = @TenantId AND \"CompanyId\" = @CompanyId AND \"Code\" = @Code {exclude})";
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId, Code = code.Trim().ToUpperInvariant(), ExcludeId = excludeId }, cancellationToken: cancellationToken));
    }
}
