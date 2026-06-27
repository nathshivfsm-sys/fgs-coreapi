namespace Fgs.User.Application.Abstractions.Persistence;

public interface IUserReadRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<TEntity?> FirstOrDefaultAsync(
        string whereClause,
        object parameters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TEntity>> ListAsync(
        string whereClause,
        object parameters,
        CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(
        string whereClause,
        object parameters,
        CancellationToken cancellationToken = default);

    Task<T?> QueryFirstAsync<T>(
        string sql,
        object? parameters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<T>> QueryListAsync<T>(
        string sql,
        object? parameters,
        CancellationToken cancellationToken = default);
}
