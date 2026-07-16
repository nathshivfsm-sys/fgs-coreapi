using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Abstractions.Security;
using Fgs.User.Domain.Enums;
using Fgs.User.Infrastructure.Database.Schemas;

namespace Fgs.User.Infrastructure.Persistence.Queries;

internal sealed class InvitationReadQuery(
    IUserReadConnectionFactory connectionFactory,
    IEmailNormalizer emailNormalizer) : IInvitationReadQuery
{
    private const string PendingStatus = nameof(InvitationStatus.Pending);
    private const string AcceptedStatus = nameof(InvitationStatus.Accepted);

    private static readonly string ValidInvitationSql = $"""
        SELECT EXISTS(
            SELECT 1
            FROM {EntitySchemaRegistry.QualifyTable("FgsInvitation")}
            WHERE "UserId" = @userId
              AND "Status" IN (@pendingStatus, @acceptedStatus))
        """;

    private static readonly string AcceptedInvitationSql = $"""
        SELECT EXISTS(
            SELECT 1
            FROM {EntitySchemaRegistry.QualifyTable("FgsInvitation")}
            WHERE "UserId" = @userId
              AND "Status" = @acceptedStatus)
        """;

    private static readonly string PendingInvitationsSql = $"""
        SELECT "Email"
        FROM {EntitySchemaRegistry.QualifyTable("FgsInvitation")}
        WHERE "Status" = @pendingStatus AND "ExpiresAtUtc" > @nowUtc
        """;

    public async Task<bool> HasValidInvitationForUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await Dapper.SqlMapper.ExecuteScalarAsync<bool>(
            connection,
            ValidInvitationSql,
            new
            {
                userId,
                pendingStatus = PendingStatus,
                acceptedStatus = AcceptedStatus
            });
    }

    public async Task<bool> HasAcceptedInvitationForUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await Dapper.SqlMapper.ExecuteScalarAsync<bool>(
            connection,
            AcceptedInvitationSql,
            new
            {
                userId,
                acceptedStatus = AcceptedStatus
            });
    }

    public async Task<bool> HasPendingInvitationForNormalizedEmailAsync(
        string normalizedEmail,
        DateTimeOffset nowUtc,
        CancellationToken cancellationToken = default)
    {
        await using var connection = await connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var emails = await Dapper.SqlMapper.QueryAsync<string>(
            connection,
            PendingInvitationsSql,
            new
            {
                pendingStatus = PendingStatus,
                nowUtc
            });

        return emails.Any(email =>
            emailNormalizer.Normalize(email) == normalizedEmail);
    }
}
