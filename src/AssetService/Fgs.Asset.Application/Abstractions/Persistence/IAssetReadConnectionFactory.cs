using System.Data.Common;

namespace Fgs.Asset.Application.Abstractions.Persistence;

public interface IAssetReadConnectionFactory
{
    Task<DbConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken = default);
}
