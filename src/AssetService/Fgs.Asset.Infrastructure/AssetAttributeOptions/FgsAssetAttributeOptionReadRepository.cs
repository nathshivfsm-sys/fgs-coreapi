using Dapper;
using Fgs.Asset.Application.Abstractions.Persistence;
using Fgs.Asset.Application.Abstractions.AssetAttributeOptions;
using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.AssetAttributeOptions.Dtos;
using Fgs.Asset.Infrastructure.Common;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
namespace Fgs.Asset.Infrastructure.AssetAttributeOptions;
internal sealed class FgsAssetAttributeOptionReadRepository : IFgsAssetAttributeOptionReadRepository
{
    private readonly IAssetReadConnectionFactory _connectionFactory;
    private readonly ITenantContextAccessor _tenantContextAccessor;
    public FgsAssetAttributeOptionReadRepository(IAssetReadConnectionFactory connectionFactory, ITenantContextAccessor tenantContextAccessor) { _connectionFactory = connectionFactory; _tenantContextAccessor = tenantContextAccessor; }
    public async Task<FgsAssetAttributeOptionDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default) { var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor); var sql = $"SELECT {FgsAssetAttributeOptionSql.SelectDetailColumns} FROM {FgsAssetAttributeOptionSql.Table} WHERE \"Id\" = @Id AND \"TenantId\" = @TenantId AND \"CompanyId\" = @CompanyId"; await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken); return (await connection.QueryFirstOrDefaultAsync<FgsAssetAttributeOptionDetailRow>(new CommandDefinition(sql, new { Id = id, TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken)))?.ToDto(); }
    public async Task<PagedResult<FgsAssetAttributeOptionSummaryDto>> ListAsync(AssetListQuery query, FgsAssetAttributeOptionListFilters filters, CancellationToken cancellationToken = default) { var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor); var paging = query.ToPagedQuery(); var page = Math.Max(1, paging.Page); var pageSize = Math.Clamp(paging.PageSize, 1, 200); var offset = (page - 1) * pageSize; var where = new List<string> { "\"TenantId\" = @TenantId", "\"CompanyId\" = @CompanyId" }; if (paging.IsActive.HasValue) where.Add("\"IsActive\" = @IsActive"); 
        if (!string.IsNullOrWhiteSpace(filters.OptionCode)) where.Add("\"OptionCode\" = @OptionCode");
        if (!string.IsNullOrWhiteSpace(filters.OptionName)) where.Add("\"OptionName\" ILIKE @OptionName");
        if (filters.AssetAttributeId.HasValue) where.Add("\"AssetAttributeId\" = @AssetAttributeId");
 if (!string.IsNullOrWhiteSpace(paging.Search)) where.Add("(\"OptionCode\" ILIKE @Search OR \"OptionName\" ILIKE @Search)"); var whereClause = string.Join(" AND ", where); var sql = $"SELECT {FgsAssetAttributeOptionSql.SelectSummaryColumns} FROM {FgsAssetAttributeOptionSql.Table} WHERE {whereClause} {FgsAssetAttributeOptionSql.ResolveOrderBy(paging.SortBy, paging.SortDirection)} LIMIT @PageSize OFFSET @Offset; SELECT COUNT(*) FROM {FgsAssetAttributeOptionSql.Table} WHERE {whereClause};"; var parameters = new { TenantId = tenantId, CompanyId = companyId, IsActive = paging.IsActive, OptionCode = filters.OptionCode?.Trim().ToUpperInvariant(), OptionName = string.IsNullOrWhiteSpace(filters.OptionName) ? null : $"%{filters.OptionName.Trim()}%", AssetAttributeId = filters.AssetAttributeId, Search = string.IsNullOrWhiteSpace(paging.Search) ? null : $"%{paging.Search.Trim()}%", PageSize = pageSize, Offset = offset }; await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken); await using var multi = await connection.QueryMultipleAsync(new CommandDefinition(sql, parameters, cancellationToken: cancellationToken)); var rows = (await multi.ReadAsync<FgsAssetAttributeOptionSummaryRow>()).ToList(); return new PagedResult<FgsAssetAttributeOptionSummaryDto>(rows.Select(r => r.ToDto()).ToList(), page, pageSize, await multi.ReadSingleAsync<int>()); }
    public async Task<IReadOnlyList<FgsAssetAttributeOptionLookupDto>> LookupAsync(bool activeOnly = true, CancellationToken cancellationToken = default) { var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor); var activeFilter = activeOnly ? "AND \"IsActive\" = TRUE" : string.Empty; var sql = $"SELECT {FgsAssetAttributeOptionSql.SelectLookupColumns} FROM {FgsAssetAttributeOptionSql.Table} WHERE \"TenantId\" = @TenantId AND \"CompanyId\" = @CompanyId {activeFilter} ORDER BY \"Id\" ASC"; await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken); return (await connection.QueryAsync<FgsAssetAttributeOptionLookupRow>(new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken))).Select(r => r.ToDto()).ToList(); }

    public async Task<bool> ExistsAssetAttributeIdAsync(long assetAttributeId, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        const string sql = "SELECT EXISTS(SELECT 1 FROM asset.\"FgsAssetAttribute\" WHERE \"TenantId\" = @TenantId AND \"CompanyId\" = @CompanyId AND \"Id\" = @Id AND \"IsActive\" = TRUE)";
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId, Id = assetAttributeId }, cancellationToken: cancellationToken));
    }

    public async Task<bool> ExistsByOptionCodeAsync(long assetAttributeId, string optionCode, long? excludeId = null, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var exclude = excludeId.HasValue ? "AND \"Id\" <> @ExcludeId" : string.Empty;
        var sql = $"SELECT EXISTS(SELECT 1 FROM {FgsAssetAttributeOptionSql.Table} WHERE \"TenantId\" = @TenantId AND \"CompanyId\" = @CompanyId AND \"AssetAttributeId\" = @AssetAttributeId AND \"OptionCode\" = @OptionCode {exclude})";
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId, AssetAttributeId = assetAttributeId, OptionCode = optionCode.Trim().ToUpperInvariant(), ExcludeId = excludeId }, cancellationToken: cancellationToken));
    }

}
