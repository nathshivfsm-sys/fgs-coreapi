using Microsoft.EntityFrameworkCore.Storage;
using UserService.Application.Common.Persistence;

namespace UserService.Infrastructure.Persistence;

public sealed class EfTransaction : ITransaction
{
    private readonly IDbContextTransaction _inner;

    public EfTransaction(IDbContextTransaction inner) => _inner = inner;

    public Task CommitAsync(CancellationToken cancellationToken = default) =>
        _inner.CommitAsync(cancellationToken);

    public Task RollbackAsync(CancellationToken cancellationToken = default) =>
        _inner.RollbackAsync(cancellationToken);

    public ValueTask DisposeAsync() => _inner.DisposeAsync();
}
