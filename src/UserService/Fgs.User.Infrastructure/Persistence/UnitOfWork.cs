using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

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

    public async Task<T> ExecuteInTransactionAsync<T>(
        Func<CancellationToken, Task<T>> operation,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var result = await operation(cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return result;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
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
