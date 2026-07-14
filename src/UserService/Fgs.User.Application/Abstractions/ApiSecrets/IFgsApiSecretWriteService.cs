using Fgs.User.Application.Features.ApiSecrets.Dtos;

namespace Fgs.User.Application.Abstractions.ApiSecrets;

public interface IFgsApiSecretWriteService
{
    Task<FgsApiSecretCreateResultDto> CreateAsync(FgsApiSecretCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsApiSecretDetailDto> PatchAsync(long id, FgsApiSecretPatchDto dto, CancellationToken cancellationToken = default);

    Task<FgsApiSecretDetailDto> RevokeAsync(long id, CancellationToken cancellationToken = default);
}
