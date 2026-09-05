using Dapper;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Persistence;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrixMaterialTiers;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixMaterialTiers.Dtos;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.SetupPricingMatrixMaterialTiers;

internal sealed class FgsSetupPricingMatrixMaterialTierReadRepository(ISetupReadConnectionFactory _connectionFactory, ITenantContextAccessor _tenantContextAccessor) : IFgsSetupPricingMatrixMaterialTierReadRepository
{
    public async Task<FgsSetupPricingMatrixMaterialTierDetailDto?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"SELECT {FgsSetupPricingMatrixMaterialTierSql.DetailColumns} FROM {FgsSetupPricingMatrixMaterialTierSql.Table} WHERE \"Id\"=@Id AND \"TenantId\"=@TenantId AND \"CompanyId\"=@CompanyId";
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(ct);
        var row = await connection.QueryFirstOrDefaultAsync<FgsSetupPricingMatrixMaterialTierRow>(new CommandDefinition(sql, new { Id=id, TenantId=tenantId, CompanyId=companyId }, cancellationToken:ct));
        return row?.ToDetailDto();
    }

    public async Task<PagedResult<FgsSetupPricingMatrixMaterialTierSummaryDto>> ListAsync(SetupListQuery query, FgsSetupPricingMatrixMaterialTierListFilters filters, CancellationToken ct = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var paging=query.ToPagedQuery(); var page=Math.Max(1,paging.Page); var pageSize=Math.Clamp(paging.PageSize,1,200); var offset=(page-1)*pageSize;
        var where=new List<string>{"\"TenantId\"=@TenantId", "\"CompanyId\"=@CompanyId"};
        if (paging.IsActive.HasValue) where.Add("\"IsActive\"=@IsActive");
        if (filters.PricingMatrixId.HasValue) where.Add("\"PricingMatrixId\" = @PricingMatrixId");

        var clause=string.Join(" AND ",where); var order=FgsSetupPricingMatrixMaterialTierSql.ResolveOrderBy(paging.SortBy,paging.SortDirection);
        var sql=$"SELECT {FgsSetupPricingMatrixMaterialTierSql.DetailColumns} FROM {FgsSetupPricingMatrixMaterialTierSql.Table} WHERE {clause} {order} LIMIT @PageSize OFFSET @Offset; SELECT COUNT(*) FROM {FgsSetupPricingMatrixMaterialTierSql.Table} WHERE {clause};";
        var args=new { TenantId=tenantId, CompanyId=companyId, IsActive=paging.IsActive, PricingMatrixId = filters.PricingMatrixId, Search=string.IsNullOrWhiteSpace(paging.Search)?null:$"%{paging.Search.Trim()}%", PageSize=pageSize, Offset=offset };
        await using var connection=await _connectionFactory.CreateOpenConnectionAsync(ct);
        await using var multi=await connection.QueryMultipleAsync(new CommandDefinition(sql,args,cancellationToken:ct));
        var rows=(await multi.ReadAsync<FgsSetupPricingMatrixMaterialTierRow>()).Select(x=>x.ToSummaryDto()).ToList(); var count=await multi.ReadSingleAsync<int>();
        return new PagedResult<FgsSetupPricingMatrixMaterialTierSummaryDto>(rows,page,pageSize,count);
    }

    public async Task<IReadOnlyList<FgsSetupPricingMatrixMaterialTierLookupDto>> LookupAsync(bool activeOnly=true, long? pricingMatrixId=null, CancellationToken ct=default)
    {
        var (tenantId, companyId)=SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql=$"SELECT {FgsSetupPricingMatrixMaterialTierSql.LookupColumns} FROM {FgsSetupPricingMatrixMaterialTierSql.Table} WHERE \"TenantId\"=@TenantId AND \"CompanyId\"=@CompanyId {(activeOnly?"AND \"IsActive\"=TRUE":"")} {(pricingMatrixId.HasValue?"AND \"PricingMatrixId\"=@ParentId":"")} ORDER BY \"PricingMatrixId\", \"FromCost\", \"Id\"";
        await using var connection=await _connectionFactory.CreateOpenConnectionAsync(ct);
        var rows=await connection.QueryAsync<FgsSetupPricingMatrixMaterialTierLookupRow>(new CommandDefinition(sql,new {TenantId=tenantId,CompanyId=companyId,ParentId=pricingMatrixId},cancellationToken:ct));
        return rows.Select(x=>x.ToDto()).ToList();
    }

    public Task<bool> ExistsPricingMatrixIdAsync(long id, CancellationToken ct = default) => ParentExistsAsync("FgsSetupPricingMatrix", id, ct);
    public Task<bool> ExistsByFromCostAsync(long matrixId, decimal fromCost, long? excludeId = null, CancellationToken ct = default) =>
        ExistsAsync("\"PricingMatrixId\"=@ParentId AND \"FromCost\"=@Value AND \"IsActive\"=TRUE" + (excludeId.HasValue ? " AND \"Id\"<>@ExcludeId" : ""), new { ParentId=matrixId, Value=fromCost, ExcludeId=excludeId }, ct);
    public Task<bool> ExistsActiveOtherItemsForMatrixAsync(long matrixId, CancellationToken ct = default) =>
        ExistsInTableAsync("FgsSetupPricingMatrixOther", matrixId, ct);

    private async Task<bool> ParentExistsAsync(string table, long id, CancellationToken ct)
    {
        var (tenantId,companyId)=SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql=$"SELECT EXISTS(SELECT 1 FROM setup.\"{table}\" WHERE \"TenantId\"=@TenantId AND \"CompanyId\"=@CompanyId AND \"Id\"=@Id AND \"IsActive\"=TRUE)";
        await using var connection=await _connectionFactory.CreateOpenConnectionAsync(ct);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql,new {TenantId=tenantId,CompanyId=companyId,Id=id},cancellationToken:ct));
    }
    private async Task<bool> ExistsAsync(string predicate, object args, CancellationToken ct)
    {
        var (tenantId,companyId)=SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql=$"SELECT EXISTS(SELECT 1 FROM {FgsSetupPricingMatrixMaterialTierSql.Table} WHERE \"TenantId\"=@TenantId AND \"CompanyId\"=@CompanyId AND {predicate})";
        var parameters=new DynamicParameters(args); parameters.Add("TenantId",tenantId); parameters.Add("CompanyId",companyId);
        await using var connection=await _connectionFactory.CreateOpenConnectionAsync(ct);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql,parameters,cancellationToken:ct));
    }
    private async Task<bool> ExistsInTableAsync(string table,long matrixId,CancellationToken ct)
    {
        var (tenantId,companyId)=SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql=$"SELECT EXISTS(SELECT 1 FROM setup.\"{table}\" WHERE \"TenantId\"=@TenantId AND \"CompanyId\"=@CompanyId AND \"PricingMatrixId\"=@MatrixId AND \"IsActive\"=TRUE)";
        await using var connection=await _connectionFactory.CreateOpenConnectionAsync(ct);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql,new {TenantId=tenantId,CompanyId=companyId,MatrixId=matrixId},cancellationToken:ct));
    }
}
