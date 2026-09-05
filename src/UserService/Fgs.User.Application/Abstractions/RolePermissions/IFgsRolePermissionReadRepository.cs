using Fgs.User.Application.Features.RolePermissions.Dtos;

namespace Fgs.User.Application.Abstractions.RolePermissions;

public interface IFgsRolePermissionReadRepository
{
    Task<FgsRolePermissionDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsRolePermissionDetailDto>> ListByRoleIdAsync(
        long fgsRoleId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsRolePermissionLookupDto>> LookupAsync(
        long fgsRoleId,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByRoleIdAndPermissionIdAsync(
        long fgsRoleId,
        long fgsPermissionId,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}
