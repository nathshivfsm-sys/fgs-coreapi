namespace UserService.Application.Common.Persistence;

public interface IUnitOfWork
{
    IRepository<TEntity> Repository<TEntity>()
        where TEntity : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}
