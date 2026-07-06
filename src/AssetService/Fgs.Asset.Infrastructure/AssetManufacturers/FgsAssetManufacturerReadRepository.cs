
using Dapper;
using Fgs.Asset.Application.Abstractions.Persistence;
using Fgs.Asset.Application.Abstractions.AssetManufacturers;
using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.AssetManufacturers.Dtos;
using Fgs.Asset.Infrastructure.Common;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;

namespace Fgs.Asset.Infrastructure.AssetManufacturers;

internal sealed class FgsAssetManufacturerReadRepository : IFgsAssetManufacturerReadRepository
{
    private readonly IAssetReadConnectionFactory _connectionFactory;
    private readonly ITenantContextAccessor _tenantContextAccessor;
    public FgsAssetManufacturerReadRepository(IAssetReadConnectionFactory connectionFactory, ITenantContextAccessor tenantContextAccessor)
    { _connectionFactory = connectionFactory; _tenantContextAccessor = tenantContextAccessor; }

    public async Task<FgsAssetManufacturerDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"SELECT {FgsAssetManufacturerSql.SelectDetailColumns} FROM {FgsAssetManufacturerSql.Table} WHERE \"Id\" = @Id AND \"TenantId\" = @TenantId AND \"CompanyId\" = @CompanyId";
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return (await connection.QueryFirstOrDefaultAsync<FgsAssetManufacturerDetailRow>(new CommandDefinition(sql, new { Id = id, TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken)))?.ToDto();
    }

    public async Task<PagedResult<FgsAssetManufacturerSummaryDto>> ListAsync(AssetListQuery query, FgsAssetManufacturerListFilters filters, CancellationToken cancellationToken = default)
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
        var sql = $"SELECT {FgsAssetManufacturerSql.SelectSummaryColumns} FROM {FgsAssetManufacturerSql.Table} WHERE {whereClause} {FgsAssetManufacturerSql.ResolveOrderBy(paging.SortBy, paging.SortDirection)} LIMIT @PageSize OFFSET @Offset; SELECT COUNT(*) FROM {FgsAssetManufacturerSql.Table} WHERE {whereClause};";
        var parameters = new { TenantId = tenantId, CompanyId = companyId, IsActive = paging.IsActive, Code = filters.Code?.Trim().ToUpperInvariant(), Name = string.IsNullOrWhiteSpace(filters.Name) ? null : $"%{filters.Name.Trim()}%", Search = string.IsNullOrWhiteSpace(paging.Search) ? null : $"%{paging.Search.Trim()}%", PageSize = pageSize, Offset = offset };
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));
        var rows = (await multi.ReadAsync<FgsAssetManufacturerSummaryRow>()).ToList();
        return new PagedResult<FgsAssetManufacturerSummaryDto>(rows.Select(r => r.ToDto()).ToList(), page, pageSize, await multi.ReadSingleAsync<int>());
    }

    public async Task<IReadOnlyList<FgsAssetManufacturerLookupDto>> LookupAsync(bool activeOnly = true, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var activeFilter = activeOnly ? "AND \"IsActive\" = TRUE" : string.Empty;
        var sql = $"SELECT {FgsAssetManufacturerSql.SelectLookupColumns} FROM {FgsAssetManufacturerSql.Table} WHERE \"TenantId\" = @TenantId AND \"CompanyId\" = @CompanyId {activeFilter} ORDER BY \"Name\" ASC";
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return (await connection.QueryAsync<FgsAssetManufacturerLookupRow>(new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken))).Select(r => r.ToDto()).ToList();
    }

    public async Task<bool> ExistsByCodeAsync(string code, long? excludeId = null, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var exclude = excludeId.HasValue ? "AND \"Id\" <> @ExcludeId" : string.Empty;
        var sql = $"SELECT EXISTS(SELECT 1 FROM {FgsAssetManufacturerSql.Table} WHERE \"TenantId\" = @TenantId AND \"CompanyId\" = @CompanyId AND \"Code\" = @Code {exclude})";
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId, Code = code.Trim().ToUpperInvariant(), ExcludeId = excludeId }, cancellationToken: cancellationToken));
    }
}
