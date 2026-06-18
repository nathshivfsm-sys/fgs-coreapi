using Refit;

namespace Fgs.Contracts.Clients;

/// <summary>
/// Response payload for GET /api/v1/auth/me (serialized inside <see cref="ApiResponse{T}"/>).
/// </summary>
public sealed record FgsAuthMeDto(
    Guid UserId,
    string Email,
    string EntraObjectId,
    IReadOnlyList<string> Roles);

public interface IFgsClaimsClient
{
    [Get("/api/v1/auth/me")]
    Task<Fgs.Contracts.Api.ApiResponse<FgsAuthMeDto>> GetMeAsync(
        [Header("Authorization")] string authorization,
        [Header("X-Tenant-Id")] long? tenantId,
        [Header("X-Company-Id")] long? companyId,
        CancellationToken cancellationToken = default);

    [Get("/api/v1/auth/validate")]
    Task<Fgs.Contracts.Api.ApiResponse<object>> ValidateUserAsync(
        [Header("Authorization")] string authorization,
        [Header("X-Tenant-Id")] long? tenantId,
        [Header("X-Company-Id")] long? companyId,
        CancellationToken cancellationToken = default);
}
