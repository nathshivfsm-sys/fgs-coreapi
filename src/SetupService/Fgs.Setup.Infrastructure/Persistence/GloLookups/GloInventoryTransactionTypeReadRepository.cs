using Dapper;
using Fgs.Setup.Application.Abstractions.GloLookups;
using Fgs.Setup.Application.Abstractions.Persistence;
using Fgs.Setup.Application.Features.GloLookups.Dtos;
using Fgs.Setup.Infrastructure.Database.Schemas;

namespace Fgs.Setup.Infrastructure.Persistence.GloLookups;

internal sealed class GloInventoryTransactionTypeReadRepository(ISetupReadConnectionFactory connectionFactory)
    : IGloInventoryTransactionTypeReadRepository
{
    private static readonly string Table = $"{FgsDatabaseSchemas.Glo}.\"GloInventoryTransactionType\"";

    public async Task<IReadOnlyList<GloInventoryTransactionTypeLookupDto>> LookupAsync(
        int? sourceTypeId = null,
        bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var where = new List<string>();
        if (activeOnly)
        {
            where.Add("\"IsActive\" = TRUE");
        }

        if (sourceTypeId.HasValue)
        {
            where.Add("\"InventoryTransactionSourceTypeId\" = @SourceTypeId");
        }

        var whereClause = where.Count > 0 ? "WHERE " + string.Join(" AND ", where) : string.Empty;
        var sql =
            $"""
            SELECT "Id", "InventoryTransactionSourceTypeId", "Code", "Name"
            FROM {Table}
            {whereClause}
            ORDER BY "SortOrder" ASC, "Name" ASC
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<GloInventoryTransactionTypeLookupDto>(
            new CommandDefinition(
                sql,
                new { SourceTypeId = sourceTypeId },
                cancellationToken: cancellationToken));

        return rows.ToList();
    }
}
