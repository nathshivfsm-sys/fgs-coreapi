using Fgs.Setup.Application.Features.TitlesOfCourtesy.Dtos;

namespace Fgs.Setup.Application.Abstractions.TitlesOfCourtesy;

public interface ITitleOfCourtesyWriteService
{
    Task<TitleOfCourtesyDetailDto> CreateAsync(TitleOfCourtesyCreateDto dto, CancellationToken cancellationToken = default);

    Task<TitleOfCourtesyDetailDto> UpdateAsync(long id, TitleOfCourtesyUpdateDto dto, CancellationToken cancellationToken = default);

    Task<TitleOfCourtesyDetailDto> PatchAsync(long id, TitleOfCourtesyPatchDto dto, CancellationToken cancellationToken = default);

    Task<TitleOfCourtesyDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
