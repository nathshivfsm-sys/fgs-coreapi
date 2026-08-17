using Fgs.User.Application.Features.UserRoles.Dtos;

namespace Fgs.User.Application.Abstractions.UserRoles;

public interface IFgsUserRoleReadRepository
{
    Task<IReadOnlyList<FgsUserRoleDetailDto>> ListByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}
