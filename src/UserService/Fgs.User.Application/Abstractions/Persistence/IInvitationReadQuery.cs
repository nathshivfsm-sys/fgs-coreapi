namespace Fgs.User.Application.Abstractions.Persistence;

public interface IInvitationReadQuery
{
    Task<bool> HasValidInvitationForUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<bool> HasAcceptedInvitationForUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<bool> HasPendingInvitationForNormalizedEmailAsync(
        string normalizedEmail,
        DateTimeOffset nowUtc,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Pending invitation check scoped to the current tenant and company (via user join).
    /// </summary>
    Task<bool> HasPendingInvitationForNormalizedEmailInCurrentTenantCompanyAsync(
        string normalizedEmail,
        DateTimeOffset nowUtc,
        CancellationToken cancellationToken = default);
}
