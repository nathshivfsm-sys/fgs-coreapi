using Dapper;
using Fgs.Setup.Application.Abstractions.GloLookups;
using Fgs.Setup.Application.Abstractions.Persistence;
using Fgs.Setup.Application.Features.GloLookups.Dtos;
using Fgs.Setup.Infrastructure.Database.Schemas;

namespace Fgs.Setup.Infrastructure.Persistence.GloLookups;

internal sealed class GloCountryReadRepository(ISetupReadConnectionFactory connectionFactory) : IGloCountryReadRepository
{
    private static readonly string Table = $"{FgsDatabaseSchemas.Glo}.\"GloCountry\"";

    public async Task<IReadOnlyList<GloCountryLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var activeFilter = activeOnly ? "AND \"IsActive\" = TRUE" : string.Empty;
        var sql =
            $"""
            SELECT "CountryCode", "CountryName", "CurrencyCode"
            FROM {Table}
            WHERE 1 = 1
              {activeFilter}
            ORDER BY "CountryName" ASC
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<GloCountryLookupDto>(
            new CommandDefinition(sql, cancellationToken: cancellationToken));

        return rows.ToList();
    }
}
