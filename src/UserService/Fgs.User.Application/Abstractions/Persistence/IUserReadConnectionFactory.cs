using System.Data.Common;

namespace Fgs.User.Application.Abstractions.Persistence;

public interface IUserReadConnectionFactory
{
    Task<DbConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken = default);
}
