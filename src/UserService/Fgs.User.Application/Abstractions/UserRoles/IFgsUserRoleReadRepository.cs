using Fgs.User.Application.Features.UserRoles.Dtos;

namespace Fgs.User.Application.Abstractions.UserRoles;

public interface IFgsUserRoleReadRepository
{
    Task<FgsUserRoleDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsUserRoleDetailDto>> ListByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FgsUserRoleLookupDto>> LookupAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByUserIdAndRoleIdAsync(
        Guid userId,
        long fgsRoleId,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}
