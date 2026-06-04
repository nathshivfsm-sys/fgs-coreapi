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
        _dbSet.FindAsync([id], cancellationToken).AsTask();

    public Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _dbSet.FindAsync([id], cancellationToken).AsTask();

    public async Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) =>
        await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);

    public async Task<TEntity?> FirstOrDefaultIgnoreFiltersAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) =>
        await _dbSet.IgnoreQueryFilters().FirstOrDefaultAsync(predicate, cancellationToken);

    public Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) =>
        _dbSet.AnyAsync(predicate, cancellationToken);

    public async Task<IReadOnlyList<TEntity>> ListAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) =>
        await _dbSet.Where(predicate).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<TEntity>> ListIgnoreFiltersAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) =>
        await _dbSet.IgnoreQueryFilters().Where(predicate).ToListAsync(cancellationToken);

    public Task AddAsync(TEntity entity, CancellationToken cancellationToken = default) =>
        _dbSet.AddAsync(entity, cancellationToken).AsTask();

    public void Update(TEntity entity) => _dbSet.Update(entity);

    public void Remove(TEntity entity) => _dbSet.Remove(entity);
}
