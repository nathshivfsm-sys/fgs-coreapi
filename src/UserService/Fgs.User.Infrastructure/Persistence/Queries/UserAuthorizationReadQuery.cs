using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Infrastructure.Database.Schemas;

namespace Fgs.User.Infrastructure.Persistence.Queries;

internal sealed class UserAuthorizationReadQuery(IUserReadConnectionFactory connectionFactory)
    : IUserAuthorizationReadQuery
{
    private static readonly string PermissionSql = $"""
        SELECT DISTINCT p."PermissionCode"
        FROM {EntitySchemaRegistry.QualifyTable("FgsUserRole")} ur
        INNER JOIN {EntitySchemaRegistry.QualifyTable("FgsRole")} r
            ON r."Id" = ur."FgsRoleId" AND r."IsActive" = true
        INNER JOIN {EntitySchemaRegistry.QualifyTable("FgsRolePermission")} rp
            ON rp."FgsRoleId" = r."Id"
        INNER JOIN {EntitySchemaRegistry.QualifyTable("FgsPermission")} p
            ON p."Id" = rp."FgsPermissionId" AND p."IsActive" = true
        WHERE ur."UserId" = @userId
        ORDER BY p."PermissionCode"
        """;

    private static readonly string DataAccessSql = $"""
        SELECT DISTINCT d."DataAccessCode"
        FROM {EntitySchemaRegistry.QualifyTable("FgsUserRole")} ur
        INNER JOIN {EntitySchemaRegistry.QualifyTable("FgsRole")} r
            ON r."Id" = ur."FgsRoleId" AND r."IsActive" = true
        INNER JOIN {EntitySchemaRegistry.QualifyTable("FgsRoleDataAccess")} rd
            ON rd."FgsRoleId" = r."Id"
        INNER JOIN {EntitySchemaRegistry.QualifyTable("FgsDataAccess")} d
            ON d."Id" = rd."FgsDataAccessId" AND d."IsActive" = true
        WHERE ur."UserId" = @userId
        ORDER BY d."DataAccessCode"
        """;

    public async Task<IReadOnlyList<string>> GetPermissionCodesForUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await Dapper.SqlMapper.QueryAsync<string>(
            connection,
            PermissionSql,
            new { userId });
        return rows.ToList();
    }

    public async Task<IReadOnlyList<string>> GetDataAccessCodesForUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await Dapper.SqlMapper.QueryAsync<string>(
            connection,
            DataAccessSql,
            new { userId });
        return rows.ToList();
    }
}
