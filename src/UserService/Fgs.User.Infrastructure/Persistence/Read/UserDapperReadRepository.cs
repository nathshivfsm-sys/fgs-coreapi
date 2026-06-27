using Dapper;
using Fgs.User.Application.Abstractions.Persistence;

namespace Fgs.User.Infrastructure.Persistence.Read;

internal sealed class UserDapperReadRepository<TEntity> : IUserReadRepository<TEntity>
    where TEntity : class
{
    private readonly IUserReadConnectionFactory _connectionFactory;
    private readonly UserEntityReadDescriptor _descriptor;

    public UserDapperReadRepository(IUserReadConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
        _descriptor = UserEntityReadRegistry.GetDescriptor<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        if (_descriptor.IdKind != UserEntityIdKind.Long)
        {
            throw new InvalidOperationException(
                $"Entity '{typeof(TEntity).Name}' does not use a long identifier.");
        }

        return await FirstOrDefaultAsync("\"Id\" = @id", new { id }, cancellationToken);
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (_descriptor.IdKind != UserEntityIdKind.Guid)
        {
            throw new InvalidOperationException(
                $"Entity '{typeof(TEntity).Name}' does not use a Guid identifier.");
        }

        return await FirstOrDefaultAsync("\"Id\" = @id", new { id }, cancellationToken);
    }

    public async Task<TEntity?> FirstOrDefaultAsync(
        string whereClause,
        object parameters,
        CancellationToken cancellationToken = default)
    {
        var sql = $"SELECT * FROM {_descriptor.Table} WHERE {whereClause} LIMIT 1";
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.QueryFirstOrDefaultAsync<TEntity>(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));
    }

    public async Task<IReadOnlyList<TEntity>> ListAsync(
        string whereClause,
        object parameters,
        CancellationToken cancellationToken = default)
    {
        var sql = $"SELECT * FROM {_descriptor.Table} WHERE {whereClause}";
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<TEntity>(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));
        return rows.ToList();
    }

    public async Task<bool> AnyAsync(
        string whereClause,
        object parameters,
        CancellationToken cancellationToken = default)
    {
        var sql = $"SELECT EXISTS(SELECT 1 FROM {_descriptor.Table} WHERE {whereClause})";
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));
    }

    public async Task<T?> QueryFirstAsync<T>(
        string sql,
        object? parameters,
        CancellationToken cancellationToken = default)
    {
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.QueryFirstOrDefaultAsync<T>(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));
    }

    public async Task<IReadOnlyList<T>> QueryListAsync<T>(
        string sql,
        object? parameters,
        CancellationToken cancellationToken = default)
    {
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<T>(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));
        return rows.ToList();
    }
}
