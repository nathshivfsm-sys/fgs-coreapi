using Fgs.User.Application.Features.DataAccesses.Dtos;

namespace Fgs.User.Application.Abstractions.DataAccesses;

public interface IFgsDataAccessWriteService
{
    Task<FgsDataAccessDetailDto> CreateAsync(FgsDataAccessCreateDto dto, CancellationToken cancellationToken = default);

    Task<FgsDataAccessDetailDto> UpdateAsync(long id, FgsDataAccessUpdateDto dto, CancellationToken cancellationToken = default);

    Task<FgsDataAccessDetailDto> PatchAsync(long id, FgsDataAccessPatchDto dto, CancellationToken cancellationToken = default);
}
