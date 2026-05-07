using UserService.Application.Common.Persistence;

namespace UserService.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly UserServiceDbContext _db;

    public UnitOfWork(UserServiceDbContext db) => _db = db;

    public IRepository<TEntity> Repository<TEntity>()
        where TEntity : class =>
        new Repository<TEntity>(_db);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _db.SaveChangesAsync(cancellationToken);

    public async Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        var tx = await _db.Database.BeginTransactionAsync(cancellationToken);
        return new EfTransaction(tx);
    }
}
