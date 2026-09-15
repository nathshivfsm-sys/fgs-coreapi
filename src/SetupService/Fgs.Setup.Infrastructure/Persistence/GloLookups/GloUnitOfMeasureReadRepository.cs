using Dapper;
using Fgs.Setup.Application.Abstractions.GloLookups;
using Fgs.Setup.Application.Abstractions.Persistence;
using Fgs.Setup.Application.Features.GloLookups.Dtos;
using Fgs.Setup.Infrastructure.Database.Schemas;

namespace Fgs.Setup.Infrastructure.Persistence.GloLookups;

internal sealed class GloUnitOfMeasureReadRepository(ISetupReadConnectionFactory connectionFactory)
    : IGloUnitOfMeasureReadRepository
{
    private static readonly string Table = $"{FgsDatabaseSchemas.Glo}.\"GloUnitOfMeasure\"";

    public async Task<IReadOnlyList<GloUnitOfMeasureLookupDto>> LookupAsync(
        string? unitType = null,
        bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var where = new List<string>();
        if (activeOnly)
        {
            where.Add("\"IsActive\" = TRUE");
        }

        if (!string.IsNullOrWhiteSpace(unitType))
        {
            where.Add("\"UnitType\" = @UnitType");
        }

        var whereClause = where.Count > 0 ? "WHERE " + string.Join(" AND ", where) : string.Empty;
        var sql =
            $"""
            SELECT "Id", "UnitCode", "Name", "Abbreviation", "UnitType"
            FROM {Table}
            {whereClause}
            ORDER BY "DisplayOrder" ASC, "Name" ASC
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<GloUnitOfMeasureLookupDto>(
            new CommandDefinition(
                sql,
                new { UnitType = unitType?.Trim() },
                cancellationToken: cancellationToken));

        return rows.ToList();
    }
}
