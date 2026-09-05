using Fgs.User.Application.Features.RoleDataAccesses.Dtos;

namespace Fgs.User.Application.Abstractions.RoleDataAccesses;

public interface IFgsRoleDataAccessReadRepository
{
    Task<FgsRoleDataAccessDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsRoleDataAccessDetailDto>> ListByRoleIdAsync(
        long fgsRoleId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsRoleDataAccessLookupDto>> LookupAsync(
        long fgsRoleId,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByRoleIdAndDataAccessIdAsync(
        long fgsRoleId,
        long fgsDataAccessId,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}
