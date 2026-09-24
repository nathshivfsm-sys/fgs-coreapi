using Dapper;
using Fgs.Setup.Application.Abstractions.GloLookups;
using Fgs.Setup.Application.Abstractions.Persistence;
using Fgs.Setup.Application.Features.GloLookups.Dtos;
using Fgs.Setup.Infrastructure.Database.Schemas;

namespace Fgs.Setup.Infrastructure.Persistence.GloLookups;

internal sealed class GloBillingCategoryReadRepository(ISetupReadConnectionFactory connectionFactory)
    : IGloBillingCategoryReadRepository
{
    private static readonly string Table = $"{FgsDatabaseSchemas.Glo}.\"GloBillingCategory\"";

    public async Task<IReadOnlyList<GloBillingCategoryTypeLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        // GloBillingCategory has no IsActive column; activeOnly is accepted for API consistency only.
        _ = activeOnly;
        var sql =
            $"""
            SELECT "BillingCategoryType", "BillingCategoryName", "DisplayOrder"
            FROM {Table}
            ORDER BY "DisplayOrder" ASC, "BillingCategoryName" ASC
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<GloBillingCategoryTypeLookupDto>(
            new CommandDefinition(sql, cancellationToken: cancellationToken));

        return rows.ToList();
    }

    public async Task<bool> ExistsByBillingCategoryTypeAsync(
        string billingCategoryType,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(billingCategoryType))
        {
            return false;
        }

        var sql =
            $"""
            SELECT EXISTS(
                SELECT 1
                FROM {Table}
                WHERE "BillingCategoryType" = @BillingCategoryType
            )
            """;

        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                sql,
                new { BillingCategoryType = billingCategoryType.Trim().ToUpperInvariant() },
                cancellationToken: cancellationToken));
    }
}
