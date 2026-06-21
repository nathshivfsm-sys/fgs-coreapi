using Fgs.Setup.Application.Features.ResolutionCodes.Dtos;

namespace Fgs.Setup.Application.Abstractions.ResolutionCodes;

public interface IResolutionCodeWriteService
{
    Task<ResolutionCodeDetailDto> CreateAsync(ResolutionCodeCreateDto dto, CancellationToken cancellationToken = default);

    Task<ResolutionCodeDetailDto> UpdateAsync(long id, ResolutionCodeUpdateDto dto, CancellationToken cancellationToken = default);

    Task<ResolutionCodeDetailDto> PatchAsync(long id, ResolutionCodePatchDto dto, CancellationToken cancellationToken = default);

    Task<ResolutionCodeDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
