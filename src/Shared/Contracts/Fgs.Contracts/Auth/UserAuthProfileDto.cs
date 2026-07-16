namespace Fgs.Contracts.Auth;

public sealed record PublicEndpointAuthDto(
    string EndpointType,
    string EnvironmentCode,
    string BaseUrl,
    string? DisplayName);

public sealed record UserAuthProfileDto(
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
    IReadOnlyList<PublicEndpointAuthDto> PublicEndpoints);
