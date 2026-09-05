using System.Linq.Expressions;
using Fgs.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Persistence.Implementations;

public sealed class EfRepository<TEntity, TDbContext> : IRepository<TEntity>
    where TEntity : class
    where TDbContext : DbContext
{
    private readonly TDbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public EfRepository(TDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public Task<TEntity?> GetByIdAsync(long id, CancellationToken cancellationToken = default) =>
        _dbSet.AsNoTracking().FirstOrDefaultAsync(CreateIdPredicate<long>(id), cancellationToken);

    public Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _dbSet.AsNoTracking().FirstOrDefaultAsync(CreateIdPredicate<Guid>(id), cancellationToken);

    public async Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) =>
        await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate, cancellationToken);

    public async Task<TEntity?> FirstOrDefaultIgnoreFiltersAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) =>
        await _dbSet.IgnoreQueryFilters().AsNoTracking().FirstOrDefaultAsync(predicate, cancellationToken);

    public Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) =>
        _dbSet.AsNoTracking().AnyAsync(predicate, cancellationToken);

    public async Task<IReadOnlyList<TEntity>> ListAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) =>
        await _dbSet.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<TEntity>> ListIgnoreFiltersAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) =>
        await _dbSet.IgnoreQueryFilters().AsNoTracking().Where(predicate).ToListAsync(cancellationToken);

    public Task AddAsync(TEntity entity, CancellationToken cancellationToken = default) =>
        _dbSet.AddAsync(entity, cancellationToken).AsTask();

    public void Update(TEntity entity)
    {
        var entry = _context.Entry(entity);
        if (entry.State != EntityState.Detached)
        {
            return;
        }

        var keyValues = _context.Model.FindEntityType(typeof(TEntity))
            ?.FindPrimaryKey()
            ?.Properties
            .Select(p => typeof(TEntity).GetProperty(p.Name)!.GetValue(entity))
            .ToArray();

        if (keyValues is { Length: > 0 })
        {
            var local = _dbSet.Local.FirstOrDefault(e =>
            {
                var localKeys = _context.Model.FindEntityType(typeof(TEntity))!
                    .FindPrimaryKey()!
                    .Properties
                    .Select(p => typeof(TEntity).GetProperty(p.Name)!.GetValue(e))
                    .ToArray();
                return keyValues.SequenceEqual(localKeys);
            });

            if (local is not null)
            {
                _context.Entry(local).CurrentValues.SetValues(entity);
                return;
            }
        }

        _dbSet.Update(entity);
    }

    public void Remove(TEntity entity) => _dbSet.Remove(entity);

    private static Expression<Func<TEntity, bool>> CreateIdPredicate<TId>(TId id)
    {
        var parameter = Expression.Parameter(typeof(TEntity), "entity");
        var property = Expression.Property(parameter, "Id");
        var constant = Expression.Constant(id, typeof(TId));
        var equality = Expression.Equal(property, constant);
        return Expression.Lambda<Func<TEntity, bool>>(equality, parameter);
    }
}
