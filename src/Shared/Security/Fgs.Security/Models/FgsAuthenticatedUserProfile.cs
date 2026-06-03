namespace Fgs.Security.Models;

public sealed record FgsAuthenticatedUserProfile(
    Guid UserId,
    string Email,
    string EntraObjectId,
    long TenantId,
    long CompanyId,
    IReadOnlyList<string> Roles);
