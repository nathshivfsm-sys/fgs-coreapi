using Fgs.Contracts.Auth;

namespace Fgs.Security.UserAuth;

public interface IUserAuthProfileStore
{
    Task<UserAuthProfileDto?> GetOrLoadAsync(
        string entraObjectId,
        CancellationToken cancellationToken = default);

    Task SetAsync(
        UserAuthProfileDto profile,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes cached profile entries. Call when user status, tenant, company, or role assignments change
    /// so the next request reloads roles and scope from the database.
    /// </summary>
    Task InvalidateAsync(
        Guid userId,
        string? entraObjectId = null,
        CancellationToken cancellationToken = default);
}
