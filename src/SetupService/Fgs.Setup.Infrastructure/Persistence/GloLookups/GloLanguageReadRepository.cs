using Dapper;
using Fgs.Setup.Application.Abstractions.GloLookups;
using Fgs.Setup.Application.Abstractions.Persistence;
using Fgs.Setup.Application.Features.GloLookups.Dtos;
using Fgs.Setup.Infrastructure.Database.Schemas;

namespace Fgs.Setup.Infrastructure.Persistence.GloLookups;

internal sealed class GloLanguageReadRepository(ISetupReadConnectionFactory connectionFactory) : IGloLanguageReadRepository
{
    private static readonly string Table = $"{FgsDatabaseSchemas.Glo}.\"GloLanguage\"";

    public async Task<IReadOnlyList<GloLanguageLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var activeFilter = activeOnly ? "AND \"IsActive\" = TRUE" : string.Empty;
        var sql =
            $"""
            SELECT "LanguageCode", "LanguageName", "CultureCode"
            FROM {Table}
            WHERE 1 = 1
              {activeFilter}
            ORDER BY "LanguageName" ASC
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<GloLanguageLookupDto>(
            new CommandDefinition(sql, cancellationToken: cancellationToken));

        return rows.ToList();
    }
}
