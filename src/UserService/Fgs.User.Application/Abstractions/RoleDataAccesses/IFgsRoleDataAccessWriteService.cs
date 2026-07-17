using Fgs.User.Application.Features.RoleDataAccesses.Dtos;

namespace Fgs.User.Application.Abstractions.RoleDataAccesses;

public interface IFgsRoleDataAccessWriteService
{
    Task<FgsRoleDataAccessDetailDto> CreateAsync(FgsRoleDataAccessCreateDto dto, CancellationToken cancellationToken = default);

    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}
