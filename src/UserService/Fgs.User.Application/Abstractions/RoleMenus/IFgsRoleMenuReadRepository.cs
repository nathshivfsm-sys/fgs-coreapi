using Fgs.User.Application.Features.RoleMenus.Dtos;

namespace Fgs.User.Application.Abstractions.RoleMenus;

public interface IFgsRoleMenuReadRepository
{
    Task<FgsRoleMenuDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsRoleMenuDetailDto>> ListByRoleIdAsync(
        long roleId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsRoleMenuLookupDto>> LookupAsync(
        long roleId,
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByRoleMenuAsync(
        long roleId,
        int menuId,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}
