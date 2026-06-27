using System.Data.Common;

namespace Fgs.Inventory.Application.Abstractions.Persistence;

public interface IInventoryReadConnectionFactory
{
    Task<DbConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken = default);
}
