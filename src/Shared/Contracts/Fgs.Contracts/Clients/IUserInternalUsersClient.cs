using Refit;

namespace Fgs.Contracts.Clients;

/// <summary>
/// Internal HTTP client for tenant-scoped user lookups owned by UserService.
/// </summary>
public interface IUserInternalUsersClient
{
    [Get("/api/v1/internal/users/ids-by-roles")]
    Task<Fgs.Contracts.Api.ApiResponse<IReadOnlyList<Guid>>> GetUserIdsByRolesAsync(
        [Query(CollectionFormat.Multi)] IEnumerable<long> roleIds,
        [Header("X-Tenant-Id")] string tenantId,
        [Header("X-Company-Id")] string companyId,
        CancellationToken cancellationToken = default);
}
