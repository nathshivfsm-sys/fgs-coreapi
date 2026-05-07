namespace UserService.Application.Common.Persistence;

public interface IRepository<TEntity>
    where TEntity : class
{
    void Add(TEntity entity);
    void AddRange(IEnumerable<TEntity> entities);
    void Update(TEntity entity);
    void Remove(TEntity entity);
    ValueTask<TEntity?> FindAsync(CancellationToken cancellationToken, params object[] keyValues);
}
