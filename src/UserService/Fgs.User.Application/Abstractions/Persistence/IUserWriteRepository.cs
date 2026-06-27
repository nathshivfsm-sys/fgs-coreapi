using Fgs.Persistence.Abstractions;

namespace Fgs.User.Application.Abstractions.Persistence;

public interface IUserWriteRepository<TEntity> : IRepository<TEntity> where TEntity : class;
