using Fgs.User.Application.Features.UserRoles.Dtos;

namespace Fgs.User.Application.Abstractions.UserRoles;

public interface IFgsUserRoleWriteService
{
    Task<FgsUserRoleDetailDto> CreateAsync(FgsUserRoleCreateDto dto, CancellationToken cancellationToken = default);

    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}
