using System.Linq.Expressions;
using Fgs.Persistence.Abstractions;
using Fgs.Persistence.Implementations;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Infrastructure.Database;

namespace Fgs.User.Infrastructure.Persistence.Write;

internal sealed class UserEfWriteRepository<TEntity> : IUserWriteRepository<TEntity>
    where TEntity : class
{
    private readonly EfRepository<TEntity, FgsUserDbContext> _inner;

    public UserEfWriteRepository(FgsUserDbContext context) =>
        _inner = new EfRepository<TEntity, FgsUserDbContext>(context);

    public Task<TEntity?> GetByIdAsync(long id, CancellationToken cancellationToken = default) =>
        _inner.GetByIdAsync(id, cancellationToken);

    public Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _inner.GetByIdAsync(id, cancellationToken);

    public Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) =>
        _inner.FirstOrDefaultAsync(predicate, cancellationToken);

    public Task<TEntity?> FirstOrDefaultIgnoreFiltersAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) =>
        _inner.FirstOrDefaultIgnoreFiltersAsync(predicate, cancellationToken);

    public Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) =>
        _inner.AnyAsync(predicate, cancellationToken);

    public Task<IReadOnlyList<TEntity>> ListAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) =>
        _inner.ListAsync(predicate, cancellationToken);

    public Task<IReadOnlyList<TEntity>> ListIgnoreFiltersAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) =>
        _inner.ListIgnoreFiltersAsync(predicate, cancellationToken);

    public Task AddAsync(TEntity entity, CancellationToken cancellationToken = default) =>
        _inner.AddAsync(entity, cancellationToken);

    public void Update(TEntity entity) => _inner.Update(entity);

    public void Remove(TEntity entity) => _inner.Remove(entity);
}
