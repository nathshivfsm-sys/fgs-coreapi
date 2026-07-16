namespace Fgs.User.Application.Abstractions.Identity;

public interface IFgsUserProfileResolver
{
    Task<FgsUserProfile?> ResolveByEntraObjectIdAsync(
        string entraObjectId,
        CancellationToken cancellationToken = default);

    Task<FgsUserProfile?> ResolveBySignupEmailAsync(
        string normalizedEmail,
        CancellationToken cancellationToken = default);

    Task<FgsUserProfile?> ResolveForEntraConnectorAsync(
        string? objectId,
        string? email,
        CancellationToken cancellationToken = default);
}

public sealed record FgsUserProfile(
    Guid UserId,
    string Email,
    string? EntraObjectId,
    long TenantId,
    long CompanyId,
    bool IsActive,
    bool IsDeleted,
    IReadOnlyList<string> Roles,
    IReadOnlyList<string> Permissions,
    IReadOnlyList<string> DataAccess,
    IReadOnlyList<Fgs.Contracts.Auth.PublicEndpointAuthDto> PublicEndpoints);
