using Dapper;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Persistence;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrixOthers;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixOthers.Dtos;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.SetupPricingMatrixOthers;

internal sealed class FgsSetupPricingMatrixOtherReadRepository(ISetupReadConnectionFactory _connectionFactory, ITenantContextAccessor _tenantContextAccessor) : IFgsSetupPricingMatrixOtherReadRepository
{
    public async Task<FgsSetupPricingMatrixOtherDetailDto?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"SELECT {FgsSetupPricingMatrixOtherSql.DetailColumns} FROM {FgsSetupPricingMatrixOtherSql.Table} WHERE \"Id\"=@Id AND \"TenantId\"=@TenantId AND \"CompanyId\"=@CompanyId";
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(ct);
        var row = await connection.QueryFirstOrDefaultAsync<FgsSetupPricingMatrixOtherRow>(new CommandDefinition(sql, new { Id=id, TenantId=tenantId, CompanyId=companyId }, cancellationToken:ct));
        return row?.ToDetailDto();
    }

    public async Task<PagedResult<FgsSetupPricingMatrixOtherSummaryDto>> ListAsync(SetupListQuery query, FgsSetupPricingMatrixOtherListFilters filters, CancellationToken ct = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var paging=query.ToPagedQuery(); var page=Math.Max(1,paging.Page); var pageSize=Math.Clamp(paging.PageSize,1,200); var offset=(page-1)*pageSize;
        var where=new List<string>{"\"TenantId\"=@TenantId", "\"CompanyId\"=@CompanyId"};
        if (paging.IsActive.HasValue) where.Add("\"IsActive\"=@IsActive");
        if (filters.PricingMatrixId.HasValue) where.Add("\"PricingMatrixId\" = @PricingMatrixId");
        if (!string.IsNullOrWhiteSpace(filters.CategoryCode)) where.Add("\"CategoryCode\" = @CategoryCode");
        if (!string.IsNullOrWhiteSpace(paging.Search)) where.Add("(\"CategoryCode\" ILIKE @Search OR \"Name\" ILIKE @Search)");
        var clause=string.Join(" AND ",where); var order=FgsSetupPricingMatrixOtherSql.ResolveOrderBy(paging.SortBy,paging.SortDirection);
        var sql=$"SELECT {FgsSetupPricingMatrixOtherSql.DetailColumns} FROM {FgsSetupPricingMatrixOtherSql.Table} WHERE {clause} {order} LIMIT @PageSize OFFSET @Offset; SELECT COUNT(*) FROM {FgsSetupPricingMatrixOtherSql.Table} WHERE {clause};";
        var args=new { TenantId=tenantId, CompanyId=companyId, IsActive=paging.IsActive, PricingMatrixId = filters.PricingMatrixId, CategoryCode = string.IsNullOrWhiteSpace(filters.CategoryCode) ? null : filters.CategoryCode.Trim().ToUpperInvariant(), Search=string.IsNullOrWhiteSpace(paging.Search)?null:$"%{paging.Search.Trim()}%", PageSize=pageSize, Offset=offset };
        await using var connection=await _connectionFactory.CreateOpenConnectionAsync(ct);
        await using var multi=await connection.QueryMultipleAsync(new CommandDefinition(sql,args,cancellationToken:ct));
        var rows=(await multi.ReadAsync<FgsSetupPricingMatrixOtherRow>()).Select(x=>x.ToSummaryDto()).ToList(); var count=await multi.ReadSingleAsync<int>();
        return new PagedResult<FgsSetupPricingMatrixOtherSummaryDto>(rows,page,pageSize,count);
    }

    public async Task<IReadOnlyList<FgsSetupPricingMatrixOtherLookupDto>> LookupAsync(bool activeOnly=true, long? pricingMatrixId=null, CancellationToken ct=default)
    {
        var (tenantId, companyId)=SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql=$"SELECT {FgsSetupPricingMatrixOtherSql.LookupColumns} FROM {FgsSetupPricingMatrixOtherSql.Table} WHERE \"TenantId\"=@TenantId AND \"CompanyId\"=@CompanyId {(activeOnly?"AND \"IsActive\"=TRUE":"")} {(pricingMatrixId.HasValue?"AND \"PricingMatrixId\"=@ParentId":"")} ORDER BY \"PricingMatrixId\", \"CategoryCode\", \"Id\"";
        await using var connection=await _connectionFactory.CreateOpenConnectionAsync(ct);
        var rows=await connection.QueryAsync<FgsSetupPricingMatrixOtherLookupRow>(new CommandDefinition(sql,new {TenantId=tenantId,CompanyId=companyId,ParentId=pricingMatrixId},cancellationToken:ct));
        return rows.Select(x=>x.ToDto()).ToList();
    }

    public Task<bool> ExistsPricingMatrixIdAsync(long id, CancellationToken ct = default) => ParentExistsAsync("FgsSetupPricingMatrix", id, ct);
    public Task<bool> ExistsByCategoryCodeAsync(long matrixId, string categoryCode, long? excludeId = null, CancellationToken ct = default) =>
        ExistsAsync("\"PricingMatrixId\"=@ParentId AND \"CategoryCode\"=@Value AND \"IsActive\"=TRUE" + (excludeId.HasValue ? " AND \"Id\"<>@ExcludeId" : ""), new { ParentId=matrixId, Value=categoryCode.Trim().ToUpperInvariant(), ExcludeId=excludeId }, ct);
    public Task<bool> ExistsActiveMaterialTiersForMatrixAsync(long matrixId, CancellationToken ct = default) =>
        ExistsInTableAsync("FgsSetupPricingMatrixMaterialTier", matrixId, ct);

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
        var sql=$"SELECT EXISTS(SELECT 1 FROM {FgsSetupPricingMatrixOtherSql.Table} WHERE \"TenantId\"=@TenantId AND \"CompanyId\"=@CompanyId AND {predicate})";
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
