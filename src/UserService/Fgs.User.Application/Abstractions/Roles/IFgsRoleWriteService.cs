using Fgs.User.Application.Features.Roles.Dtos;

namespace Fgs.User.Application.Abstractions.Roles;

public interface IFgsRoleWriteService
{
    Task<FgsRoleDetailDto> CreateAsync(FgsRoleCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsRoleDetailDto> UpdateAsync(long id, FgsRoleUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsRoleDetailDto> PatchAsync(long id, FgsRolePatchDto dto, CancellationToken cancellationToken = default);
}
