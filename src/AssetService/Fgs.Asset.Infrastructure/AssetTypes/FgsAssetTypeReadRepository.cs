
using Dapper;
using Fgs.Asset.Application.Abstractions.Persistence;
using Fgs.Asset.Application.Abstractions.AssetTypes;
using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.AssetTypes.Dtos;
using Fgs.Asset.Infrastructure.Common;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;

namespace Fgs.Asset.Infrastructure.AssetTypes;

internal sealed class FgsAssetTypeReadRepository : IFgsAssetTypeReadRepository
{
    private readonly IAssetReadConnectionFactory _connectionFactory;
    private readonly ITenantContextAccessor _tenantContextAccessor;
    public FgsAssetTypeReadRepository(IAssetReadConnectionFactory connectionFactory, ITenantContextAccessor tenantContextAccessor)
    { _connectionFactory = connectionFactory; _tenantContextAccessor = tenantContextAccessor; }

    public async Task<FgsAssetTypeDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"SELECT {FgsAssetTypeSql.SelectDetailColumns} FROM {FgsAssetTypeSql.Table} WHERE \"Id\" = @Id AND \"TenantId\" = @TenantId AND \"CompanyId\" = @CompanyId";
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return (await connection.QueryFirstOrDefaultAsync<FgsAssetTypeDetailRow>(new CommandDefinition(sql, new { Id = id, TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken)))?.ToDto();
    }

    public async Task<PagedResult<FgsAssetTypeSummaryDto>> ListAsync(AssetListQuery query, FgsAssetTypeListFilters filters, CancellationToken cancellationToken = default)
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
        var sql = $"SELECT {FgsAssetTypeSql.SelectSummaryColumns} FROM {FgsAssetTypeSql.Table} WHERE {whereClause} {FgsAssetTypeSql.ResolveOrderBy(paging.SortBy, paging.SortDirection)} LIMIT @PageSize OFFSET @Offset; SELECT COUNT(*) FROM {FgsAssetTypeSql.Table} WHERE {whereClause};";
        var parameters = new { TenantId = tenantId, CompanyId = companyId, IsActive = paging.IsActive, Code = filters.Code?.Trim().ToUpperInvariant(), Name = string.IsNullOrWhiteSpace(filters.Name) ? null : $"%{filters.Name.Trim()}%", Search = string.IsNullOrWhiteSpace(paging.Search) ? null : $"%{paging.Search.Trim()}%", PageSize = pageSize, Offset = offset };
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));
        var rows = (await multi.ReadAsync<FgsAssetTypeSummaryRow>()).ToList();
        return new PagedResult<FgsAssetTypeSummaryDto>(rows.Select(r => r.ToDto()).ToList(), page, pageSize, await multi.ReadSingleAsync<int>());
    }

    public async Task<IReadOnlyList<FgsAssetTypeLookupDto>> LookupAsync(bool activeOnly = true, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var activeFilter = activeOnly ? "AND \"IsActive\" = TRUE" : string.Empty;
        var sql = $"SELECT {FgsAssetTypeSql.SelectLookupColumns} FROM {FgsAssetTypeSql.Table} WHERE \"TenantId\" = @TenantId AND \"CompanyId\" = @CompanyId {activeFilter} ORDER BY \"Name\" ASC";
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return (await connection.QueryAsync<FgsAssetTypeLookupRow>(new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken))).Select(r => r.ToDto()).ToList();
    }

    public async Task<bool> ExistsByCodeAsync(string code, long? excludeId = null, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var exclude = excludeId.HasValue ? "AND \"Id\" <> @ExcludeId" : string.Empty;
        var sql = $"SELECT EXISTS(SELECT 1 FROM {FgsAssetTypeSql.Table} WHERE \"TenantId\" = @TenantId AND \"CompanyId\" = @CompanyId AND \"Code\" = @Code {exclude})";
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId, Code = code.Trim().ToUpperInvariant(), ExcludeId = excludeId }, cancellationToken: cancellationToken));
    }
}
