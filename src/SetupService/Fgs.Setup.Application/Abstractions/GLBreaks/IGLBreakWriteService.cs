using Fgs.Setup.Application.Features.GLBreaks.Dtos;

namespace Fgs.Setup.Application.Abstractions.GLBreaks;

public interface IGLBreakWriteService
{
    Task<GLBreakDetailDto> CreateAsync(GLBreakCreateDto dto, CancellationToken cancellationToken = default);

    Task<GLBreakDetailDto> UpdateAsync(long id, GLBreakUpdateDto dto, CancellationToken cancellationToken = default);

    Task<GLBreakDetailDto> PatchAsync(long id, GLBreakPatchDto dto, CancellationToken cancellationToken = default);

    Task<GLBreakDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
