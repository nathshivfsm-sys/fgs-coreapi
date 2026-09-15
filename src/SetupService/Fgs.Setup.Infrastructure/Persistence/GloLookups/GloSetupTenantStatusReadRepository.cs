using Dapper;
using Fgs.Setup.Application.Abstractions.GloLookups;
using Fgs.Setup.Application.Abstractions.Persistence;
using Fgs.Setup.Application.Features.GloLookups.Dtos;
using Fgs.Setup.Infrastructure.Database.Schemas;

namespace Fgs.Setup.Infrastructure.Persistence.GloLookups;

internal sealed class GloSetupTenantStatusReadRepository(ISetupReadConnectionFactory connectionFactory)
    : IGloSetupTenantStatusReadRepository
{
    private static readonly string Table = $"{FgsDatabaseSchemas.Glo}.\"GloSetupTenantStatus\"";

    public async Task<IReadOnlyList<GloSetupTenantStatusLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var activeFilter = activeOnly ? "AND \"IsActive\" = TRUE" : string.Empty;
        var sql =
            $"""
            SELECT "Id", "Name"
            FROM {Table}
            WHERE 1 = 1
              {activeFilter}
            ORDER BY "Name" ASC
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<GloSetupTenantStatusLookupDto>(
            new CommandDefinition(sql, cancellationToken: cancellationToken));

        return rows.ToList();
    }
}
