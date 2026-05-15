using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.User.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly FgsUserDbContext _context;
    private readonly Dictionary<Type, object> _repositories = new();

    public UnitOfWork(FgsUserDbContext context) => _context = context;

    public IRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        var type = typeof(TEntity);
        if (_repositories.TryGetValue(type, out var repo))
        {
            return (IRepository<TEntity>)repo;
        }

        var instance = new Repository<TEntity>(_context);
        _repositories[type] = instance;
        return instance;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);

    public Task<T> ExecuteInTransactionAsync<T>(
        Func<CancellationToken, Task<T>> operation,
        CancellationToken cancellationToken = default)
    {
        // Required when UseNpgsql(... EnableRetryOnFailure): user transactions must run inside the execution strategy.
        var strategy = _context.Database.CreateExecutionStrategy();
        return strategy.ExecuteAsync<T>(
            async ct =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync(ct);
                try
                {
                    var result = await operation(ct);
                    await _context.SaveChangesAsync(ct);
                    await transaction.CommitAsync(ct);
                    return result;
                }
                catch
                {
                    await transaction.RollbackAsync(ct);
                    throw;
                }
            },
            cancellationToken);
    }

    public async Task ExecuteInTransactionAsync(
        Func<CancellationToken, Task> operation,
        CancellationToken cancellationToken = default)
    {
        await ExecuteInTransactionAsync<object?>(
            async ct =>
            {
                await operation(ct);
                return null;
            },
            cancellationToken);
    }
}
