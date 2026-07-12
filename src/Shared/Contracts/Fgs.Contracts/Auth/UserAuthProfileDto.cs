namespace Fgs.Contracts.Auth;

public sealed record UserAuthProfileDto(
    Guid UserId,
    string Email,
    string? EntraObjectId,
    long TenantId,
    long CompanyId,
    bool IsActive,
    bool IsDeleted,
    IReadOnlyList<string> Roles);
