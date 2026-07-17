using Fgs.User.Application.Features.Permissions.Dtos;

namespace Fgs.User.Application.Abstractions.Permissions;

public interface IFgsPermissionWriteService
{
    Task<FgsPermissionDetailDto> CreateAsync(
        FgsPermissionCreateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsPermissionDetailDto> UpdateAsync(
        long id,
        FgsPermissionUpdateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsPermissionDetailDto> PatchAsync(
        long id,
        FgsPermissionPatchDto dto,
        CancellationToken cancellationToken = default);
}
