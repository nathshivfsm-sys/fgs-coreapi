namespace Fgs.User.Application.Abstractions.Persistence;

public interface IInvitationReadQuery
{
    Task<bool> HasValidInvitationForUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<bool> HasPendingInvitationForNormalizedEmailAsync(
        string normalizedEmail,
        DateTimeOffset nowUtc,
        CancellationToken cancellationToken = default);
}
