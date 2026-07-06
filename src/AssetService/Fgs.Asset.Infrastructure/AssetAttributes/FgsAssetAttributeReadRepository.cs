using Dapper;
using Fgs.Asset.Application.Abstractions.Persistence;
using Fgs.Asset.Application.Abstractions.AssetAttributes;
using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.AssetAttributes.Dtos;
using Fgs.Asset.Infrastructure.Common;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
namespace Fgs.Asset.Infrastructure.AssetAttributes;
internal sealed class FgsAssetAttributeReadRepository : IFgsAssetAttributeReadRepository
{
    private readonly IAssetReadConnectionFactory _connectionFactory;
    private readonly ITenantContextAccessor _tenantContextAccessor;
    public FgsAssetAttributeReadRepository(IAssetReadConnectionFactory connectionFactory, ITenantContextAccessor tenantContextAccessor) { _connectionFactory = connectionFactory; _tenantContextAccessor = tenantContextAccessor; }
    public async Task<FgsAssetAttributeDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default) { var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor); var sql = $"SELECT {FgsAssetAttributeSql.SelectDetailColumns} FROM {FgsAssetAttributeSql.Table} WHERE \"Id\" = @Id AND \"TenantId\" = @TenantId AND \"CompanyId\" = @CompanyId"; await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken); return (await connection.QueryFirstOrDefaultAsync<FgsAssetAttributeDetailRow>(new CommandDefinition(sql, new { Id = id, TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken)))?.ToDto(); }
    public async Task<PagedResult<FgsAssetAttributeSummaryDto>> ListAsync(AssetListQuery query, FgsAssetAttributeListFilters filters, CancellationToken cancellationToken = default) { var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor); var paging = query.ToPagedQuery(); var page = Math.Max(1, paging.Page); var pageSize = Math.Clamp(paging.PageSize, 1, 200); var offset = (page - 1) * pageSize; var where = new List<string> { "\"TenantId\" = @TenantId", "\"CompanyId\" = @CompanyId" }; if (paging.IsActive.HasValue) where.Add("\"IsActive\" = @IsActive"); 
        if (!string.IsNullOrWhiteSpace(filters.AttributeCode)) where.Add("\"AttributeCode\" = @AttributeCode");
        if (!string.IsNullOrWhiteSpace(filters.AttributeName)) where.Add("\"AttributeName\" ILIKE @AttributeName");
        if (filters.AssetTypeId.HasValue) where.Add("\"AssetTypeId\" = @AssetTypeId");
 if (!string.IsNullOrWhiteSpace(paging.Search)) where.Add("(\"AttributeCode\" ILIKE @Search OR \"AttributeName\" ILIKE @Search)"); var whereClause = string.Join(" AND ", where); var sql = $"SELECT {FgsAssetAttributeSql.SelectSummaryColumns} FROM {FgsAssetAttributeSql.Table} WHERE {whereClause} {FgsAssetAttributeSql.ResolveOrderBy(paging.SortBy, paging.SortDirection)} LIMIT @PageSize OFFSET @Offset; SELECT COUNT(*) FROM {FgsAssetAttributeSql.Table} WHERE {whereClause};"; var parameters = new { TenantId = tenantId, CompanyId = companyId, IsActive = paging.IsActive, AttributeCode = filters.AttributeCode?.Trim().ToUpperInvariant(), AttributeName = string.IsNullOrWhiteSpace(filters.AttributeName) ? null : $"%{filters.AttributeName.Trim()}%", AssetTypeId = filters.AssetTypeId, Search = string.IsNullOrWhiteSpace(paging.Search) ? null : $"%{paging.Search.Trim()}%", PageSize = pageSize, Offset = offset }; await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken); await using var multi = await connection.QueryMultipleAsync(new CommandDefinition(sql, parameters, cancellationToken: cancellationToken)); var rows = (await multi.ReadAsync<FgsAssetAttributeSummaryRow>()).ToList(); return new PagedResult<FgsAssetAttributeSummaryDto>(rows.Select(r => r.ToDto()).ToList(), page, pageSize, await multi.ReadSingleAsync<int>()); }
    public async Task<IReadOnlyList<FgsAssetAttributeLookupDto>> LookupAsync(bool activeOnly = true, CancellationToken cancellationToken = default) { var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor); var activeFilter = activeOnly ? "AND \"IsActive\" = TRUE" : string.Empty; var sql = $"SELECT {FgsAssetAttributeSql.SelectLookupColumns} FROM {FgsAssetAttributeSql.Table} WHERE \"TenantId\" = @TenantId AND \"CompanyId\" = @CompanyId {activeFilter} ORDER BY \"Id\" ASC"; await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken); return (await connection.QueryAsync<FgsAssetAttributeLookupRow>(new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken))).Select(r => r.ToDto()).ToList(); }

    public async Task<bool> ExistsAssetTypeIdAsync(long assetTypeId, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        const string sql = "SELECT EXISTS(SELECT 1 FROM asset.\"FgsAssetType\" WHERE \"TenantId\" = @TenantId AND \"CompanyId\" = @CompanyId AND \"Id\" = @Id AND \"IsActive\" = TRUE)";
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId, Id = assetTypeId }, cancellationToken: cancellationToken));
    }

    public async Task<bool> ExistsByAttributeCodeAsync(long assetTypeId, string attributeCode, long? excludeId = null, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var exclude = excludeId.HasValue ? "AND \"Id\" <> @ExcludeId" : string.Empty;
        var sql = $"SELECT EXISTS(SELECT 1 FROM {FgsAssetAttributeSql.Table} WHERE \"TenantId\" = @TenantId AND \"CompanyId\" = @CompanyId AND \"AssetTypeId\" = @AssetTypeId AND \"AttributeCode\" = @AttributeCode {exclude})";
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId, AssetTypeId = assetTypeId, AttributeCode = attributeCode.Trim().ToUpperInvariant(), ExcludeId = excludeId }, cancellationToken: cancellationToken));
    }

}
