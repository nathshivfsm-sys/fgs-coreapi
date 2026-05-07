using Microsoft.EntityFrameworkCore;
using UserService.Application.Common.Persistence;

namespace UserService.Infrastructure.Persistence;

internal sealed class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    private readonly DbContext _context;

    public Repository(DbContext context) => _context = context;

    private DbSet<TEntity> Set => _context.Set<TEntity>();

    public void Add(TEntity entity) => Set.Add(entity);

    public void AddRange(IEnumerable<TEntity> entities) => Set.AddRange(entities);

    public void Update(TEntity entity) => Set.Update(entity);

    public void Remove(TEntity entity) => Set.Remove(entity);

    public ValueTask<TEntity?> FindAsync(CancellationToken cancellationToken, params object[] keyValues) =>
        Set.FindAsync(keyValues, cancellationToken);
}
