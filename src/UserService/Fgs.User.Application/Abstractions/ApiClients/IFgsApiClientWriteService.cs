using Fgs.User.Application.Features.ApiClients.Dtos;

namespace Fgs.User.Application.Abstractions.ApiClients;

public interface IFgsApiClientWriteService
{
    Task<FgsApiClientDetailDto> CreateAsync(FgsApiClientCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsApiClientDetailDto> UpdateAsync(long id, FgsApiClientUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsApiClientDetailDto> PatchAsync(long id, FgsApiClientPatchDto dto, CancellationToken cancellationToken = default);
}
