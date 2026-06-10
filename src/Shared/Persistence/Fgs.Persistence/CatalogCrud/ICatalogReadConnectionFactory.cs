using Npgsql;

namespace Fgs.Persistence.CatalogCrud;

public interface ICatalogReadConnectionFactory
{
    Task<NpgsqlConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken = default);
}
