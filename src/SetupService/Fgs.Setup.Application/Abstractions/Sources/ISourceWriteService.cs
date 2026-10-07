using Fgs.Setup.Application.Features.Sources.Dtos;

namespace Fgs.Setup.Application.Abstractions.Sources;

public interface ISourceWriteService
{
    Task<SourceDetailDto> CreateAsync(SourceCreateDto dto, CancellationToken cancellationToken = default);

    Task<SourceDetailDto> UpdateAsync(long id, SourceUpdateDto dto, CancellationToken cancellationToken = default);

    Task<SourceDetailDto> PatchAsync(long id, SourcePatchDto dto, CancellationToken cancellationToken = default);

    Task<SourceDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
