using Dapper;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Persistence;
using Fgs.Setup.Application.Abstractions.SetupPricingMatrixLaborTiers;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.SetupPricingMatrixLaborTiers.Dtos;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.SetupPricingMatrixLaborTiers;

internal sealed class FgsSetupPricingMatrixLaborTierReadRepository(ISetupReadConnectionFactory _connectionFactory, ITenantContextAccessor _tenantContextAccessor) : IFgsSetupPricingMatrixLaborTierReadRepository
{
    public async Task<FgsSetupPricingMatrixLaborTierDetailDto?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"SELECT {FgsSetupPricingMatrixLaborTierSql.DetailColumns} FROM {FgsSetupPricingMatrixLaborTierSql.Table} WHERE \"Id\"=@Id AND \"TenantId\"=@TenantId AND \"CompanyId\"=@CompanyId";
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(ct);
        var row = await connection.QueryFirstOrDefaultAsync<FgsSetupPricingMatrixLaborTierRow>(new CommandDefinition(sql, new { Id=id, TenantId=tenantId, CompanyId=companyId }, cancellationToken:ct));
        return row?.ToDetailDto();
    }

    public async Task<PagedResult<FgsSetupPricingMatrixLaborTierSummaryDto>> ListAsync(SetupListQuery query, FgsSetupPricingMatrixLaborTierListFilters filters, CancellationToken ct = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var paging=query.ToPagedQuery(); var page=Math.Max(1,paging.Page); var pageSize=Math.Clamp(paging.PageSize,1,200); var offset=(page-1)*pageSize;
        var where=new List<string>{"\"TenantId\"=@TenantId", "\"CompanyId\"=@CompanyId"};
        if (paging.IsActive.HasValue) where.Add("\"IsActive\"=@IsActive");
        if (filters.PricingMatrixLaborId.HasValue) where.Add("\"PricingMatrixLaborId\" = @PricingMatrixLaborId");

        var clause=string.Join(" AND ",where); var order=FgsSetupPricingMatrixLaborTierSql.ResolveOrderBy(paging.SortBy,paging.SortDirection);
        var sql=$"SELECT {FgsSetupPricingMatrixLaborTierSql.DetailColumns} FROM {FgsSetupPricingMatrixLaborTierSql.Table} WHERE {clause} {order} LIMIT @PageSize OFFSET @Offset; SELECT COUNT(*) FROM {FgsSetupPricingMatrixLaborTierSql.Table} WHERE {clause};";
        var args=new { TenantId=tenantId, CompanyId=companyId, IsActive=paging.IsActive, PricingMatrixLaborId = filters.PricingMatrixLaborId, Search=string.IsNullOrWhiteSpace(paging.Search)?null:$"%{paging.Search.Trim()}%", PageSize=pageSize, Offset=offset };
        await using var connection=await _connectionFactory.CreateOpenConnectionAsync(ct);
        await using var multi=await connection.QueryMultipleAsync(new CommandDefinition(sql,args,cancellationToken:ct));
        var rows=(await multi.ReadAsync<FgsSetupPricingMatrixLaborTierRow>()).Select(x=>x.ToSummaryDto()).ToList(); var count=await multi.ReadSingleAsync<int>();
        return new PagedResult<FgsSetupPricingMatrixLaborTierSummaryDto>(rows,page,pageSize,count);
    }

    public async Task<IReadOnlyList<FgsSetupPricingMatrixLaborTierLookupDto>> LookupAsync(bool activeOnly=true, long? pricingMatrixLaborId=null, CancellationToken ct=default)
    {
        var (tenantId, companyId)=SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql=$"SELECT {FgsSetupPricingMatrixLaborTierSql.LookupColumns} FROM {FgsSetupPricingMatrixLaborTierSql.Table} WHERE \"TenantId\"=@TenantId AND \"CompanyId\"=@CompanyId {(activeOnly?"AND \"IsActive\"=TRUE":"")} {(pricingMatrixLaborId.HasValue?"AND \"PricingMatrixLaborId\"=@ParentId":"")} ORDER BY \"PricingMatrixLaborId\", \"SequenceOrder\", \"Id\"";
        await using var connection=await _connectionFactory.CreateOpenConnectionAsync(ct);
        var rows=await connection.QueryAsync<FgsSetupPricingMatrixLaborTierLookupRow>(new CommandDefinition(sql,new {TenantId=tenantId,CompanyId=companyId,ParentId=pricingMatrixLaborId},cancellationToken:ct));
        return rows.Select(x=>x.ToDto()).ToList();
    }

    public Task<bool> ExistsPricingMatrixLaborIdAsync(long id, CancellationToken ct = default) => ParentExistsAsync("FgsSetupPricingMatrixLabor", id, ct);
    public Task<bool> ExistsBySequenceOrderAsync(long laborId, short sequenceOrder, long? excludeId = null, CancellationToken ct = default) =>
        ExistsAsync("\"PricingMatrixLaborId\"=@ParentId AND \"SequenceOrder\"=@Value" + (excludeId.HasValue ? " AND \"Id\"<>@ExcludeId" : ""), new { ParentId=laborId, Value=sequenceOrder, ExcludeId=excludeId }, ct);

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
        var sql=$"SELECT EXISTS(SELECT 1 FROM {FgsSetupPricingMatrixLaborTierSql.Table} WHERE \"TenantId\"=@TenantId AND \"CompanyId\"=@CompanyId AND {predicate})";
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
