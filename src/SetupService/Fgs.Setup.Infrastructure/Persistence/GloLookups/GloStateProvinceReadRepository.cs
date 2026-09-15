using Dapper;
using Fgs.Setup.Application.Abstractions.GloLookups;
using Fgs.Setup.Application.Abstractions.Persistence;
using Fgs.Setup.Application.Features.GloLookups.Dtos;
using Fgs.Setup.Infrastructure.Database.Schemas;

namespace Fgs.Setup.Infrastructure.Persistence.GloLookups;

internal sealed class GloStateProvinceReadRepository(ISetupReadConnectionFactory connectionFactory)
    : IGloStateProvinceReadRepository
{
    private static readonly string Table = $"{FgsDatabaseSchemas.Glo}.\"GloStateProvince\"";

    public async Task<IReadOnlyList<GloStateProvinceLookupDto>> LookupAsync(
        string countryCode,
        bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var activeFilter = activeOnly ? "AND \"IsActive\" = TRUE" : string.Empty;
        var sql =
            $"""
            SELECT "Id", "CountryCode", "StateProvinceCode", "StateProvinceName"
            FROM {Table}
            WHERE "CountryCode" = @CountryCode
              {activeFilter}
            ORDER BY "StateProvinceName" ASC
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<GloStateProvinceLookupDto>(
            new CommandDefinition(
                sql,
                new { CountryCode = countryCode.Trim().ToUpperInvariant() },
                cancellationToken: cancellationToken));

        return rows.ToList();
    }
}
