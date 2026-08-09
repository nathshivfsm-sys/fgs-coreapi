using Dapper;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Persistence;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrixLabors;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixLabors.Dtos;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.SetupPricingMatrixLabors;

internal sealed class FgsSetupPricingMatrixLaborReadRepository(ISetupReadConnectionFactory _connectionFactory, ITenantContextAccessor _tenantContextAccessor) : IFgsSetupPricingMatrixLaborReadRepository
{
    public async Task<FgsSetupPricingMatrixLaborDetailDto?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"SELECT {FgsSetupPricingMatrixLaborSql.DetailColumns} FROM {FgsSetupPricingMatrixLaborSql.Table} WHERE \"Id\"=@Id AND \"TenantId\"=@TenantId AND \"CompanyId\"=@CompanyId";
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(ct);
        var row = await connection.QueryFirstOrDefaultAsync<FgsSetupPricingMatrixLaborRow>(new CommandDefinition(sql, new { Id=id, TenantId=tenantId, CompanyId=companyId }, cancellationToken:ct));
        return row?.ToDetailDto();
    }

    public async Task<PagedResult<FgsSetupPricingMatrixLaborSummaryDto>> ListAsync(SetupListQuery query, FgsSetupPricingMatrixLaborListFilters filters, CancellationToken ct = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var paging=query.ToPagedQuery(); var page=Math.Max(1,paging.Page); var pageSize=Math.Clamp(paging.PageSize,1,200); var offset=(page-1)*pageSize;
        var where=new List<string>{"\"TenantId\"=@TenantId", "\"CompanyId\"=@CompanyId"};
        if (paging.IsActive.HasValue) where.Add("\"IsActive\"=@IsActive");
        if (filters.PricingMatrixId.HasValue) where.Add("\"PricingMatrixId\" = @PricingMatrixId");

        var clause=string.Join(" AND ",where); var order=FgsSetupPricingMatrixLaborSql.ResolveOrderBy(paging.SortBy,paging.SortDirection);
        var sql=$"SELECT {FgsSetupPricingMatrixLaborSql.DetailColumns} FROM {FgsSetupPricingMatrixLaborSql.Table} WHERE {clause} {order} LIMIT @PageSize OFFSET @Offset; SELECT COUNT(*) FROM {FgsSetupPricingMatrixLaborSql.Table} WHERE {clause};";
        var args=new { TenantId=tenantId, CompanyId=companyId, IsActive=paging.IsActive, PricingMatrixId = filters.PricingMatrixId, Search=string.IsNullOrWhiteSpace(paging.Search)?null:$"%{paging.Search.Trim()}%", PageSize=pageSize, Offset=offset };
        await using var connection=await _connectionFactory.CreateOpenConnectionAsync(ct);
        await using var multi=await connection.QueryMultipleAsync(new CommandDefinition(sql,args,cancellationToken:ct));
        var rows=(await multi.ReadAsync<FgsSetupPricingMatrixLaborRow>()).Select(x=>x.ToSummaryDto()).ToList(); var count=await multi.ReadSingleAsync<int>();
        return new PagedResult<FgsSetupPricingMatrixLaborSummaryDto>(rows,page,pageSize,count);
    }

    public async Task<IReadOnlyList<FgsSetupPricingMatrixLaborLookupDto>> LookupAsync(bool activeOnly=true, long? pricingMatrixId=null, CancellationToken ct=default)
    {
        var (tenantId, companyId)=SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql=$"SELECT {FgsSetupPricingMatrixLaborSql.LookupColumns} FROM {FgsSetupPricingMatrixLaborSql.Table} WHERE \"TenantId\"=@TenantId AND \"CompanyId\"=@CompanyId {(activeOnly?"AND \"IsActive\"=TRUE":"")} {(pricingMatrixId.HasValue?"AND \"PricingMatrixId\"=@ParentId":"")} ORDER BY \"PricingMatrixId\", \"LaborRateTypeId\", \"Id\"";
        await using var connection=await _connectionFactory.CreateOpenConnectionAsync(ct);
        var rows=await connection.QueryAsync<FgsSetupPricingMatrixLaborLookupRow>(new CommandDefinition(sql,new {TenantId=tenantId,CompanyId=companyId,ParentId=pricingMatrixId},cancellationToken:ct));
        return rows.Select(x=>x.ToDto()).ToList();
    }

    public Task<bool> ExistsPricingMatrixIdAsync(long id, CancellationToken ct = default) => ParentExistsAsync("FgsSetupPricingMatrix", id, ct);
    public async Task<bool> ExistsByIdAsync(long id, CancellationToken ct = default) => await ExistsAsync("\"Id\" = @Id", new { Id = id }, ct);
    public async Task<long?> GetPricingMatrixIdAsync(long laborId, bool activeOnly = true, CancellationToken ct = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"SELECT \"PricingMatrixId\" FROM {FgsSetupPricingMatrixLaborSql.Table} WHERE \"TenantId\"=@TenantId AND \"CompanyId\"=@CompanyId AND \"Id\"=@Id {(activeOnly ? "AND \"IsActive\"=TRUE" : "")}";
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(ct);
        return await connection.QueryFirstOrDefaultAsync<long?>(new CommandDefinition(sql, new { TenantId=tenantId, CompanyId=companyId, Id=laborId }, cancellationToken:ct));
    }

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
        var sql=$"SELECT EXISTS(SELECT 1 FROM {FgsSetupPricingMatrixLaborSql.Table} WHERE \"TenantId\"=@TenantId AND \"CompanyId\"=@CompanyId AND {predicate})";
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
