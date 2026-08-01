using Fgs.User.Application.Features.Users.Dtos;

namespace Fgs.User.Application.Abstractions.Users;

public interface IFgsUserWriteService
{
    Task<FgsUserDetailDto> InviteAsync(FgsUserInviteDto dto, CancellationToken cancellationToken = default);

    Task<FgsUserDetailDto> UpdateAsync(Guid id, FgsUserUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsUserDetailDto> PatchAsync(Guid id, FgsUserPatchDto dto, CancellationToken cancellationToken = default);

    Task<FgsUserDetailDto> ResendInviteAsync(Guid id, CancellationToken cancellationToken = default);
}
