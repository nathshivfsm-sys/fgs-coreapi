namespace Fgs.User.Application.Features.Auth.Queries.GetAuthMe;

public sealed record AuthMeDto(
    Guid UserId,
    string Email,
    string? EntraObjectId,
    long TenantId,
    long CompanyId,
    IReadOnlyList<string> Roles);
