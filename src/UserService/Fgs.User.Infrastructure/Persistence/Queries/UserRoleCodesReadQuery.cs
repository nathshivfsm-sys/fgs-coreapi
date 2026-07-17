using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Infrastructure.Database.Schemas;

namespace Fgs.User.Infrastructure.Persistence.Queries;

internal sealed class UserRoleCodesReadQuery(IUserReadConnectionFactory connectionFactory)
    : IUserRoleCodesReadQuery
{
    private static readonly string Sql = $"""
        SELECT r."RoleCode"
        FROM {EntitySchemaRegistry.QualifyTable("FgsUserRole")} ur
        INNER JOIN {EntitySchemaRegistry.QualifyTable("FgsRole")} r ON r."Id" = ur."FgsRoleId"
        WHERE ur."UserId" = @userId
          AND r."IsActive" = true
        """;

    public async Task<IReadOnlyList<string>> GetRoleCodesForUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await Dapper.SqlMapper.QueryAsync<string>(
            connection,
            Sql,
            new { userId });

        return rows.ToList();
    }
}
