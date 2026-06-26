using Dapper;
using Fgs.Foundation.CatalogCrud;
using Fgs.Foundation.CatalogCrud.Abstractions;
using Fgs.MultiTenancy;

namespace Fgs.Persistence.CatalogCrud;

public sealed class CatalogEntityReadRepository : IEntityReadRepository
{
    private readonly ICatalogReadConnectionFactory _connectionFactory;
    private readonly ITenantContextAccessor _tenantContextAccessor;

    public CatalogEntityReadRepository(
        ICatalogReadConnectionFactory connectionFactory,
        ITenantContextAccessor tenantContextAccessor)
    {
        _connectionFactory = connectionFactory;
        _tenantContextAccessor = tenantContextAccessor;
    }

    public async Task<object?> GetByIdAsync(
        CatalogEntityDescriptor descriptor,
        string id,
        CancellationToken cancellationToken = default)
    {
        var parsedId = CatalogEntityMapper.ParseId(id, descriptor.KeyType)
            ?? throw new FormatException($"Invalid identifier '{id}'.");

        var (tenantId, companyId) = ResolveTenantScope();
        var (sql, parameters) = CatalogSqlBuilder.BuildGetById(descriptor, parsedId, tenantId, companyId);

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var row = await connection.QueryFirstOrDefaultAsync<dynamic>(new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));
        if (row is null)
        {
            return null;
        }

        var dictionary = (IDictionary<string, object?>)row;
        return CatalogEntityMapper.MapRowToDto(dictionary, descriptor.DetailDtoType);
    }

    public async Task<PagedResult<object>> ListAsync(
        CatalogEntityDescriptor descriptor,
        PagedQuery paging,
        IReadOnlyDictionary<string, string?> filters,
        Type summaryDtoType,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = ResolveTenantScope();
        var (sql, parameters) = CatalogSqlBuilder.BuildList(descriptor, paging, filters, tenantId, companyId);

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

        var rows = (await multi.ReadAsync<dynamic>()).ToList();
        var totalCount = await multi.ReadSingleAsync<int>();

        var items = rows
            .Select(row => CatalogEntityMapper.MapRowToDto((IDictionary<string, object?>)row, summaryDtoType))
            .ToList();

        return new PagedResult<object>(items, paging.Page, paging.PageSize, totalCount);
    }

    public async Task<bool> ExistsAsync(
        CatalogEntityDescriptor descriptor,
        IReadOnlyDictionary<string, object?> propertyValues,
        string? excludeId,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = ResolveTenantScope();
        object? parsedExcludeId = excludeId is null
            ? null
            : CatalogEntityMapper.ParseId(excludeId, descriptor.KeyType);

        var (sql, parameters) = CatalogSqlBuilder.BuildExists(
            descriptor,
            propertyValues,
            parsedExcludeId,
            tenantId,
            companyId);

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));
    }

    private (long? TenantId, long? CompanyId) ResolveTenantScope()
    {
        if (_tenantContextAccessor.Current is ITenantContext context)
        {
            return (context.TenantId, context.CompanyId);
        }

        return (null, null);
    }
}
