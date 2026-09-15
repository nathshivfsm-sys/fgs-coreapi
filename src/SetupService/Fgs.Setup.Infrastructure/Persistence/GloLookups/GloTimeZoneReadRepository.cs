using Dapper;
using Fgs.Setup.Application.Abstractions.GloLookups;
using Fgs.Setup.Application.Abstractions.Persistence;
using Fgs.Setup.Application.Features.GloLookups.Dtos;
using Fgs.Setup.Infrastructure.Database.Schemas;

namespace Fgs.Setup.Infrastructure.Persistence.GloLookups;

internal sealed class GloTimeZoneReadRepository(ISetupReadConnectionFactory connectionFactory) : IGloTimeZoneReadRepository
{
    private static readonly string Table = $"{FgsDatabaseSchemas.Glo}.\"GloTimeZone\"";

    public async Task<IReadOnlyList<GloTimeZoneLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var activeFilter = activeOnly ? "AND \"IsActive\" = TRUE" : string.Empty;
        var sql =
            $"""
            SELECT "Id", "TimeZoneCode", "Name", "IsActive"
            FROM {Table}
            WHERE 1 = 1
              {activeFilter}
            ORDER BY "Name" ASC
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<GloTimeZoneLookupDto>(
            new CommandDefinition(sql, cancellationToken: cancellationToken));

        return rows.ToList();
    }
}
