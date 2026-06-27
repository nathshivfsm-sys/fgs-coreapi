using System.Data.Common;

namespace Fgs.Setup.Application.Abstractions.Persistence;

public interface ISetupReadConnectionFactory
{
    Task<DbConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken = default);
}
