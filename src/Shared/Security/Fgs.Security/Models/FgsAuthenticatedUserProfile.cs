namespace Fgs.Security.Models;

public sealed record FgsAuthenticatedUserProfile(
    Guid UserId,
    string Email,
    string EntraObjectId,
    IReadOnlyList<string> Roles);
