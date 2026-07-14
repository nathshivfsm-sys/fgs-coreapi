using Fgs.User.Application.Features.RolePermissions.Dtos;

namespace Fgs.User.Application.Abstractions.RolePermissions;

public interface IFgsRolePermissionWriteService
{
    Task<FgsRolePermissionDetailDto> CreateAsync(FgsRolePermissionCreateDto dto, CancellationToken cancellationToken = default);

    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}
