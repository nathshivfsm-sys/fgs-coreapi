namespace Fgs.User.Application.Abstractions.Identity;

public interface IFgsUserProfileResolver
{
    Task<FgsUserProfile?> ResolveByEntraObjectIdAsync(
        string entraObjectId,
        CancellationToken cancellationToken = default);
}

public sealed record FgsUserProfile(
    Guid UserId,
    string Email,
    string EntraObjectId,
    long TenantId,
    long CompanyId,
    IReadOnlyList<string> Roles);
