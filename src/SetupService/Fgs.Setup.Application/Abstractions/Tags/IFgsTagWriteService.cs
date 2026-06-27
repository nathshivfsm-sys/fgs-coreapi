using Fgs.Setup.Application.Features.Tags.Dtos;

namespace Fgs.Setup.Application.Abstractions.Tags;

public interface IFgsTagWriteService
{
    Task<FgsTagDetailDto> CreateAsync(FgsTagCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsTagDetailDto> UpdateAsync(long id, FgsTagUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsTagDetailDto> PatchAsync(long id, FgsTagPatchDto dto, CancellationToken cancellationToken = default);

    Task<FgsTagDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
