using Fgs.Setup.Application.Features.FgsBusinessTypes.Dtos;

namespace Fgs.Setup.Application.Abstractions.FgsBusinessTypes;

public interface IFgsBusinessTypeWriteService
{
    Task<FgsBusinessTypeDetailDto> CreateAsync(FgsBusinessTypeCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsBusinessTypeDetailDto> UpdateAsync(long id, FgsBusinessTypeUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsBusinessTypeDetailDto> PatchAsync(long id, FgsBusinessTypePatchDto dto, CancellationToken cancellationToken = default);

    Task<FgsBusinessTypeDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
