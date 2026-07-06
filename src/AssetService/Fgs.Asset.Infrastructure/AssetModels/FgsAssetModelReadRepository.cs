using Dapper;
using Fgs.Asset.Application.Abstractions.Persistence;
using Fgs.Asset.Application.Abstractions.AssetModels;
using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.AssetModels.Dtos;
using Fgs.Asset.Infrastructure.Common;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
namespace Fgs.Asset.Infrastructure.AssetModels;
internal sealed class FgsAssetModelReadRepository : IFgsAssetModelReadRepository
{
    private readonly IAssetReadConnectionFactory _connectionFactory;
    private readonly ITenantContextAccessor _tenantContextAccessor;
    public FgsAssetModelReadRepository(IAssetReadConnectionFactory connectionFactory, ITenantContextAccessor tenantContextAccessor) { _connectionFactory = connectionFactory; _tenantContextAccessor = tenantContextAccessor; }
    public async Task<FgsAssetModelDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default) { var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor); var sql = $"SELECT {FgsAssetModelSql.SelectDetailColumns} FROM {FgsAssetModelSql.Table} WHERE \"Id\" = @Id AND \"TenantId\" = @TenantId AND \"CompanyId\" = @CompanyId"; await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken); return (await connection.QueryFirstOrDefaultAsync<FgsAssetModelDetailRow>(new CommandDefinition(sql, new { Id = id, TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken)))?.ToDto(); }
    public async Task<PagedResult<FgsAssetModelSummaryDto>> ListAsync(AssetListQuery query, FgsAssetModelListFilters filters, CancellationToken cancellationToken = default) { var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor); var paging = query.ToPagedQuery(); var page = Math.Max(1, paging.Page); var pageSize = Math.Clamp(paging.PageSize, 1, 200); var offset = (page - 1) * pageSize; var where = new List<string> { "\"TenantId\" = @TenantId", "\"CompanyId\" = @CompanyId" }; if (paging.IsActive.HasValue) where.Add("\"IsActive\" = @IsActive"); 
        if (!string.IsNullOrWhiteSpace(filters.ModelNumber)) where.Add("\"ModelNumber\" ILIKE @ModelNumber");
        if (filters.AssetTypeId.HasValue) where.Add("\"AssetTypeId\" = @AssetTypeId");
        if (filters.AssetManufacturerId.HasValue) where.Add("\"AssetManufacturerId\" = @AssetManufacturerId");
 if (!string.IsNullOrWhiteSpace(paging.Search)) where.Add("(\"ModelNumber\" ILIKE @Search OR \"ModelDescription\" ILIKE @Search)"); var whereClause = string.Join(" AND ", where); var sql = $"SELECT {FgsAssetModelSql.SelectSummaryColumns} FROM {FgsAssetModelSql.Table} WHERE {whereClause} {FgsAssetModelSql.ResolveOrderBy(paging.SortBy, paging.SortDirection)} LIMIT @PageSize OFFSET @Offset; SELECT COUNT(*) FROM {FgsAssetModelSql.Table} WHERE {whereClause};"; var parameters = new { TenantId = tenantId, CompanyId = companyId, IsActive = paging.IsActive, ModelNumber = string.IsNullOrWhiteSpace(filters.ModelNumber) ? null : $"%{filters.ModelNumber.Trim()}%", AssetTypeId = filters.AssetTypeId, AssetManufacturerId = filters.AssetManufacturerId, Search = string.IsNullOrWhiteSpace(paging.Search) ? null : $"%{paging.Search.Trim()}%", PageSize = pageSize, Offset = offset }; await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken); await using var multi = await connection.QueryMultipleAsync(new CommandDefinition(sql, parameters, cancellationToken: cancellationToken)); var rows = (await multi.ReadAsync<FgsAssetModelSummaryRow>()).ToList(); return new PagedResult<FgsAssetModelSummaryDto>(rows.Select(r => r.ToDto()).ToList(), page, pageSize, await multi.ReadSingleAsync<int>()); }
    public async Task<IReadOnlyList<FgsAssetModelLookupDto>> LookupAsync(bool activeOnly = true, CancellationToken cancellationToken = default) { var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor); var activeFilter = activeOnly ? "AND \"IsActive\" = TRUE" : string.Empty; var sql = $"SELECT {FgsAssetModelSql.SelectLookupColumns} FROM {FgsAssetModelSql.Table} WHERE \"TenantId\" = @TenantId AND \"CompanyId\" = @CompanyId {activeFilter} ORDER BY \"ModelNumber\" ASC"; await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken); return (await connection.QueryAsync<FgsAssetModelLookupRow>(new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken))).Select(r => r.ToDto()).ToList(); }

    public async Task<bool> ExistsAssetTypeIdAsync(long assetTypeId, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        const string sql = "SELECT EXISTS(SELECT 1 FROM asset.\"FgsAssetType\" WHERE \"TenantId\" = @TenantId AND \"CompanyId\" = @CompanyId AND \"Id\" = @Id AND \"IsActive\" = TRUE)";
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId, Id = assetTypeId }, cancellationToken: cancellationToken));
    }

    public async Task<bool> ExistsAssetManufacturerIdAsync(long assetManufacturerId, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        const string sql = "SELECT EXISTS(SELECT 1 FROM asset.\"FgsAssetManufacturer\" WHERE \"TenantId\" = @TenantId AND \"CompanyId\" = @CompanyId AND \"Id\" = @Id AND \"IsActive\" = TRUE)";
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId, Id = assetManufacturerId }, cancellationToken: cancellationToken));
    }

}
