using Refit;

namespace Fgs.Contracts.Clients;

/// <summary>
/// Response payload for GET /api/v1/auth/me (serialized inside <see cref="ApiResponse{T}"/>).
/// </summary>
public sealed record FgsAuthMeDto(
    Guid UserId,
    string Email,
    string EntraObjectId,
    long TenantId,
    long CompanyId,
    IReadOnlyList<string> Roles);

public interface IFgsClaimsClient
{
    [Get("/api/v1/auth/me")]
    Task<Fgs.Contracts.Api.ApiResponse<FgsAuthMeDto>> GetMeAsync(
        [Header("Authorization")] string authorization,
        CancellationToken cancellationToken = default);
}
