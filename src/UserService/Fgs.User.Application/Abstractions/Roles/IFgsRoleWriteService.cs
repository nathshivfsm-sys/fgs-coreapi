using Fgs.User.Application.Features.Roles.Dtos;

namespace Fgs.User.Application.Abstractions.Roles;

public interface IFgsRoleWriteService
{
    Task<FgsRoleDetailDto> CreateAsync(FgsRoleCreateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a non-built-in copy of an existing role (including permissions, menus, and data access).
    /// Sets <c>ParentRoleId</c> to the source role.
    /// </summary>
    Task<FgsRoleDetailDto> CloneAsync(
        long sourceRoleId,
        FgsRoleCloneDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsRoleDetailDto> UpdateAsync(long id, FgsRoleUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsRoleDetailDto> PatchAsync(long id, FgsRolePatchDto dto, CancellationToken cancellationToken = default);
}
