using Fgs.Persistence.Abstractions;
using Fgs.Setup.Infrastructure.Database;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Infrastructure.Database.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Fgs.User.Infrastructure.Database.UnitOfWorks;

public sealed class SetupUnitOfWork : ISetupUnitOfWork
{
    private readonly FgsSetupDbContext _context;
    private readonly Dictionary<Type, object> _repositories = new();

    public SetupUnitOfWork(FgsSetupDbContext context) => _context = context;

    public IRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        var type = typeof(TEntity);
        if (_repositories.TryGetValue(type, out var repo))
        {
            return (IRepository<TEntity>)repo;
        }

        var instance = new SetupRepository<TEntity>(_context);
        _repositories[type] = instance;
        return instance;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);

    public Task<T> ExecuteInTransactionAsync<T>(
        Func<CancellationToken, Task<T>> operation,
        CancellationToken cancellationToken = default)
    {
        var strategy = _context.Database.CreateExecutionStrategy();
        return strategy.ExecuteAsync(
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
