using Fgs.User.Application.Features.RoleDataAccesses.Dtos;

namespace Fgs.User.Application.Abstractions.RoleDataAccesses;

public interface IFgsRoleDataAccessReadRepository
{
    Task<IReadOnlyList<FgsRoleDataAccessDetailDto>> ListByRoleIdAsync(
        long fgsRoleId,
        CancellationToken cancellationToken = default);
}
